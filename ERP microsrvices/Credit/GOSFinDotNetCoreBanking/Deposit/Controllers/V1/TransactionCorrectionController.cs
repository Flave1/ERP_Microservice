using Deposit.Contracts.Response.Deposit;
using Deposit.Contracts.V1;
using Deposit.Handlers.Auths;
using Deposit.Handlers.Deposit.AccountSetup;
using Deposit.Handlers.Deposit.BankClosure;
using Deposit.Handlers.Deposit.TransactionCorrectionSetup;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class TransactionCorrectionController : Controller
    {
        private readonly IMediator _mediator;
        public TransactionCorrectionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(ApiRoutes.TransactionCorrection.ADD_UPDATE_TRANSACTION_CORRECTION_SETUP)]
        public async Task<IActionResult> ADD_UPDATE_TRANSACTION_CORRECTION_SETUP([FromBody] AddUpdateTransactionCorrectionSetupCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }


        [HttpPost(ApiRoutes.TransactionCorrection.DELETE_TRANSACTION_CORRECTION_SETUP)]
        public async Task<IActionResult> DELETE_TRANSACTION_CORRECTION_SETUP([FromBody] DeleteTransactionCorrectionSetupCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet(ApiRoutes.TransactionCorrection.GET_ALL_TRANSACTION_CORRECTION_SETUP)]
        public async Task<IActionResult> GET_ALL_TRANSACTION_CORRECTION_SETUP()
        {
            var query = new GetAllTransactionCorrectionSetupQuery();
            var response = await _mediator.Send(query);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet(ApiRoutes.TransactionCorrection.GET_TRANSACTION_CORRECTION_SETUP_BY_ID)]
        public async Task<IActionResult> GET_TRANSACTION_CORRECTION_SETUP_BY_ID([FromQuery] GetSingleTransactionCorrectionSetupQuery query)
        {
            var response = await _mediator.Send(query);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet(ApiRoutes.TransactionCorrection.DOWNLOAD_TRANSACTION_CORRECTION_SETUP)]
        public async Task<IActionResult> DOWNLOAD_TRANSACTION_CORRECTION_SETUP()
        {
            var query = new DownloadTransactionCorrectionSetupCommand();
            var response = await _mediator.Send(query);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost(ApiRoutes.TransactionCorrection.UPLOAD_TRANSACTION_CORRECTION_SETUP)]
        public async Task<IActionResult> UPLOAD_TRANSACTION_CORRECTION_SETUP()
        {
            var command = new UploadTransactionCorrectionSetup();
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }


          
    }
}
