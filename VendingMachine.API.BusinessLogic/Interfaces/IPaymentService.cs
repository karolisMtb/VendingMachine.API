using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Model;

namespace VendingMachine.API.BusinessLogic.Interfaces
{
    public interface IPaymentService
    {
        Task<Dictionary<int, int>> InitPaymentProcessAsync(Deposit deposit, Purchase lastPurchase);
    }
}