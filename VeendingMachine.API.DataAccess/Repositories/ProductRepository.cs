using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeendingMachine.API.DataAccess.DatabaseContext;
using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Interfaces;

namespace VeendingMachine.API.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly VendorContext _vendorContext;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(VendorContext vendorContext, ILogger<ProductRepository> logger)
        {
            _vendorContext = vendorContext;
            _logger = logger;
        }

        public async Task<Product> GetProductAsync(int vendingNumber)
        {
            if (_vendorContext.Products != null)
            {
                var product = await _vendorContext.Products.FirstOrDefaultAsync(p => p.ProductNumber == vendingNumber);

                return product;
            }
            else
            {
                _logger.LogError("Product with a given id does not exist");
                throw new NullReferenceException("Product with a given id does not exist");
            }
        }

        public int GetProductsCountFromDB()
        {
            return _vendorContext.Products.Count();
        }

        public void AddProductRange(List<Product> products)
        {
            _vendorContext.Products.RemoveRange(_vendorContext.Products);
            _vendorContext.Products.AddRange(products);
        }

        public async Task SaveChangesAsync()
        {
            await _vendorContext.SaveChangesAsync();
        }
    }
}

// i Endpoint ivede int produkto id
// iskvieciamas servisas, kuris per repo gauna produkta is db
// laukiamas mokejimas
// kai sumokama iskviecimas servisas apskaiciuoti pinigus ir grazinti graza
// pridedamas produktas i db prie purchases
// updatinama depositstack db
