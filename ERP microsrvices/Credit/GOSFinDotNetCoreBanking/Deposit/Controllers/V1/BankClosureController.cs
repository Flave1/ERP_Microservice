using Deposit.Contracts.Command;
using Deposit.Contracts.V1;
using Deposit.Handlers.Auths;
using Deposit.Handlers.Deposit.BankClosure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Controllers.V1
{
    [ERPAuthorize]
    public class BankClosureController : Controller
    {
        private readonly IMediator _mediator;
        public BankClosureController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost(ApiRoutes.BankClosure.ADD_UPDATE_BANK_CLOSURE)]
        public async Task<IActionResult> AddUpdateBankClosure([FromBody] AddUpdateBankClosureCommand command)
        { 
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet(ApiRoutes.BankClosure.GET_BANK_CLOSURE_AWAITING_APPRO)]
        public async Task<IActionResult> GET_BANK_CLOSURE_AWAITING_APPRO()
        {
            var query = new GetBankClosureAwaitingApprovalQuery();
            return Ok(await _mediator.Send(query)); 
        }

        
        [HttpPost(ApiRoutes.BankClosure.STAFF_BANK_CLOSURE_APPROVAL)]
        public async Task<IActionResult> STAFF_BANK_CLOSURE_APPROVAL([FromBody] AccountClosureStaffApprovalCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost(ApiRoutes.BankClosure.ADD_UPDATE_BANK_CLOSURE_SETUP)]
        public async Task<IActionResult> AddUpdateBankClosureSetup([FromBody] AddUpdateBankClosureSetupCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost(ApiRoutes.BankClosure.DELETE_BANK_CLOSURE)]
        public async Task<IActionResult> DeleteBankClosure([FromBody] DeleteBankClosureCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }


        [HttpPost(ApiRoutes.BankClosure.DELETE_BANK_CLOSURE_SETUP)]
        public async Task<IActionResult> DeleteBankClosureSetup([FromBody] DeleteBankClosureSetupCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }


        [HttpGet(ApiRoutes.BankClosure.GET_BANK_CLOSURE_SETUP)]
        public async Task<IActionResult> GET_BANK_CLOSURE_SETUP([FromQuery] GetSingleBankClosureSetupQuery query)
        {
            var response = await _mediator.Send(query); 
            return Ok(response);
        }

        [HttpGet(ApiRoutes.BankClosure.GET_ALL_BANK_CLOSURE_SETUP)]
        public async Task<IActionResult> GET_ALL_BANK_CLOSURE_SETUP()
        {
            var query = new GetAllBankClosureSetupQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet(ApiRoutes.BankClosure.DOWNLOAD_BANK_CLOSURE_SETUP)]
        public async Task<IActionResult> DOWNLOAD_BANK_CLOSURE_SETUP()
        {
            var query = new DownloadBankClosureCommand();
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        [HttpPost(ApiRoutes.BankClosure.UPLOAD_BANK_CLOSURE_SETUP)]
        public async Task<IActionResult> UPLOAD_BANK_CLOSURE_SETUP()
        {
            var command = new UploadBankClosureSetup();
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }
        
    }
}
