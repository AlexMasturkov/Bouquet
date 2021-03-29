using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using Bouquet.Utility;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Bouquet.Areas.Admin.Controllers
{
    // we use stored procedures to call each action 
    // we can uncomment commented parts to use without stored procedure methods

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

        /*
        public IActionResult Upsert(int? id)
        {
           EventType eventType = new EventType();

            if (id == null)//this is for creating
            {
                return View(eventType);
            }
            else //this is for editing
            {
                //block to use procedure
                var parameter = new DynamicParameters();
                parameter.Add("@Id", id);
                eventType = _unitOfWork.SPCall.OneRecord<EventType>(SD.ProcedureEventTypeGet, parameter);
                //end block

                //eventType = _unitOfWork.EventType.Get(id.GetValueOrDefault());
                if (eventType == null)
                {
                    return NotFound();
                }
                return View(eventType);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(EventType eventType)
        {
            if (ModelState.IsValid)
            {
                //use procedure
                var parameter = new DynamicParameters();
                parameter.Add("@Name", eventType.Name);
                //end

                if (eventType.Id == 0)//POST to create newEventType 
                {
                    // _unitOfWork.EventType.Add(eventType);
                    _unitOfWork.SPCall.Execute(SD.ProcedureEventTypeCreate, parameter);
                }
                else// POST to update existedEventType
                {
                    parameter.Add("@Id", eventType.Id);
                    _unitOfWork.SPCall.Execute(SD.ProcedureEventTypeUpdate, parameter);
                    //_unitOfWork.EventType.Update(eventType);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(eventType);
        }
        */

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
             var allEventTypes = await _unitOfWork.EventType.GetAllAsync();
            //var allEventTypes = _unitOfWork.SPCall.List<EventType>(SD.ProcedureEventTypeGetAll,null);//using procedures
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

        /*


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                //block to use procedure
                var parameter = new DynamicParameters();
                parameter.Add("@Id", id);
                var removeEventType = _unitOfWork.SPCall.OneRecord<EventType>(SD.ProcedureEventTypeGet, parameter);
                //end block

                //var removeEventType = _unitOfWork.EventType.Get(id);
                if (removeEventType == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }
                _unitOfWork.SPCall.Execute(SD.ProcedureEventTypeDelete, parameter);
                //_unitOfWork.EventType.Remove(removeEventType);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Success deletingEventType: " + removeEventType.Name });
            }
            else
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
        }
        */
        #endregion
    }
}