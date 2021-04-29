using Deposit.Contracts.V1;
using Deposit.Data;
using Deposit.Handlers.Auths;
using Deposit.Handlers.Deposit.AccountReactivationSetup;
using Deposit.Handlers.Deposit.AccountSetup;
using Deposit.Handlers.Deposit.BankClosure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class AccountReactivationController : Controller
    {
        private readonly IMediator _mediator;
        public AccountReactivationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost(ApiRoutes.AcccountReactivationEndpoints.ADD_REACTIVATE_ACCOUNT_SETUP)]
        public async Task<IActionResult> ADD_REACTIVATE_ACCOUNT_SETUP([FromBody] AddUpdateReactivationAccountSetupCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.AcccountReactivationEndpoints.GET_ALL_REACTIVATE_ACCOUNT_SETUP)]
        public async Task<IActionResult> GET_ALL_REACTIVATE_ACCOUNT_SETUP()
        {
            var query = new GetAllReactivationAccountSetupQuery();
            var response = await _mediator.Send(query); 
            return Ok(response); 
        }

        [HttpGet(ApiRoutes.AcccountReactivationEndpoints.GET_REACTIVATE_ACCOUNT_SETUP)]
        public async Task<IActionResult> GET_REACTIVATE_ACCOUNT_SETUP([FromQuery] GetSingleReactivationAccountSetupQuery query)
        { 
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost(ApiRoutes.AcccountReactivationEndpoints.DELETE_REACTIVATE_ACCOUNT_SETUP)]
        public async Task<IActionResult> DELETE_REACTIVATE_ACCOUNT_SETUP([FromBody] DeleteReactivationAccountSetupCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
          
        [HttpPost(ApiRoutes.AcccountReactivationEndpoints.UPLOAD_REACTIVATE_ACCOUNT_SETUP)]
        public async Task<IActionResult> UPLOAD_REACTIVATE_ACCOUNT_SETUP()
        {
            var command = new UploadAccountReactivationSetup();
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.AcccountReactivationEndpoints.DOWNLOAD_REACTIVATE_ACCOUNT_SETUP)]
        public async Task<IActionResult> DOWNLOAD_REACTIVATE_ACCOUNT_SETUP()
        {
            var query = new DownloadAccountReactivationSetupCommand();
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        } 
    }
}
