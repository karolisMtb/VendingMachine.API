using VeendingMachine.API.DataAccess.Entities;

namespace VeendingMachine.API.DataAccess.Interfaces
{
    public interface IPurchaseRepository
    {
        Task UpdateAsync(Guid puchaseId); // updatina paid status
        void ResetPurchaseDb();
        Task AddPurchaseAsync(Purchase purchase);
        Task<bool> IsLastPurchasePaid();
         Task SaveChangesAsync();
        Task<Purchase> GetLastNotPaidPurchaseAsync();
    }
}
