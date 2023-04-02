using VeendingMachine.API.DataAccess.DatabaseContext;
using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Interfaces;

namespace VeendingMachine.API.DataAccess.Repositories
{
    public class MoneyUnitRepository : IMoneyUnitRepository
    {
        private readonly VendorContext _vendorContext;
        public MoneyUnitRepository(VendorContext vendorContext)
        {
            _vendorContext = vendorContext;
        }

        public async Task AddMoneyUnitsAsync(List<MoneyUnit> moneyUnits)
        {
            _vendorContext.MoneyUnits.RemoveRange(_vendorContext.MoneyUnits);
            await _vendorContext.MoneyUnits.AddRangeAsync(moneyUnits);
        }

        public async Task SaveChangesAsync()
        {
            await _vendorContext.SaveChangesAsync();
        }
    }
}
