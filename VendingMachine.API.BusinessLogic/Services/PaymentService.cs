using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Interfaces;
using VeendingMachine.API.DataAccess.Model;
using VendingMachine.API.BusinessLogic.Interfaces;

namespace VendingMachine.API.BusinessLogic.Services
{
    public class PaymentService: IPaymentService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IDepositStackRepository _depositStackRepository;

        public PaymentService(IPurchaseRepository purchaseRepository, IDepositStackRepository depositStackRepository)
        {
            _purchaseRepository = purchaseRepository;
            _depositStackRepository = depositStackRepository;
        }

        public async Task InitPaymentProcessAsync(Deposit deposit)
        {
            Purchase lastPurchase = _purchaseRepository.GetLastNotPaidPurchase();

            if (!InsertedEnoughChange(deposit, lastPurchase.Product.Price))
                throw new Exception("Not enough change has been inserted. We're giving your change back and try again please.");

            await _depositStackRepository.AddDepositToDepositStack(deposit);

            Deposit change = CalculateChange();

            //calculate change and save it in a variable
            //update DepositStack
            //update product count
            //calculate change
            //await UpdateDepositStackDbAsync(deposit, priceToPay);
        }

        private bool InsertedEnoughChange(Deposit deposit, decimal productPrice)
        {
            decimal totalDeposit = Utilities.ConvertDepositIntoDecimal(deposit);

            if (totalDeposit >= productPrice)
            {
                return true;
            }

            return false;
        }

        private Deposit CalculateChange()
        {
            return null;
        }






    }
}
