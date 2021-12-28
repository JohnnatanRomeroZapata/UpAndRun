using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UpAndRun.Data;
using UpAndRun.Models;
using UpAndRun.Models.ViewModels;
using UpAndRun.Utility;

namespace UpAndRun.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UpAndRunDbContext _upAndRunDbContext;

        public HomeController(ILogger<HomeController> logger, UpAndRunDbContext upAndRunDbContext)
        {
            _logger = logger;
            _upAndRunDbContext = upAndRunDbContext;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                ListOfProducts = _upAndRunDbContext.Products.Include(p => p.Category)
                                                            .Include(p => p.ApplicationType),
                ListOfCategories = _upAndRunDbContext.Categories
            };

            return View(homeVM);
        }


        [HttpGet]
        public IActionResult ProductDetails(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart) != null && HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart);
            }

            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _upAndRunDbContext.Products.Include(p => p.Category).Include(p => p.ApplicationType).Where(p => p.Id == id).FirstOrDefault(),
                ExistInCart = false
            };

            foreach (var item in shoppingCartList)
            {
                if(item.ProductId == id)
                {
                    detailsVM.ExistInCart = true;
                }
            }

            return View(detailsVM);
        }

        [HttpPost, ActionName("ProductDetails")]
        public IActionResult ProductDetailsPost(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if(HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart) != null && HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart);
            }

            shoppingCartList.Add(new ShoppingCart { ProductId = id });

            HttpContext.Session.Set(WebConstant.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart) != null && HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart);
            }

            var itemToRemove = shoppingCartList.SingleOrDefault(p => p.ProductId == id);

            if(itemToRemove != null)
            {
                shoppingCartList.Remove(itemToRemove);
            }

            HttpContext.Session.Set(WebConstant.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
