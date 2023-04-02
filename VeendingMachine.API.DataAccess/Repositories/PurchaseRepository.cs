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
            // setina purchase paid status i true
        }

        public async Task SaveChangesAsync()
        {
            await _vendorContext.SaveChangesAsync();
        }

        public async Task AddPurchaseAsync(Purchase purchase)
        {
            await _vendorContext.Purchases.AddAsync(purchase);
            await SaveChangesAsync();
        }

        public async Task<bool> IsLastPurchasePaid()
        {
            if(_vendorContext.Purchases.Count() != 0)
            {
                //error handling
                Purchase lastPurchase = await _vendorContext.Purchases.OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
                return lastPurchase.Paid;
            }
            return true;
        }

        public Purchase GetLastNotPaidPurchase()
        {
            var lastNotPaidPurchase = _vendorContext.Purchases.OrderByDescending(x => x.Timestamp).Include(x => x.Product).FirstOrDefault(x => x.Paid == false);

            if (lastNotPaidPurchase == null)
            {
                throw new FileNotFoundException("All items have been paid. Choose a new item.");
            }

            return lastNotPaidPurchase;
        }

        
    }
}
