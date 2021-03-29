using Bouquet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bouquet.DataAccess.Repository.IRepository
{
   public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}
