using Bouquet.DataAccess.Data;
using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bouquet.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails orderDetails)
        {
            _db.Update(orderDetails);
        }
    }
}
