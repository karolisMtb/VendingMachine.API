using VeendingMachine.API.DataAccess.Entities;

namespace VeendingMachine.API.DataAccess.Interfaces
{
    public interface IVendingMachineService
    {
        Task InitProductPurchaseAsync(int vendingNumber);
        Task<bool> CheckIfLastPurchasePaidAsync();
        Task<int> GetTotalProductCountAsync();
        Task<Product> GetProductAsync(int vendingNumber);
    }
}
