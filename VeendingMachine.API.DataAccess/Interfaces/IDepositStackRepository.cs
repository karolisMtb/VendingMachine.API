using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Model;

namespace VeendingMachine.API.DataAccess.Interfaces
{
    public interface IDepositStackRepository
    {
        Task AddDepositStackRangeAsync(List<DepositStack> depositStack);
        Task AddDepositToDepositStackAsync(Deposit deposit);
        Task SaveChangesAsync();
        Task<List<DepositStack>> GetAllAsync();
        Task UpdateDepositStackDbAsync(Guid moneyUnitId, int coinAmount, int denomination);
        Task<Guid> GetCentIdAsync();
        Task<Guid> GetEuroIdAsync();
    }
}
