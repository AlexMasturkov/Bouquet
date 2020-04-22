using Bouquet.DataAccess.Data;
using Bouquet.Models;
using Bouquet.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Bouquet.DataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public void Inizialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
                if (_db.Roles.Any(r => r.Name == SD.RoleAdmin)) return;
                _roleManager.CreateAsync(new IdentityRole(SD.RoleAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RoleCompanyUser)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RoleEmployee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.RoleIndividual)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "mastruk@list.ru",
                    Email = "mastruk@list.ru",
                    EmailConfirmed = true,
                    Name = "Alex Master"
                }, "Admin123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == "mastruk@list.ru").FirstOrDefault();
                _userManager.AddToRoleAsync(user, SD.RoleAdmin).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

            }
         
        }
    }
}

