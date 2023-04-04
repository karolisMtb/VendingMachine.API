using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Interfaces;
using VeendingMachine.API.DataAccess.Model;
using VendingMachine.API.BusinessLogic.Interfaces;

namespace VendingMachine.API.BusinessLogic.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IDepositStackRepository _depositStackRepository;

        public PaymentService(IPurchaseRepository purchaseRepository, IDepositStackRepository depositStackRepository)
        {
            _purchaseRepository = purchaseRepository;
            _depositStackRepository = depositStackRepository;
        }

        public async Task<Dictionary<int, int>> InitPaymentProcessAsync(Deposit deposit, Purchase lastPurchase)
        {
            await _depositStackRepository.AddDepositToDepositStackAsync(deposit);

            Dictionary<int, int> changeResult = await CalculateChangeAsync(deposit, lastPurchase.Product.Price);

            await _purchaseRepository.UpdateAsync(lastPurchase.Id);

            return changeResult;
        }

        private async Task<Dictionary<int, int>> CalculateChangeAsync(Deposit deposit, decimal productPrice)
        {
            Guid euroId = await _depositStackRepository.GetEuroIdAsync();
            Guid centId = await _depositStackRepository.GetCentIdAsync();

            int[] denominations = { 1, 2, 5, 10, 20, 50, 100, 200 };
            int[] coinAmount = { 0, 0, 0, 0, 0, 0, 0, 0 };

            decimal depositTotalValue = Utilities.ConvertDepositIntoDecimal(deposit);

            List<DepositStack> availableDepositStack = await _depositStackRepository.GetAllAsync();

            await ConvertEuroToCents(availableDepositStack, euroId);

            var sortedDepositStack = availableDepositStack.OrderBy(x => x.Denomination).ToList();

            int[] coinAmountAvailable = await GetEachCoinAmountAvailableAsync(sortedDepositStack);


            decimal changeAmountToReturn = depositTotalValue - productPrice;
            decimal changeAmountInCents = changeAmountToReturn * 100;

            for (int i = denominations.Length - 1; i >= 0; i--)
            {
                while (changeAmountInCents >= denominations[i] && coinAmountAvailable[i] != 0)
                {
                    changeAmountInCents -= denominations[i];
                    coinAmountAvailable[i] --;
                    coinAmount[i] += 1;
                }
            }

            await ConvertCentsToEuro(availableDepositStack, euroId);

            Dictionary<int, int> changeResult = new Dictionary<int, int>();

            for (int i = 0; i < coinAmount.Length; i++)
            {
                if (coinAmount[i] == 0)
                {
                    continue;
                }
                else
                {
                    if (denominations[i] == 100 || denominations[i] == 200)
                    {
                        await _depositStackRepository.UpdateDepositStackDbAsync(euroId, coinAmount[i], denominations[i] / 100);
                    }
                    else
                    {
                        await _depositStackRepository.UpdateDepositStackDbAsync(centId, coinAmount[i], denominations[i]);
                    }

                    changeResult.Add(denominations[i], coinAmount[i]);
                }
            }
            return changeResult;
        }

        private async Task ConvertEuroToCents(List<DepositStack> depositStackList, Guid euroId)
        {
            foreach (var element in depositStackList)
            {
                if (element.MoneyUnit.Id == euroId)
                {
                    element.Denomination = element.Denomination * 100;
                }
            }
        }

        private async Task ConvertCentsToEuro(List<DepositStack> depositStackList, Guid euroId)
        {
            foreach (var element in depositStackList)
            {
                if (element.MoneyUnit.Id == euroId)
                {
                    element.Denomination = element.Denomination / 100;
                }
            }
        }

        private async Task<int[]> GetEachCoinAmountAvailableAsync(List<DepositStack> sortedDepositStack)
        {
            int[] coinAmountAvailable = new int[sortedDepositStack.Count];
            for(int i = 0; i < coinAmountAvailable.Length; i++)
            {
                coinAmountAvailable[i] = sortedDepositStack[i].Amount;
            }

            return coinAmountAvailable;
        }
    }
}
