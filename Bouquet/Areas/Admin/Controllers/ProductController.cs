using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using Bouquet.Models.ViewModels;
using Bouquet.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bouquet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHost;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<Category> CategoryFirstList = await _unitOfWork.Category.GetAllAsync();

           ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = CategoryFirstList.Select(i=> new SelectListItem { 
                    Text = i.Name,
                    Value = i.Id.ToString()                
                }),
                EventTypeList = _unitOfWork.EventType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if(id == null)//this is for creating
            {
                return View(productVM);
            }
            else //this is for editing
            {
                productVM.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());

                if(productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _webHost.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products\");
                    var extension = Path.GetExtension(files[0].FileName);
                    if(productVM.Product.ImageUrl != null)
                    {
                        //this is an edit and we need to remove old image
                      
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads,fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                else
                {
                    //update when not change image
                    if(productVM.Product.Id!=0)
                    {
                        var productFromDb = _unitOfWork.Product.Get(productVM.Product.Id);
                        productVM.Product.ImageUrl = productFromDb.ImageUrl;
                    }
                }


                if (productVM.Product.Id == 0)//POST to create new Category 
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else// POST to update existed Category
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<Category> CategoryFirstList = await _unitOfWork.Category.GetAllAsync();
                productVM.CategoryList = CategoryFirstList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                productVM.EventTypeList = _unitOfWork.EventType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if(productVM.Product.Id !=0)
                {
                    productVM.Product = _unitOfWork.Product.Get(productVM.Product.Id);
                }
            }
            return View(productVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allProducts = _unitOfWork.Product.GetAll(includeProperties:"Category,EventType");
            return Json(new { data = allProducts });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if( id != 0)
            {
                var removeProduct = _unitOfWork.Product.Get(id);
                if(removeProduct == null)
                {
                    return Json(new { success = false, message ="Error while deleting" });
                }

                string webRootPath = _webHost.WebRootPath;
                var imagePath = Path.Combine(webRootPath, removeProduct.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _unitOfWork.Product.Remove(removeProduct);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Success deleting Category: " + removeProduct.Name });
            }
            else
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
        }
        #endregion
    }
}