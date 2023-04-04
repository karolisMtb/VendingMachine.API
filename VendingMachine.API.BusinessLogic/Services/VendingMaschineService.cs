using VeendingMachine.API.DataAccess.DatabaseContext;
using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Interfaces;

namespace VendingMachine.API.BusinessLogic.Services
{
    public class VendingMaschineService : IVendingMachineService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        public VendingMaschineService(VendorContext vendorContext, IProductRepository productRepository, IPurchaseRepository purchaseRepository)
        {
            _productRepository = productRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task InitProductPurchaseAsync(int vendingNumber)
        {
            Product product = await GetProductAsync(vendingNumber);

            Purchase newPurchase = new()
            {
                Product = product,
                Id = Guid.NewGuid(),
                Paid = false,
                Timestamp = DateTime.UtcNow
            };

            await AddToPurchasesDbAsync(newPurchase);
        }

        private async Task AddToPurchasesDbAsync(Purchase purchase)
        {
            await _purchaseRepository.AddPurchaseAsync(purchase);
        }

        public async Task<Product> GetProductAsync(int vendingNumber)
        {
            var product = await _productRepository.GetProductAsync(vendingNumber);
            return product;
        }

        public async Task<bool> CheckIfLastPurchasePaidAsync()
        {
            return await _purchaseRepository.IsLastPurchasePaidAsync();
        }

        public async Task<int> GetTotalProductCountAsync()
        {
            int productCount = await _productRepository.GetProductsCountFromDbAsync();
            return productCount;
        }
    }
}
