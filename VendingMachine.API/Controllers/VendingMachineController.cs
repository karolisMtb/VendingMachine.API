using Microsoft.AspNetCore.Mvc;
using System.Net;
using VeendingMachine.API.DataAccess.Entities;
using VeendingMachine.API.DataAccess.Interfaces;
using VeendingMachine.API.DataAccess.Model;
using VendingMachine.API.BusinessLogic;
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

        /// <summary>
        /// Endpoint which emulates a product selection from the vending machine.
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">User side error</response>
        /// <response code="500">Server side error</response>
        [HttpPost]
        [Route("Purchase/{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> Purchase(int id)
        {
            //validation
            bool lastpurchasePaid = await _vendingMachineService.CheckIfLastPurchasePaidAsync();

            if (!lastpurchasePaid)
            {
                var notPaidProduct = await _purchaseRepository.GetLastNotPaidPurchaseAsync();
                return BadRequest($"You have to pay first for the previous purchase. You have to pay at least {notPaidProduct.Product.Price}");
            }

            int productCount = await _vendingMachineService.GetTotalProductCountAsync();

            if(id <= 0 || id > productCount)
            {
                return BadRequest("Number should be from 1 to " + productCount--);
            }

            //product purchase flow
            try
            {
                await _vendingMachineService.InitProductPurchaseAsync(id);
                var product = await _vendingMachineService.GetProductAsync(id);
                return Ok($"Please insert {product.Price} in the other endpoint.");
            }
            catch(Exception ex)
            {
                _logger.LogError($"Product purchase failed: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
    
        }

        /// <summary>
        /// Endpoint which emulates payment for a product selected in the previous endpoint.
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">User side error</response>
        /// <response code="500">Server side error</response>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Route("DepositStack")]
        public async Task <ActionResult> DepositStack(Deposit deposit)
        {
            
            try
            {
                //validation
                Purchase lastPurchase = await _purchaseRepository.GetLastNotPaidPurchaseAsync();

                if (Utilities.ConvertDepositIntoDecimal(deposit) < lastPurchase.Product.Price)
                {
                    return BadRequest("Not enough change has been inserted. We're giving your change back and try again please.");
                }

                //product payment flow
                var changeResult = await _paymentService.InitPaymentProcessAsync(deposit, lastPurchase);

                return Ok(changeResult);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"Purchase payment failed {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Purchase payment failed {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
