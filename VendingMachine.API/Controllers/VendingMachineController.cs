using Microsoft.AspNetCore.Mvc;
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


        public VendingMachineController(ILogger<VendingMachineController> logger,
                                        IVendingMachineService vendingMachineService,
                                        IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
            _vendingMachineService = vendingMachineService;
        }

        [HttpPost]
        [Route("Product/{id}")]
        public async Task<ActionResult> Product(int id)
        {
            bool lastpurchasePaid = await _vendingMachineService.CheckIfLastPurchasePaidAsync();

            if (!lastpurchasePaid)
            {
                //galima parasyti suma kuria reikia sumoketi uz pirma pirkini
                return BadRequest("You have to pay first for the previous purchase");
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
        [Route("Money")]
        public void Money(Deposit deposit)
        {
            _paymentService.InitPaymentProcessAsync(deposit);
            // sumokejimas
            // update depositStack kai sumoki pinigus
        }

     
    }
}
