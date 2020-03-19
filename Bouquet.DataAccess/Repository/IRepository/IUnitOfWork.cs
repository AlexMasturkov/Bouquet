using System;
using System.Collections.Generic;
using System.Text;

namespace Bouquet.DataAccess.Repository.IRepository
{
   public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Category { get; }
        ISPCall SPCall { get; }

    }
}
