using VeendingMachine.API.DataAccess.Model;

namespace VendingMachine.API.BusinessLogic.Interfaces
{
    public interface IPaymentService
    {
        Task InitPaymentProcessAsync(Deposit deposit);
    }
}