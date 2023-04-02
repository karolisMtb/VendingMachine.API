using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeendingMachine.API.DataAccess.DatabaseContext;
using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Interfaces;
using VeendingMachine.API.DataAccess.Model;

namespace VeendingMachine.API.DataAccess.Repositories
{
    public class DepositStackRepository : IDepositStackRepository
    {
        private readonly VendorContext _vendorContext;
        private readonly ILogger<DepositStackRepository> _logger;
        public DepositStackRepository(VendorContext vendorContext, ILogger<DepositStackRepository> logger)
        {
            _vendorContext = vendorContext;
            _logger = logger;
        }

        public async Task AddDepositToDepositStack(Deposit deposit) 
        {
            Guid euroId = _vendorContext.MoneyUnits.Where(x => x.Name == "Euro").First().Id;
            Guid centId = _vendorContext.MoneyUnits.Where(x => x.Name == "Ct").First().Id;

            var oneCentDepositStack = _vendorContext.DepositStacks.Where(x => x.Denomination == 1 && x.MoneyUnit.Id == centId).First();
            var twoCentDepositStack = _vendorContext.DepositStacks.Where(x => x.Denomination == 2 && x.MoneyUnit.Id == centId).First();
            var fiveCentDepositStack = _vendorContext.DepositStacks.Where(x => x.Denomination == 5 && x.MoneyUnit.Id == centId).First();
            var tenCentDepositStack = _vendorContext.DepositStacks.Where(x => x.Denomination == 10 && x.MoneyUnit.Id == centId).First();
            var twentyCentDepositStack = _vendorContext.DepositStacks.Where(x => x.Denomination == 20 && x.MoneyUnit.Id == centId).First();
            var fiftyCentDepositStack = _vendorContext.DepositStacks.Where(x => x.Denomination == 50 && x.MoneyUnit.Id == centId).First();

            var oneEuroDepositStack = _vendorContext.DepositStacks.Where(x => x.Denomination == 1 && x.MoneyUnit.Id == euroId).First();
            var twoEuroDepositStack = _vendorContext.DepositStacks.Where(x => x.Denomination == 2 && x.MoneyUnit.Id == euroId).First();

            oneCentDepositStack.Amount += deposit.CentDeposit.oneCentCoinAmount;

            twoCentDepositStack.Amount += deposit.CentDeposit.twoCentCoinAmount;


            //_vendorContext.DepositStacks.Where(x => x.Id == oneCentDenominationId).First().Amount += deposit.CentDeposit.oneCentCoinAmount;
            //_vendorContext.DepositStacks.Where(x => x.Id == twoCentDenominationId).First().Amount += deposit.CentDeposit.twoCentCoinAmount;
            //_vendorContext.DepositStacks.Where(x => x.Id == fiveCentDenominationId).First().Amount += deposit.CentDeposit.fiveCentCoinAmount;
            //_vendorContext.DepositStacks.Where(x => x.Id == tenCentDenominationId).First().Amount += deposit.CentDeposit.tenCentCoinAmount;
            //_vendorContext.DepositStacks.Where(x => x.Id == twentyCentDenominationId).First().Amount += deposit.CentDeposit.twentyCentCoinAmount;
            //_vendorContext.DepositStacks.Where(x => x.Id == fiftyCentDenominationId).First().Amount += deposit.CentDeposit.fiftyCentCoinAmount;

            //_vendorContext.DepositStacks.Where(x => x.Id == oneEuroDenomination).First().Amount += deposit.OneEuroCoinAmount;
            //_vendorContext.DepositStacks.Where(x => x.Id == twoEuroDenomination).First().Amount += deposit.TwoEuroCoinAmount;
            
            _vendorContext.SaveChanges();

            //await SaveChangesAsync();
        }

        public async Task AddDepositStackRangeAsync(List<DepositStack> depositStack)
        {
            _vendorContext.DepositStacks.RemoveRange(_vendorContext.DepositStacks);
            await _vendorContext.DepositStacks.AddRangeAsync(depositStack);
            _logger.LogInformation("Table row count is {0}", _vendorContext.DepositStacks.Count().ToString());
        }

        public async Task SaveChangesAsync()
        {
            await _vendorContext.SaveChangesAsync();
        }

        //public Task CalculateChangeAsync(Deposit deposit, decimal amountToPay)
        //{

        //    string[] denom = { "1", "2", "5", "10", "20", "50", "100", "200" };
        //    int[] amount = { 0, 0, 0, 0, 0, 0, 0, 0 };

        //    if(amount.Sum == )

        //        // dirbu cia

        //    throw new NotImplementedException();

        //    return null;
        //}
    }
}
