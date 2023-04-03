using VeendingMachine.API.DataAccess.DatabaseContext;
using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Interfaces;
using VeendingMachine.API.DataAccess.Model;

namespace VendingMachine.API.BusinessLogic.Services
{
    public class VendingMaschineService : IVendingMachineService
    {
        private readonly IDepositStackRepository _depositStackRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        public VendingMaschineService(VendorContext vendorContext, IDepositStackRepository depositStackRepository, IProductRepository productRepository, IPurchaseRepository purchaseRepository)
        {
            _depositStackRepository = depositStackRepository;
            _productRepository = productRepository;
            _purchaseRepository = purchaseRepository;
        }

        //initProductPurchase(id)
        //viska async. Paskaityk kodel visa pplication turi buti async, o ne tik keli metodai. 
        public async Task InitProductPurchaseAsync(int vendingNumber)
        {
            Product product = await GetProductAsync(vendingNumber);
            Purchase newPurchase = new();
            newPurchase.Product = product;
            newPurchase.Id = Guid.NewGuid();
            newPurchase.Paid = false;
            newPurchase.Timestamp = DateTime.UtcNow;
            await AddToPurchasesDbAsync(newPurchase);
        }
        //public async Task InitPaymentProcessAsync(Deposit deposit)
        //{
        //    Purchase lastPurchase = await _purchaseRepository.GetLastNotPaidPurchaseAsync();
        //    decimal priceToPay = lastPurchase.Product.Price;
        //    //await UpdateDepositStackDbAsync(deposit, priceToPay);
        //    await CalculateChangeAsync(deposit, priceToPay);
        //}

        private async Task AddToPurchasesDbAsync(Purchase purchase)
        {
            await _purchaseRepository.AddPurchaseAsync(purchase);
        }

        //private async Task CalculateChangeAsync(Deposit deposit, decimal priceToPay)
        //{
        //    await _paymentService.CalculateChangeAsync(deposit)
        //}

        public async Task<Product> GetProductAsync(int vendingNumber)
        {
            var product = await _productRepository.GetProductAsync(vendingNumber);
            return product;
        }

        public async Task<bool> CheckIfLastPurchasePaidAsync()
        {
            return await _purchaseRepository.IsLastPurchasePaid();
        }


        //private async Task UpdateDepositStackDbAsync(Deposit deposit, decimal amountToPay)
        //{
        //    await _depositStackRepository.CalculateChangeAsync(deposit, amountToPay);
        //    throw new NotImplementedException();
        //}

        private async Task UpdatePurchaseStatusAsync(Guid purchaseId)
        {
            throw new NotImplementedException();
        }

        public int GetTotalProductCount()
        {
            return _productRepository.GetProductsCountFromDB();
        }
    }
}
