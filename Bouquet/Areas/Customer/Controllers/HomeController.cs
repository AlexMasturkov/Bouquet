using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using Bouquet.Models.ViewModels;
using Bouquet.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Bouquet.Area.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(string id ="")
        {
            IEnumerable<Product> productListDb = _unitOfWork.Product.GetAll(includeProperties: "Category,EventType");
            var productList = productListDb;
            if(id != "")
            {
                productList = productListDb.Where(p => p.Category.Name.Contains(id.Substring(0,3)) || p.EventType.Name.Contains(id.Substring(0, 3))|| p.Name.Contains(id.Substring(0, 3)));
            }
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim!=null)
            {
                var count = _unitOfWork.ShoppingCart
                .GetAll(c => c.ApplicationUserId == claim.Value)
                .ToList().Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);
            }
            return View(productList.ToList());
        }

        public IActionResult IndexJS()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,EventType");
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart
                .GetAll(c => c.ApplicationUserId == claim.Value)
                .ToList().Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);
            }
            return View(productList);
        }

        public IActionResult Details(int id)
        {
            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,EventType");
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = productFromDb,
                ProductId = productFromDb.Id
            };
            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart cart)
        {
            cart.Id = 0;
            if(!ModelState.IsValid)
            {
                //we can add to cart
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cart.ApplicationUserId = claim.Value;

           

                ShoppingCart shoppingCartDb = _unitOfWork.ShoppingCart
                    .GetFirstOrDefault(u => u.ApplicationUserId == cart.ApplicationUserId && u.ProductId==cart.ProductId,includeProperties:"Product");


                if (shoppingCartDb == null)
                {
                    _unitOfWork.ShoppingCart.Add(cart);
                }
                else
                {
                    shoppingCartDb.Count += cart.Count;
                    //_unitOfWork.ShoppingCart.Update(cart);
                }
                _unitOfWork.Save();

                var count = _unitOfWork.ShoppingCart
                   .GetAll(c => c.ApplicationUserId == cart.ApplicationUserId)
                   .ToList().Count();

                //HttpContext.Session.SetObject(SD.ssShoppingCart, cart);
                //var obj = HttpContext.Session.GetObject<ShoppingCart>(SD.ssShoppingCart);
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);

                return RedirectToAction(nameof(Index));

            }
            else
            {
                //return to view
                var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == cart.ProductId, includeProperties: "Category,EventType");
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    Product = productFromDb,
                    ProductId = productFromDb.Id
                };
                return View(shoppingCart);   
            }           
            
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
        #region API CALLS
        [HttpGet]
        public IActionResult GetAllIndex(int? id)
        {
            var allProducts = _unitOfWork.Product.GetAll(includeProperties: "Category,EventType").ToList();           
            foreach (var item in allProducts)
            {
                item.Description = SD.ConvertToRawHtml(item.Description);
            }
          
            if (id == 1)
            {
                return Json(new { data = allProducts.Where(p => p.Category.Name == "Rosas").ToList() });
            }
            else if (id == 11)
            {
                return Json(new { data = allProducts.OrderByDescending(p => p.Name).ToList() });
            }
            else if (id == 2)
            {
                return Json(new { data = allProducts.OrderBy(p => p.Price).ToList() });
            }
            else if (id == 12)
            {
                return Json(new { data = allProducts.OrderByDescending(p => p.Price).ToList() });
            }
            else if (id == 3)
            {
                return Json(new { data = allProducts.OrderBy(p => p.Category.Name).ToList() });
            }
            else if (id == 13)
            {
                return Json(new { data = allProducts.OrderByDescending(p => p.Category.Name).ToList() });
            }
            else if (id == 4)
            {
                return Json(new { data = allProducts.OrderBy(p => p.EventType.Name).ToList() });
            }
            else if (id == 14)
            {
                return Json(new { data = allProducts.OrderByDescending(p => p.EventType.Name).ToList() });
            }
            else
            {
                return Json(new { data = allProducts });
            }

        }
        #endregion
    }
}
