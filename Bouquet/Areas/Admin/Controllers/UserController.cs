using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bouquet.DataAccess.Data;
using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

       

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ApplicationUsers.Include(u => u.Company).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach(var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if(user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }

            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var userDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(userDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking"});
            }
            if(userDb.LockoutEnd!=null && userDb.LockoutEnd > DateTime.Now)
            {
                userDb.LockoutEnd = DateTime.Now;
            }
            else 
            {
                userDb.LockoutEnd = DateTime.Now.AddYears(50);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Success to Lock/Unlock" });
        }
       
        #endregion
    }
}