using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Interfaces;

namespace VendingMachine.API.BusinessLogic.Services
{
    public class DataPopulationService : IDataPopulationService
    {

        private readonly IProductRepository _productRepository;
        private readonly IDepositStackRepository _depositStackRepository;
        private readonly IMoneyUnitRepository _moneyUnitRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        private readonly MoneyUnit _centUnit = new MoneyUnit { Id = Guid.NewGuid(), Name = "Ct" };
        private readonly MoneyUnit _euroUnit = new MoneyUnit { Id = Guid.NewGuid(), Name = "Euro" };

        public DataPopulationService(
            IProductRepository productRepository,
            IDepositStackRepository depositStackRepository,
            IMoneyUnitRepository moneyUnitRepository,
            IPurchaseRepository purchaseRepository)
        {
            _productRepository = productRepository;
            _depositStackRepository = depositStackRepository;
            _moneyUnitRepository = moneyUnitRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task SeedInitialData()
        {
            //clear metodas
            await SeedDatabaseWithMoneyUnitsAsync();
            await _moneyUnitRepository.SaveChangesAsync();

            await SeedDatabaseWithDepositStackAsync();
            await _productRepository.SaveChangesAsync();

            await SeedDatabaseWithProductsAsync();
            await _depositStackRepository.SaveChangesAsync();
            
            await ClearPurchaseDatabase();
        }

        private async Task SeedDatabaseWithDepositStackAsync()
        {
            List<DepositStack> depositStack = GetDepositStack();
            await _depositStackRepository.AddDepositStackRangeAsync(depositStack);
        }

        private async Task SeedDatabaseWithProductsAsync()
        {
            List<Product> products = GetVendingProducts();
            _productRepository.AddProductRange(products);
        }

        private async Task SeedDatabaseWithMoneyUnitsAsync()
        {
            await _moneyUnitRepository.AddMoneyUnitsAsync(new List<MoneyUnit>() { _centUnit, _euroUnit});
        }

        private async Task ClearPurchaseDatabase()
        {
            _purchaseRepository.ResetPurchaseDb();
            await _purchaseRepository.SaveChangesAsync();
        }

        private List<Product> GetVendingProducts()
        {
            List<Product> products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), ProductNumber = 1, Count = 5, Name = "Snickers", Price = 0.60M, Timestamp = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ProductNumber = 2, Count = 5, Name = "Mars", Price = 0.75M, Timestamp = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ProductNumber = 3, Count = 5, Name = "Tupla", Price = 0.45M, Timestamp = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ProductNumber = 4, Count = 5, Name = "Cheetos", Price = 0.90M, Timestamp = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ProductNumber = 5, Count = 5, Name = "Apple", Price = 0.30M, Timestamp = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ProductNumber = 6, Count = 5, Name = "Pear", Price = 0.35M, Timestamp = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ProductNumber = 7, Count = 5, Name = "Fanta", Price = 1.20M, Timestamp = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ProductNumber = 8, Count = 5, Name = "Cola zero", Price = 1.30M, Timestamp = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ProductNumber = 9, Count = 5, Name = "Cola", Price = 1.45M, Timestamp = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ProductNumber = 10, Count = 5, Name = "Bear", Price = 1.70M, Timestamp = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ProductNumber = 11, Count = 5, Name = "Orange juice", Price = 1.45M, Timestamp = DateTime.UtcNow }
            };

            return products;
        }

        private List<DepositStack> GetDepositStack()
        {
            List<DepositStack> depositStack = new List<DepositStack>
            {
                new DepositStack { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Amount = 100, Denomination = 1, MoneyUnit = _centUnit },
                new DepositStack { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Amount = 80, Denomination = 2, MoneyUnit = _centUnit },
                new DepositStack { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Amount = 10, Denomination = 5, MoneyUnit = _centUnit },
                new DepositStack { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Amount = 10, Denomination = 10, MoneyUnit = _centUnit },
                new DepositStack { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Amount = 60, Denomination = 20, MoneyUnit = _centUnit },
                new DepositStack { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Amount = 40, Denomination = 50, MoneyUnit = _centUnit },
                new DepositStack { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Amount = 20, Denomination = 1, MoneyUnit = _euroUnit },
                new DepositStack { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow, Amount = 20, Denomination = 2, MoneyUnit = _euroUnit }
            };

            return depositStack;
        }
    }
}
