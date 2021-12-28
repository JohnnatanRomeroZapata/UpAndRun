using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UpAndRun.Data;
using UpAndRun.Models;

namespace UpAndRun.Controllers
{
    public class CategoryController : Controller
    {
        private readonly UpAndRunDbContext _upAndRunDbContext;

        public CategoryController(UpAndRunDbContext upAndRunDbContext)
        {
            _upAndRunDbContext = upAndRunDbContext;
        }

        [HttpGet]
        public IActionResult IndexCategory()
        {
            IEnumerable<Category> listOfcategories = _upAndRunDbContext.Categories;

            return View(listOfcategories);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _upAndRunDbContext.Categories.Add(category);
                _upAndRunDbContext.SaveChanges();
                return RedirectToAction("IndexCategory");
            }

            return View(category);

        }

        [HttpGet]
        public IActionResult UpdateCategory(int? Id)
        {
            if (Id is null || Id == 0)
            {
                return NotFound();
            }

            Category category = _upAndRunDbContext.Categories.Find(Id);

            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _upAndRunDbContext.Categories.Update(category);
                _upAndRunDbContext.SaveChanges();
                return RedirectToAction("IndexCategory");
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult DeleteCategory(int? Id)
        {
            if (Id is null || Id == 0)
            {
                return NotFound();
            }

            Category category = _upAndRunDbContext.Categories.Find(Id);

            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeleteCategoryPost(int? Id)
        {
            if (Id is null || Id == 0)
            {
                return NotFound();
            }

            Category categoryToDelete = _upAndRunDbContext.Categories.Find(Id);

            if (categoryToDelete is null)
            {
                return NotFound();
            }
            _upAndRunDbContext.Categories.Remove(categoryToDelete);
            _upAndRunDbContext.SaveChanges();
            return RedirectToAction("IndexCategory");
        }
    }
}
