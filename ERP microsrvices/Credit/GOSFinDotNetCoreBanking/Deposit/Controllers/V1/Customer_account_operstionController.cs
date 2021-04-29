using Deposit.Contracts.Response.Deposit.Call_over; 
using Deposit.Contracts.Response.Deposit.Deposit_form;
using Deposit.Contracts.Response.Deposit.Operation;
using Deposit.Contracts.V1;
using Deposit.Handlers.Auths;
using Deposit.Handlers.Deposit.Call_over;
using Deposit.Handlers.Deposit.Deposit_form;
using Deposit.Handlers.Deposit.Reactivation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class Customer_account_operstionController : Controller
    {
        private readonly IMediator _mediator;
        public Customer_account_operstionController(IMediator mediator)
        {
            _mediator = mediator; 
        }

        [HttpPost(ApiRoutes.Customer_account_operation.DEPOSIT_TO_CUSTOMER)]
        public async Task<IActionResult> DEPOSIT_TO_CUSTOMER([FromBody] Deposit_to_customer command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.Customer_account_operation.GET_ALL_DEPOSITS)]
        public async Task<IActionResult> GET_ALL_DEPOSITS()
        {
            var query = new GetAllCustomerDepositTransactionQuery();
            return Ok(await _mediator.Send(query)); 
        }



        [HttpPost(ApiRoutes.Customer_account_operation.WITHDRAW_FROM_CUSTOMER)]
        public async Task<IActionResult> WITHDRAW_FROM_CUSTOMER([FromBody] Withdrwal_from_customer_accountCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.Customer_account_operation.GET_ALL_WITHDRAWALS)]
        public async Task<IActionResult> GET_ALL_WITHDRAWALS()
        {
            var query = new Get_withdrwal_from_customer_accountQuery();
            return Ok(await _mediator.Send(query));
        }

        

        [HttpPost(ApiRoutes.Customer_account_operation.REACTIVATE_ACCOUNT)]
        public async Task<IActionResult> REACTIVATE_ACCOUNT([FromBody] Reactivate_Customer_command command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.Customer_account_operation.REACTIVATION_STAFF_APPROVAL)]
        public async Task<IActionResult> REACTIVATION_STAFF_APPROVAL([FromBody] Reactivation_staff_approvalCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.Customer_account_operation.GET_ALL_REACTIVATIONS)]
        public async Task<IActionResult> GET_ALL_REACTIVATIONS()
        {
            var query = new Get_all_reactivated_accountsQuery();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.Customer_account_operation.GET_ALL_AWAITING_REACTIVATIONS)]
        public async Task<IActionResult> GET_ALL_AWAITING_REACTIVATIONS()
        {
            var query = new GetReactivationAwaitingApprovalQuery();
            return Ok(await _mediator.Send(query));
        }

        [HttpPost(ApiRoutes.Customer_account_operation.CALL_OVER_CURRENCY_AMOUNT)]
        public async Task<IActionResult> CALL_OVER_CURRENCY_AMOUNT([FromBody] add_call_over_currecies_and_amount command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        

        [HttpGet(ApiRoutes.Customer_account_operation.GET_STAFF_CALL_OVERS)]
        public async Task<IActionResult> GET_STAFF_CALL_OVERS([FromQuery] Get_current_staff_call_overs_query query)
        { 
            return Ok(await _mediator.Send(query));
        }

        [HttpPost(ApiRoutes.Customer_account_operation.VALIDATE_TRANSACTIONS)]
        public async Task<IActionResult> VALIDATE_TRANSACTIONS([FromBody] Validate_transaction command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.Customer_account_operation.GET_STAFF_VALIDATE_TRANSACTIONS_FOR_APPROVAL)]
        public async Task<IActionResult> GET_STAFF_VALIDATE_TRANSACTIONS_FOR_APPROVAL()
        {
            var query = new Get_transaction_validations_awaiting_Query();
            return Ok(await _mediator.Send(query));
        }

        [HttpPost(ApiRoutes.Customer_account_operation.ADD_VALIDATE_TRANSACTIONS)]
        public async Task<IActionResult> ADD_VALIDATE_TRANSACTIONS([FromBody] Transaction_validations_staff_approvalCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.Customer_account_operation.GET_TRANSACTIONS_BY_CURRENCIES_STRUCTURE)]
        public async Task<IActionResult> GET_TRANSACTIONS_BY_CURRENCIES_STRUCTURE([FromQuery] Get_all_transactions_by_currencies_query query)
        {
            var response = await _mediator.Send(query);
            if (response.Transactions.Any())
                return Ok(response);
            return NoContent();
        }

    }
}
