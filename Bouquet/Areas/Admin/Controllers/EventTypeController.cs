using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using Bouquet.Utility;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Bouquet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin)]
    public class EventTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public EventTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Upsert(int? id)
        {
            EventType eventType = new EventType();

            if (id == null)//this is for creating
            {
                return View(eventType);
            }
            else //this is for editing
            {
                eventType = await _unitOfWork.EventType.GetAsync(id.GetValueOrDefault());
                if (eventType == null)
                {
                    return NotFound();
                }
                return View(eventType);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(EventType eventType)
        {
            if (ModelState.IsValid)
            {
                if (eventType.Id == 0)//POST tto create new Category 
                {
                    await _unitOfWork.EventType.AddAsync(eventType);
                }
                else// POST to update existed Category
                {
                    _unitOfWork.EventType.Update(eventType);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(eventType);
        }      

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
             var allEventTypes = await _unitOfWork.EventType.GetAllAsync();           
            return Json(new { data = allEventTypes });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                var removeEventType = await _unitOfWork.EventType.GetAsync(id);
                if (removeEventType == null)
                {
                    TempData["Error"] = "Error deleteing Event type";
                    return Json(new { success = false, message = "Error while deleting" });
                }
                await _unitOfWork.EventType.RemoveAsync(removeEventType);
                _unitOfWork.Save();

                TempData["Success"] = "Event type successfully deleted";
                return Json(new { success = true, message = "Success deleting Event type: " + removeEventType.Name });
            }
            else
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
        }
       
        #endregion
    }
}