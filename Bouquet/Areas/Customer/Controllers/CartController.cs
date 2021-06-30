using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using Bouquet.Models.ViewModels;
using Bouquet.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Stripe;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Bouquet.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;
        private TwilioSettings _twilioSettings { get; set; }
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }       

        public CartController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, IEmailSender emailSender, UserManager<IdentityUser> userManager, IOptions<TwilioSettings> twilioSettings)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _twilioSettings = twilioSettings.Value;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value, includeProperties: "Company");
            foreach (var list in ShoppingCartVM.ListCarts)
            {
                list.Price = list.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
                list.Product.Description = SD.ConvertToRawHtml(list.Product.Description);
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "...";
                }
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email is empty!");
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            ModelState.AddModelError(string.Empty, "Verification email sent.Please check your email.");
            return RedirectToAction("Index");
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault
                            (c => c.Id == cartId, includeProperties: "Product");
            cart.Count += 1;
            cart.Price = cart.Product.Price;
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault
                            (c => c.Id == cartId, includeProperties: "Product");
            if (cart.Count == 1)
            {
                var cnt = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, cnt - 1);
            }
            else
            {
                cart.Count -= 1;
                cart.Price = cart.Product.Price;
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault
                            (c => c.Id == cartId, includeProperties: "Product");
            var cnt = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.ssShoppingCart, cnt - 1);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new Models.OrderHeader(),
                ListCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value, includeProperties: "Company");
            foreach (var list in ShoppingCartVM.ListCarts)
            {
                list.Price = list.Product.Price;
                list.Price2 = list.Product.Price2;
                list.Price3 = list.Product.Price3;
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count + list.Price2 * list.Count2 + list.Price3 * list.Count3);
            }
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummaryPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                                                            .GetFirstOrDefault(c => c.Id == claim.Value,
                                                                    includeProperties: "Company");

            ShoppingCartVM.ListCarts = _unitOfWork.ShoppingCart
                                        .GetAll(c => c.ApplicationUserId == claim.Value,
                                        includeProperties: "Product");

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            foreach (var item in ShoppingCartVM.ListCarts)
            {
                item.Price = item.Product.Price;
                item.Price2 = item.Product.Price2;
                item.Price3 = item.Product.Price3;
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = item.Price,
                    Price2 = item.Price2,
                    Price3 = item.Price3,
                    Count = item.Count,
                    Count2 = item.Count2,
                    Count3 = item.Count3
                };
                ShoppingCartVM.OrderHeader.OrderTotal += (orderDetails.Count * orderDetails.Price + orderDetails.Count2 * orderDetails.Price2 + orderDetails.Count3 * orderDetails.Price3);
                _unitOfWork.OrderDetails.Add(orderDetails);
            }
            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCarts);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.ssShoppingCart, 0);

            if (stripeToken == null)
            {
                //order will be created for delayed payment for authroized company
                ShoppingCartVM.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            else
            {
                //process the payment
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.OrderHeader.OrderTotal * 100),
                    Currency = "cad",
                    Description = "Order ID : " + ShoppingCartVM.OrderHeader.Id,
                    Source = stripeToken
                };

                var service = new ChargeService();
                Charge charge = service.Create(options);

                if (charge.Id == null)
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
                else
                {
                    ShoppingCartVM.OrderHeader.TransactionId = charge.Id;
                }
                if (charge.Status.ToLower() == "succeeded")
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
                    ShoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
                }
            }
            _unitOfWork.Save();
     
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email is empty!");
            }

            var pathToFile = _hostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                        + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplates"
                        + Path.DirectorySeparatorChar.ToString() + "Confirmation_Order.html";
            var subject = "Your New Order";
            string HtmlBody = "";
            using (StreamReader streamReader = System.IO.File.OpenText(pathToFile))
            {
                HtmlBody = streamReader.ReadToEnd();
            }

            string message = "Your Order ID: " + ShoppingCartVM.OrderHeader.Id + "<br/>Order Total: " + ShoppingCartVM.OrderHeader.OrderTotal + " CAD$";
            string messageBody = string.Format(HtmlBody,
                     subject,
                     String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                     user.Name,
                     user.Email,
                     message                   
                     );
         
            await _emailSender.SendEmailAsync(user.Email, "Your New Order", messageBody);
            ModelState.AddModelError(string.Empty, "Verification email sent.Please check your email.");
            return RedirectToAction("OrderConfirmation", new { id = ShoppingCartVM.OrderHeader.Id });
        }


        #region API CALLS
        public IActionResult OrderConfirmation(int id)
        {  //Need to uncomment to use sms

            //OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
            //TwilioClient.Init(_twilioSettings.AccountSid, _twilioSettings.AuthToken);
            //try
            //{
            //    var message = MessageResource.Create(
            //        body: "Order Placed on Bouquet. Your Order Id: " + id,
            //        from: new Twilio.Types.PhoneNumber(_twilioSettings.PhoneNumber),
            //        to: new Twilio.Types.PhoneNumber(orderHeader.PhoneNumber)
            //        );
            //}
            //catch(Exception ex)
            //{

            //}
            return View(id);
        }

        [HttpGet]
        public IActionResult GetAll(int? id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new Models.OrderHeader(),
                ListCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value, includeProperties: "Company");
            foreach (var list in ShoppingCartVM.ListCarts)
            {
                list.Price = list.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
                list.Product.Description = SD.ConvertToRawHtml(list.Product.Description);
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "...";
                }
            }          
            return Json(new { data = ShoppingCartVM });

        }

        [HttpPost]
        public IActionResult DeliveryAddress([FromBody] string id)
        {
            return Json(id);
        }


        [HttpPut]
        public IActionResult PlusItem([FromBody] string id)
        {
            // From string value first element related bouguet price option and the second is cardId
            var priceOption = Int32.Parse(id.Substring(0, 1));
            var cartId = Int32.Parse(id.Substring(1));
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId, includeProperties: "Product");        
            int totalAmount = 0;        

            if (priceOption == 1)
            {
                cart.Count += 1;
                cart.Price = cart.Product.Price;
            }
            else if (priceOption == 2)
            {
                cart.Count2 += 1;
                cart.Price2 = cart.Product.Price2;
            }
            else if (priceOption == 3)
            {
                cart.Count3 += 1;
                cart.Price3 = cart.Product.Price3;
            }
            _unitOfWork.Save();

            var allCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList();
            foreach (var el in allCarts)
            {
                totalAmount = totalAmount + el.Count + el.Count2 + el.Count3;
            }
            HttpContext.Session.SetInt32(SD.ssShoppingCart, totalAmount);

            var data = new ReturnData(){ Success = true, Message = "Success Count Up", Count1 = cart.Count, Count2 = cart.Count2, Count3 = cart.Count3 ,Price1 = cart.Price * cart.Count, Price2 = cart.Price2 * cart.Count2, Price3 = cart.Price3 * cart.Count3, Amount = totalAmount };

            return Json(data);           
        }

        [HttpPut]
        public IActionResult MinusItem([FromBody] string id)
        {
        // From string value first element related bouguet price option and the second is cardId
            var priceOption = Int32.Parse(id.Substring(1, 1));
            var checkCart = Int32.Parse(id.Substring(0, 1));
            var cartId = Int32.Parse(id.Substring(2));           
            //Get particular cart to update to remove item or delete cart
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault
                            (c => c.Id == cartId, includeProperties: "Product");

            int totalAmount = 0;
            var data = new ReturnData();
            try
            {
                if (priceOption == 1)
                {
                    cart.Count -= 1;
                    cart.Price = cart.Product.Price;
                }
                if (priceOption == 2)
                {
                    cart.Count2 -= 1;
                    cart.Price2 = cart.Product.Price2;
                }
                if (priceOption == 3)
                {
                    cart.Count3 -= 1;
                    cart.Price3 = cart.Product.Price3;
                }
                if (checkCart == 1) // 1 means we delete this cart
                {
                    _unitOfWork.ShoppingCart.Remove(cart);
                }
                _unitOfWork.Save();
            }
            catch
            {
                data = new ReturnData()
                {
                    Success = false,
                    Message = "Error Count Down"
                };
            }
                      
            var allCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList();
          
            foreach (var el in allCarts)
            {
                totalAmount = totalAmount + el.Count + el.Count2 + el.Count3;
            }

            HttpContext.Session.SetInt32(SD.ssShoppingCart, totalAmount);


            data = new ReturnData() { Success = true, Message = "Success Count Down", Count1 = cart.Count, Count2 = cart.Count2, Count3 = cart.Count3, Price1 = cart.Price * cart.Count, Price2 = cart.Price2 * cart.Count2, Price3 = cart.Price3 * cart.Count3, Amount =totalAmount };

            return Json(data);
        }  


        #endregion
    }
}