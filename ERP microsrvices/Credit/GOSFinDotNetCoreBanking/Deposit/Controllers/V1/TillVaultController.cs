using Deposit.Contracts.Command;
using Deposit.Contracts.V1;
using Deposit.Handlers.Auths;
using Deposit.Handlers.Deposit.AccountSetup;
using Deposit.Handlers.Deposit.BankClosure;
using Deposit.Handlers.Deposit.TillVaultSetup;
using Deposit.Handlers.Deposit.VaultSetup;
using Deposit.Handlers.TillVaults;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class TillVaultController : Controller
    {
        private readonly IMediator _mediator;
        public TillVaultController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost(ApiRoutes.TillVault.ADD_UPDATE_TILL_VAULT_SETUP)]
        public async Task<IActionResult> AddUpdateTillVault([FromBody] AddUpdateTillVaultCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }
 

        [HttpPost(ApiRoutes.TillVault.DELETE_TILL_VAULT_SETUP)]
        public async Task<IActionResult> DeleteTillVault([FromBody] DeleteTillVaultSetupCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet(ApiRoutes.TillVault.GET_ALL_TILL_VAULT_SETUP)]
        public async Task<IActionResult> DeleteTillVault()
        {
            var query = new GetAllTillVaultSetupQuery();
            var response = await _mediator.Send(query);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }


        [HttpGet(ApiRoutes.TillVault.DOWNLOAD_TILL_VAULT_SETUP)]
        public async Task<IActionResult> DOWNLOAD_TILL_VAULT_SETUP()
        {
            var command = new DownloadVaultSetupCommand();
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }


        [HttpPost(ApiRoutes.TillVault.UPLOAD_TILL_VAULT_SETUP)]
        public async Task<IActionResult> UPLOAD_TILL_VAULT_SETUP()
        {
            var command = new UploadTiilVaultsSetup();
            var response = await _mediator.Send(command);
            if (!response.Status.IsSuccessful)
                return BadRequest(response);
            return Ok(response);
        }

         

    }
}
