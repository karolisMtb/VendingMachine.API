using Microsoft.EntityFrameworkCore;
using VeendingMachine.API.DataAccess.DatabaseContext;
using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Interfaces;

namespace VeendingMachine.API.DataAccess.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly VendorContext _vendorContext;
        public PurchaseRepository(VendorContext vendorContext)
        {
            _vendorContext = vendorContext;
        }

        public void ResetPurchaseDb()
        {
            _vendorContext.Purchases.RemoveRange(_vendorContext.Purchases);
        }

        public async Task UpdateAsync(Guid purchaseId)
        {
            Purchase purchaseToUpdate = await _vendorContext.Purchases.FirstOrDefaultAsync(x => x.Id == purchaseId);
            purchaseToUpdate.Paid = true;
            _vendorContext.Purchases.Update(purchaseToUpdate);

            await SaveChangesAsync();
        }


        public async Task AddPurchaseAsync(Purchase purchase)
        {
            await _vendorContext.Purchases.AddAsync(purchase);
            await SaveChangesAsync();
        }

        public async Task<bool> IsLastPurchasePaidAsync()
        {
            if(await _vendorContext.Purchases.AnyAsync())
            {
                Purchase lastPurchase = await _vendorContext.Purchases.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
                return lastPurchase.Paid;
            }
            return true;
        }

        public async Task<Purchase> GetLastNotPaidPurchaseAsync()
        {
            var lastNotPaidPurchase = await _vendorContext.Purchases.OrderByDescending(x => x.Timestamp).Include(x => x.Product).FirstOrDefaultAsync(x => x.Paid == false);

            if (lastNotPaidPurchase == null)
            {
                throw new FileNotFoundException("All items have been paid. Choose a new item.");
            }

            return lastNotPaidPurchase;
        }
        public async Task SaveChangesAsync()
        {
            await _vendorContext.SaveChangesAsync();
        }        
    }
}
