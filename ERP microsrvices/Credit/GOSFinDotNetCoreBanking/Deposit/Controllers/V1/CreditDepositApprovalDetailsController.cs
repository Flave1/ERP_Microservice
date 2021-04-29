using Deposit.Contracts.V1;
using Deposit.Handlers.Auths;
using Deposit.Handlers.Details;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class CreditDepositApprovalDetailsController : Controller
    {
        private readonly IMediator _mediator;
        public CreditDepositApprovalDetailsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet(ApiRoutes.ApprovalDetail.CREDIT_DEPOSIT_APPROVAL_DETAIL)]
        public async Task<IActionResult> CREDIT_DEPOSIT_APPROVAL_DETAIL([FromQuery] GetCurrentTargetApprovalDetailQuery query)
        { 
            return Ok(await _mediator.Send(query));
        } 
    }
}
