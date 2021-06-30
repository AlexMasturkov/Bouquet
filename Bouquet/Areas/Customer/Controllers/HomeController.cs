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
                var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList().Count();
                var count1 = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).Select(c=>c.Count);
                var cartItems = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList();

                var elements = cartItems;
                int totalAmount = 0;
                foreach(var el in cartItems)
                {
                    totalAmount = totalAmount + el.Count + el.Count2 + el.Count3;
                }

                HttpContext.Session.SetInt32(SD.ssShoppingCart, totalAmount);
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
        public IActionResult Details(ShoppingCart cart , int id)
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
                    if (id == 1)
                    {
                        cart.Count = 1;
                    }
                    else if (id == 2)
                    {
                        cart.Count2 =  1;
                    }
                    else if (id == 3)
                    {
                        cart.Count3 =  1;
                    }
                    _unitOfWork.ShoppingCart.Add(cart);
                }
                else
                {
                    int count1 = shoppingCartDb.Count;
                    int count2 = shoppingCartDb.Count2;
                    int count3 = shoppingCartDb.Count3;

                    if (id == 1)
                    {
                        shoppingCartDb.Count = count1 + 1;
                    }
                    else if (id == 2)
                    {
                        shoppingCartDb.Count2 = count2 + 1;
                    }
                    else if (id == 3)
                    {
                        shoppingCartDb.Count3 = count3 + 1;
                    }
                }
                _unitOfWork.Save();

                var count = _unitOfWork.ShoppingCart
                   .GetAll(c => c.ApplicationUserId == cart.ApplicationUserId)
                   .ToList().Count();

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
        public IActionResult Payment()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Shipping()
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
        public IActionResult GetAllIndex(string id)
        {
            var allProducts = _unitOfWork.Product.GetAll(includeProperties: "Category,EventType");   
            if (id.Length >= 3 && id !="")
            {               
                id = id.First().ToString().ToUpper() + id.Substring(1,2);
                var productList = allProducts.Where(p => p.Category.Name.Contains(id) || p.EventType.Name.Contains(id) || p.Name.Contains(id)); 
                return Json(new { data = productList.ToList() });              
            }        
            else
            {
                return Json(new { data = allProducts });
            }
        }

        [HttpGet]
        public IActionResult GetDetails(int id)
        {
            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,EventType");
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = productFromDb,
                ProductId = productFromDb.Id
            };

            return Json(new { data = shoppingCart });           
        }        

        #endregion
    }
}
