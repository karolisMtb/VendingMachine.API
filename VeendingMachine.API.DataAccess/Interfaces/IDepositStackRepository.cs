using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Model;

namespace VeendingMachine.API.DataAccess.Interfaces
{
    public interface IDepositStackRepository
    {
        Task AddDepositStackRangeAsync(List<DepositStack> depositStack);
        Task AddDepositToDepositStack(Deposit deposit);
        Task SaveChangesAsync();
        //Task CalculateChangeAsync(Deposit deposit, decimal amountToPay);
    }
}
