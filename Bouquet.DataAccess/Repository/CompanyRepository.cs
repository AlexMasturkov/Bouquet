using Bouquet.DataAccess.Data;
using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bouquet.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            var companyDb = _db.Companies.FirstOrDefault(c => c.Id == company.Id);
            if (companyDb != null)
            {
                companyDb.Name = company.Name;
                companyDb.StreetAddress = company.StreetAddress;
                companyDb.City = company.City;
                companyDb.State = company.State;
                companyDb.PostalCode = company.PostalCode;
                companyDb.Phone = company.Phone;
                companyDb.IsAuthorizedCompany = company.IsAuthorizedCompany;
            }
        }
    }
}
