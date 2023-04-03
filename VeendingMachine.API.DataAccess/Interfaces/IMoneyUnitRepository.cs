using VeendingMachine.API.DataAccess.Entities;

namespace VeendingMachine.API.DataAccess.Interfaces
{
    public interface IMoneyUnitRepository
    {
        Task AddMoneyUnitsAsync(List<MoneyUnit> moneyUnits);
        Task SaveChangesAsync();
        Task<List<MoneyUnit>> GetAll();
    }
}
