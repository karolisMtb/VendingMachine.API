using Microsoft.AspNetCore.Mvc;
using System.Net;
using VeendingMachine.API.DataAccess.Interfaces;
using VeendingMachine.API.DataAccess.Model;
using VendingMachine.API.BusinessLogic.Interfaces;

namespace VendingMachine.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VendingMachineController : ControllerBase
    {
        private readonly ILogger<VendingMachineController> _logger;
        private readonly IVendingMachineService _vendingMachineService;
        private readonly IPaymentService _paymentService;
        private readonly IPurchaseRepository _purchaseRepository;


        public VendingMachineController(ILogger<VendingMachineController> logger,
                                        IVendingMachineService vendingMachineService,
                                        IPaymentService paymentService,
                                        IPurchaseRepository purchaseRepository)
        {
            _logger = logger;
            _paymentService = paymentService;
            _vendingMachineService = vendingMachineService;
            _purchaseRepository = purchaseRepository;
        }

        [HttpPost]
        [Route("Product/{id}")]
        public async Task<ActionResult> Product(int id)
        {
            bool lastpurchasePaid = await _vendingMachineService.CheckIfLastPurchasePaidAsync();

            if (!lastpurchasePaid)
            {
                var notPaidProduct = await _purchaseRepository.GetLastNotPaidPurchaseAsync();

                return BadRequest("You have to pay first for the previous purchase." +
                    $"You have to pay at least {notPaidProduct.Product.Price}");
            }
            else
            {
                int productCount = _vendingMachineService.GetTotalProductCount();
                if(id <= 0 || id > productCount)
                {
                    return BadRequest("Number should be from 1 to " + productCount--);
                }

                try
                {
                    await _vendingMachineService.InitProductPurchaseAsync(id);
                    var product = await _vendingMachineService.GetProductAsync(id);
                    return Ok($"Please insert {product.Price} in the other endpoint");
                }
                catch(Exception ex)
                {
                    _logger.LogError("Purchase event failed at initializing");
                    return StatusCode(StatusCodes.Status503ServiceUnavailable,
                        ex.Message);
                }
            }
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("Money")]
        public async Task <ActionResult> Money(Deposit deposit)
        {
            await _paymentService.InitPaymentProcessAsync(deposit);

            return Ok();
            // sumokejimas
            // update depositStack kai sumoki pinigus
        }

     
    }
}
