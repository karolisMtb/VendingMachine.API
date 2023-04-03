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


        //
        public async Task InitPaymentProcessAsync(Deposit deposit)
        {
            Purchase lastPurchase = _purchaseRepository.GetLastNotPaidPurchaseAsync().Result;

            if (!InsertedEnoughChange(deposit, lastPurchase.Product.Price))
            {
                throw new Exception("Not enough change has been inserted. We're giving your change back and try again please.");
            }

            await _depositStackRepository.AddDepositToDepositStackAsync(deposit);

            Dictionary<int, int> changeResult = await CalculateChangeAsync(deposit, lastPurchase.Product.Price);

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

        private async Task<Dictionary<int, int>> CalculateChangeAsync(Deposit deposit, decimal productPrice)
        {
            Guid euroId = await _depositStackRepository.GetEuroIdAsync();
            Guid centId = await _depositStackRepository.GetCentIdAsync();

            int[] denominations = { 1, 2, 5, 10, 20, 50, 100, 200 };
            int[] coinAmount = { 0, 0, 0, 0, 0, 0, 0, 0 };

            decimal depositTotalValue = Utilities.ConvertDepositIntoDecimal(deposit);

            List<DepositStack> availableDepositStack = await _depositStackRepository.GetAllAsync(); // visi pinigai            

            foreach (var element in availableDepositStack)
            {
                if(element.MoneyUnit.Id == euroId)
                {
                    element.Denomination *= 100;
                }
            }

            var sortedDepositStack = availableDepositStack.OrderByDescending(x => x.MoneyUnit.Name).ThenBy(x => x.Denomination).ToList();

            decimal changeAmountToReturn = depositTotalValue - productPrice;
            decimal changeAmountInCents = changeAmountToReturn * 100;

            for (int i = denominations.Length - 1; i >= 0; i--)
            {
                while (changeAmountInCents >= denominations[i])
                {
                    changeAmountInCents -= denominations[i];
                    coinAmount[i] += 1;
                }
            }

            Dictionary<int, int> changeResult = new Dictionary<int, int>();

            for(int i = 0; i < coinAmount.Length; i++)
            {
                if (coinAmount[i] == 0)
                {
                    continue;
                }
                else
                {
                    if (denominations[i] == 100 || denominations[i] == 200)
                    {
                        _depositStackRepository.UpdateDepositStackDbAsync(euroId, coinAmount[i], denominations[i] / 100);
                    }
                    else
                    {
                        _depositStackRepository.UpdateDepositStackDbAsync(centId, coinAmount[i], denominations[i]);
                    }

                    changeResult.Add(denominations[i], coinAmount[i]);
                }
            }
            return changeResult;
        }
    }
}
