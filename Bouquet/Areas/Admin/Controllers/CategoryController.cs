using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();

            if(id == null)//this is for creating
            {
                return View(category);
            }
            else //this is for editing
            {
                category = _unitOfWork.Category.Get(id.GetValueOrDefault());
                if(category == null)
                {
                    return NotFound();
                }
                return View(category);
            }           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        { 
            if(ModelState.IsValid)
            {
                if(category.Id == 0)//POST to create new Category 
                {
                    _unitOfWork.Category.Add(category);                  
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
        public IActionResult GetAll()
        {
            var allCategories = _unitOfWork.Category.GetAll();
            return Json(new { data = allCategories });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if( id != 0)
            {
                var removeCategory = _unitOfWork.Category.Get(id);
                if(removeCategory == null)
                {
                    return Json(new { success = false, message ="Error while deleting" });
                }
                _unitOfWork.Category.Remove(removeCategory);
                _unitOfWork.Save();
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