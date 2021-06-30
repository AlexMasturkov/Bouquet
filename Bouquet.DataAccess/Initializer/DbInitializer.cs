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

        string[] newCategories = { "Bouquets", "Plants", "Decoration" };
        string[] newEvents = { "Birthday", "Romance", "New Year", "Meeting" };       
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
                    StreetAddress ="25 Chubarova street 14",
                    City ="Zmerinca",
                    PostalCode="M3B7G8",
                    PhoneNumber="4167894455",
                    State="ON",
                    Name = "Ustas Master"
                }, "Admin123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == "mastruk@list.ru").FirstOrDefault();
                _userManager.AddToRoleAsync(user, SD.RoleAdmin).GetAwaiter().GetResult();

                if (_db.Categories.Any(c => c.Id >= 1))
                {
                    return;
                }
                else
                {
                    for (int i = 0; i < newCategories.Length; i++)
                    {
                        Category category = new Category();
                        category.Name = newCategories[i];
                        _db.Categories.Add(category);
                        _db.SaveChanges();
                    }
                }

                if (_db.EventTypes.Any(e => e.Id >= 1))
                {
                    return;
                }
                else
                {

                    for (int i = 0; i < newEvents.Length; i++)
                    {
                       EventType eventType = new EventType();
                        eventType.Name = newEvents[i];
                        _db.EventTypes.Add(eventType);
                        _db.SaveChanges();
                    }

                }

                if (_db.Products.Any(e => e.Id >= 1))
                {
                    return;
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Product product = new Product();
                        product.Name = "Product 1" + i;
                        product.Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit." +
                            " Minima orem ipsum dolor sit amet consecteturquisquam adipisci, inventore quasi, " +
                            "laudantium totam, vero harum delectus recusandae eum id suscipit.";
                        product.ImageUrl = @"\" + SD.ImageFolder + @"\" + "defaultImage" + ".jpeg";
                        product.Price = 25.98m;
                        product.Price2 = 35.98m;
                        product.Price3 = 45.98m;
                        product.RegularOption = "5 items plus decoration";
                        product.PremiumOption = "7 items plus decoration";
                        product.LuxuryOption = "9 items plus decoration";
                        product.CategoryId = 1;
                        product.EventTypeId = 3;
                        _db.Products.Add(product);
                        _db.SaveChanges();
                    }
                }

                }
            catch (Exception ex)
            {

            }
         
        }
    }
}

