﻿using Bouquet.DataAccess.Data;
using Bouquet.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bouquet.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            EventType = new EventTypeRepository(_db);
            SPCall = new SPCall(_db);
        }

        public ICategoryRepository Category { get; private set; }
        public IEventTypeRepository EventType { get; private set; }

        public ISPCall SPCall { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
