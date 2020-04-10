using System;
using System.Collections.Generic;
using System.Text;

namespace Bouquet.DataAccess.Repository.IRepository
{
   public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Category { get; }
        IEventTypeRepository EventType { get; }
        IProductRepository Product{ get; }
        ICompanyRepository Company { get; }
        IApplicationUserRepository ApplicationUser { get; }
        ISPCall SPCall { get; }
        void Save();

    }
}
