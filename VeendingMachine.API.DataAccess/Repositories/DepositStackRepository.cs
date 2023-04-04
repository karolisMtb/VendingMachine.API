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

        public async Task AddDepositToDepositStackAsync(Deposit deposit) 
        {
            GetOneCentDepositStackAsync().Result.Amount += deposit.CentDeposit.oneCentCoinAmount;
            GetTwoCentDepositStackAsync().Result.Amount += deposit.CentDeposit.twoCentCoinAmount;
            GetFiveCentDepositStackAsync().Result.Amount += deposit.CentDeposit.fiveCentCoinAmount;
            GetTenCentDepositStackAsync().Result.Amount += deposit.CentDeposit.tenCentCoinAmount;
            GetTwentyCentDepositStackAsync().Result.Amount += deposit.CentDeposit.twentyCentCoinAmount;
            GetFiftyCentDepositStackAsync().Result.Amount += deposit.CentDeposit.fiftyCentCoinAmount;

            GetOneEuroDepositStackAsync().Result.Amount += deposit.OneEuroCoinAmount;
            GetTwoEuroDepositStackAsync().Result.Amount += deposit.TwoEuroCoinAmount;            

            await SaveChangesAsync();
        }

        public async Task<List<DepositStack>> GetAllAsync()
        {
            return await _vendorContext.Set<DepositStack>().ToListAsync();
        }

        public async Task<Guid> GetEuroIdAsync()
        {
            MoneyUnit euroMoneyUnit = await _vendorContext.MoneyUnits.Where(x => x.Name == "Euro").FirstAsync();
            return euroMoneyUnit.Id;
        }

        public async Task<Guid> GetCentIdAsync()
        {
            MoneyUnit centMoneyUnit = await _vendorContext.MoneyUnits.Where(x => x.Name == "Ct").FirstAsync();
            return centMoneyUnit.Id;
        }

        private async Task<DepositStack> GetOneCentDepositStackAsync()
        {
            DepositStack oneCentDepositStack = await _vendorContext.DepositStacks.Where(x => x.Denomination == 1 && x.MoneyUnit.Id == GetCentIdAsync().Result).FirstAsync();
            return oneCentDepositStack;
        }

        private async Task<DepositStack> GetTwoCentDepositStackAsync()
        {
            DepositStack twoCentDepositStack = await _vendorContext.DepositStacks.Where(x => x.Denomination == 2 && x.MoneyUnit.Id == GetCentIdAsync().Result).FirstAsync();
            return twoCentDepositStack;
        }

        private async Task<DepositStack> GetFiveCentDepositStackAsync()
        {
            DepositStack fiveCentDepositStack = await _vendorContext.DepositStacks.Where(x => x.Denomination == 5 && x.MoneyUnit.Id == GetCentIdAsync().Result).FirstAsync();
            return fiveCentDepositStack;
        }

        private async Task<DepositStack> GetTenCentDepositStackAsync()
        {
            DepositStack tenCentDepositStack = await _vendorContext.DepositStacks.Where(x => x.Denomination == 10 && x.MoneyUnit.Id == GetCentIdAsync().Result).FirstAsync();
            return tenCentDepositStack;
        }

        private async Task<DepositStack> GetTwentyCentDepositStackAsync()
        {
            DepositStack twentyCentDepositStack = await _vendorContext.DepositStacks.Where(x => x.Denomination == 20 && x.MoneyUnit.Id == GetCentIdAsync().Result).FirstAsync();
            return twentyCentDepositStack;
        }

        private async Task<DepositStack> GetFiftyCentDepositStackAsync()
        {
            DepositStack fiftyCentDepositStack = await _vendorContext.DepositStacks.Where(x => x.Denomination == 50 && x.MoneyUnit.Id == GetCentIdAsync().Result).FirstAsync();
            return fiftyCentDepositStack;
        }

        private async Task<DepositStack> GetOneEuroDepositStackAsync()
        {
            DepositStack fiftyCentDepositStack = await _vendorContext.DepositStacks.Where(x => x.Denomination == 1 && x.MoneyUnit.Id == GetEuroIdAsync().Result).FirstAsync();
            return fiftyCentDepositStack;
        }

        private async Task<DepositStack> GetTwoEuroDepositStackAsync()
        {
            DepositStack twoEuroDepositStack = await _vendorContext.DepositStacks.Where(x => x.Denomination == 2 && x.MoneyUnit.Id == GetEuroIdAsync().Result).FirstAsync();
            return twoEuroDepositStack;
        }

        public async Task AddDepositStackRangeAsync(List<DepositStack> depositStack)
        {
            _vendorContext.DepositStacks.RemoveRange(_vendorContext.DepositStacks);
            await _vendorContext.DepositStacks.AddRangeAsync(depositStack);
            _logger.LogInformation("Table row count is {0}", _vendorContext.DepositStacks.Count().ToString());
        }

        public async Task UpdateDepositStackDbAsync(Guid moneyUnitId, int coinAmount, int denomination)
        {
            DepositStack depositStackToUpdate = await _vendorContext.DepositStacks.Where(x => x.MoneyUnit.Id == moneyUnitId && x.Denomination == denomination).FirstAsync();
            try
            {
                depositStackToUpdate.Amount -= coinAmount;
                _vendorContext.DepositStacks.Update(depositStackToUpdate);
            }
            catch(Exception e)
            {
                throw new Exception("Could not update deposit stack coin amount");
            }
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _vendorContext.SaveChangesAsync();
        }

    }
}
