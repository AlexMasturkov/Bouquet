using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bouquet.DataAccess.Data;
using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using Bouquet.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin + "," + SD.RoleEmployee)]
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

        private class UserCurrent
        {
            public string Name { get; set; }
            public string Role { get; set; }
            public string Id { get; set; }

            public DateTimeOffset? LockoutEnd { get; set; }

        };

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ApplicationUsers.Include(u => u.Company).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            var userApplication = new UserCurrent();
            var userApplicationList = new List<UserCurrent>();
            foreach (var user in userList)
            {              
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                userApplication.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;             
                userApplication.Name = user.Name;
                userApplication.Id = user.Id;
                userApplication.LockoutEnd = user.LockoutEnd;
                userApplicationList.Add(new UserCurrent{ Name = user.Name, Id = user.Id, Role= userApplication.Role, LockoutEnd = userApplication.LockoutEnd});
            }           

            return Json(new { data = userApplicationList });
        }

        [HttpGet]
        public IActionResult GetDetails(string id)
        {
            var userDb = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == id);
            if (userDb == null)
            {
                return Json(new { success = false, message = "Error get Details" });
            }
            else
            {
                var userRole = _db.UserRoles.ToList();
                var roles = _db.Roles.ToList();
                var roleId = userRole.FirstOrDefault(u => u.UserId == id).RoleId;              
                userDb.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if (userDb.Company == null)
                {
                    userDb.Company = new Company()
                    {
                        Name = "",
                        Phone =""
                    };
                }

                return Json(new { data = new { userDb.Name, userDb.City, userDb.StreetAddress, userDb.PostalCode, userDb.PhoneNumber, userDb.Email, userDb.State, userDb.Role, userDb.Company } });
            }
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
                userDb.LockoutEnd = DateTime.Now.AddDays(90);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Success to Lock/Unlock" });
        }       
        #endregion
    }
}