using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using Bouquet.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bouquet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin +","+ SD.RoleEmployee)]
    public class CompanyController : Controller
    { private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();

            if(id == null)//this is for creating
            {
                return View(company);
            }
            else //this is for editing
            {
                company = _unitOfWork.Company.Get(id.GetValueOrDefault());
                if(company == null)
                {
                    return NotFound();
                }
                return View(company);
            }           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        { 
            if(ModelState.IsValid)
            {
                if(company.Id == 0)//POST to create new Category 
                {
                    _unitOfWork.Company.Add(company);                  
                }
                else// POST to update existed Category
                {
                    _unitOfWork.Company.Update(company);                 
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allCompanies = _unitOfWork.Company.GetAll();
            return Json(new { data = allCompanies });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if( id != 0)
            {
                var removeCompany = _unitOfWork.Company.Get(id);
                if(removeCompany == null)
                {
                    return Json(new { success = false, message ="Error while deleting" });
                }
                _unitOfWork.Company.Remove(removeCompany);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Success deleting Category: " + removeCompany.Name });
            }
            else
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
        }
        #endregion
    }
}
