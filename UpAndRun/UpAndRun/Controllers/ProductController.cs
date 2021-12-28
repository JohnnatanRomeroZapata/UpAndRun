using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UpAndRun.Data;
using UpAndRun.Models;
using UpAndRun.Models.ViewModels;

namespace UpAndRun.Controllers
{
    public class ProductController : Controller
    {
        private readonly UpAndRunDbContext _upAndRunDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(UpAndRunDbContext upAndRunDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _upAndRunDbContext = upAndRunDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult IndexProduct()
        {
            IEnumerable<Product> productList = _upAndRunDbContext.Products
                                               .Include(p => p.Category)
                                               .Include(p => p.ApplicationType);

            return View(productList);
        }

        [HttpGet]
        public IActionResult CreateUpdateProduct(int? id)
        {
            ProductVM productVM = new ProductVM();

            productVM.Product = new Product();

            productVM.CategorySelectList = _upAndRunDbContext.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            });

            productVM.ApplicationTypeSelectList = _upAndRunDbContext.ApplicationTypes.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.ApplicationName
            });

            if (ProductToBeCreated(id))
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _upAndRunDbContext.Products.Find(id);

                if (productVM.Product is null)
                {
                    return NotFound();
                }

                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdateProduct(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (ProductToBeCreated(productVM.Product.Id))
                {
                    InsertProductInDb(webRootPath, file, productVM);
                }
                else
                {
                    Product productFromDb = _upAndRunDbContext.Products.AsNoTracking().FirstOrDefault(p => p.Id == productVM.Product.Id);

                    if (hasNotImage(file))
                    {
                        string imageRootPath = webRootPath + WebConstant.ImagePath;
                        string oldFile = Path.Combine(imageRootPath, productFromDb.ImageUrl);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        productVM.Product.ImageUrl = SaveImageAndReturnItsName(webRootPath, file);
                    }
                    else
                    {
                        productVM.Product.ImageUrl = productFromDb.ImageUrl;
                    }

                    _upAndRunDbContext.Products.Update(productVM.Product);
                }

                _upAndRunDbContext.SaveChanges();

                return RedirectToAction("IndexProduct");
            }

            productVM.CategorySelectList = _upAndRunDbContext.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CategoryName
            });

            return View(productVM);
        }

        private bool ProductToBeCreated(int? id)
        {
            if (id is null || id == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void InsertProductInDb(string webRootPath, IFormFileCollection file, ProductVM productVM)
        {
            string imageNameAndItsExtension = SaveImageAndReturnItsName(webRootPath, file);

            productVM.Product.ImageUrl = imageNameAndItsExtension;

            _upAndRunDbContext.Products.Add(productVM.Product);
        }

        private string SaveImageAndReturnItsName(string webRootPath, IFormFileCollection file)
        {
            string imageRootPath = webRootPath + WebConstant.ImagePath;
            string fileName = Guid.NewGuid().ToString();
            string fileExtension = Path.GetExtension(file[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(imageRootPath, fileName + fileExtension), FileMode.Create))
            {
                file[0].CopyTo(fileStream);
            }

            return fileName + fileExtension;
        }

        private bool hasNotImage(IFormFileCollection file)
        {
            return file.Count == 0;
        }

        [HttpGet]
        public IActionResult DeleteProduct(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            Product productToDelete = _upAndRunDbContext.Products.Include(p => p.Category).Include(p => p.ApplicationType).FirstOrDefault(p => p.Id == id);

            if(productToDelete is null)
            {
                return NotFound();
            }

            return View(productToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProductPost(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            Product productToDelete = _upAndRunDbContext.Products.Find(id);

            if (productToDelete is null)
            {
                return NotFound();
            }

            string imageRootPath = _webHostEnvironment.WebRootPath + WebConstant.ImagePath;
            string oldFile = Path.Combine(imageRootPath, productToDelete.ImageUrl);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _upAndRunDbContext.Products.Remove(productToDelete);
            _upAndRunDbContext.SaveChanges();

            return RedirectToAction("IndexProduct");
        }
    }
}
