﻿using Bouquet.DataAccess.Data;
using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bouquet.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            var categoryDb = _db.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (categoryDb != null)
            {
                categoryDb.Name = category.Name;               
            }
        }
    }
}
