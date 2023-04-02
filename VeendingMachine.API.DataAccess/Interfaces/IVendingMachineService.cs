using VeendingMachine.API.DataAccess.Entities;

namespace VeendingMachine.API.DataAccess.Interfaces
{
    public interface IVendingMachineService
    {
        Task InitProductPurchaseAsync(int vendingNumber);
        //Task InitPaymentProcessAsync(Deposit deposit);
        Task<bool> CheckIfLastPurchasePaidAsync();
        int GetTotalProductCount();
        Task<Product> GetProductAsync(int vendingNumber); // istraukia produkta pagal id
        //Task AddToPurchasesDbAsync(Purchase purchase); // ideda ji i db, setina paid = false;
        //Task UpdatePurchaseStatusAsync(Guid purchaseId);
        //Task CalculateChangeAsync();
        //Task UpdateDepositStackDbAsync();
        //Purchase GetLastPurchased();
    }
}
