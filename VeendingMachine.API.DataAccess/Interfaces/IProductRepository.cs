using VeendingMachine.API.DataAccess.Entities;

namespace VeendingMachine.API.DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(int vendingNumber);
        void AddProductRange(List<Product> products);
        int GetProductsCountFromDB();
        Task SaveChangesAsync();
    }
}
