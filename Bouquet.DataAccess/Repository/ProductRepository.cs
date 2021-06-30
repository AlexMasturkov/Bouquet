using Bouquet.DataAccess.Data;
using Bouquet.DataAccess.Repository.IRepository;
using Bouquet.Models;
using System.Linq;

namespace Bouquet.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var productDb = _db.Products.FirstOrDefault(c => c.Id == product.Id);
            if (productDb != null)
            {
                if(product.ImageUrl != null)
                {
                    productDb.ImageUrl = product.ImageUrl;
                }
                productDb.Name = product.Name;
                productDb.Description = product.Description;
                productDb.RegularOption = product.RegularOption;
                productDb.PremiumOption = product.PremiumOption;
                productDb.LuxuryOption = product.LuxuryOption;
                productDb.Price = product.Price;
                productDb.Price2 = product.Price2;
                productDb.Price3 = product.Price3;
                productDb.CategoryId = product.CategoryId;
                productDb.EventTypeId = product.EventTypeId;              
            }
        }
    }
}
