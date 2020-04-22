using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using Bouquet.Models.ViewModels;
using Bouquet.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Bouquet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task< IActionResult> Index(int productPage = 1)
        {
            CategoryVM categoryVM = new CategoryVM()
            {
                Categories = await _unitOfWork.Category.GetAllAsync()
            };
            var count = categoryVM.Categories.Count();
            categoryVM.Categories = categoryVM.Categories.OrderBy(p => p.Name).Skip((productPage - 1) * 2).Take(2).ToList();
            categoryVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = 2,
                TotalItem = count,
                UrlParam= "/Admin/Category/Index?productPage=:"
            };

            return View(categoryVM);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Category category = new Category();

            if(id == null)//this is for creating
            {
                return View(category);
            }
            else //this is for editing
            {
                category = await _unitOfWork.Category.GetAsync(id.GetValueOrDefault());
                if(category == null)
                {
                    return NotFound();
                }
                return View(category);
            }           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Category category)
        { 
            if(ModelState.IsValid)
            {
                if(category.Id == 0)//POST to create new Category 
                {
                   await _unitOfWork.Category.AddAsync(category);                  
                }
                else// POST to update existed Category
                {
                    _unitOfWork.Category.Update(category);                 
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allCategories = await _unitOfWork.Category.GetAllAsync();
            return Json(new { data = allCategories });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if( id != 0)
            {
                var removeCategory = await _unitOfWork.Category.GetAsync(id);
                if(removeCategory == null)
                {
                    TempData["Error"] = "Error deleteing Category";
                    return Json(new { success = false, message ="Error while deleting" });
                }
               await _unitOfWork.Category.RemoveAsync(removeCategory);
                _unitOfWork.Save();

                TempData["Success"] = "Category successfully deleted";
                return Json(new { success = true, message = "Success deleting Category: " + removeCategory.Name });
            }
            else
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
        }
        #endregion
    }
}