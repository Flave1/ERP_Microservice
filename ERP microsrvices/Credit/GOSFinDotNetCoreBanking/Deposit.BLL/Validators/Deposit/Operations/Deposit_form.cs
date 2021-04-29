using Deposit.Contracts.Response.Deposit.Deposit_form;
using Deposit.Data;
using Deposit.Repository.Interface.Deposit;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.Deposit.Operations
{
    

    public class AddUpdate_deposit_formVal : AbstractValidator<Deposit_to_customer>
    { 
        private readonly ICustomerService _service;
        public AddUpdate_deposit_formVal(ICustomerService service)
        {  
            _service = service;
            RuleFor(rr => rr.CustomerId).NotEmpty();
            RuleFor(rr => rr.Deposit_amount).NotEmpty();
            RuleFor(rr => rr.Remark).NotEmpty();
            RuleFor(rr => rr.Currency).NotEmpty();
            RuleFor(rr => rr.Transaction_mode).NotEmpty();
            RuleFor(e => e).NotEmpty().MustAsync(Check_for_opening_balace).WithMessage("You are already running out of opening balance for selected currency");


        }

        private async Task<bool> Check_for_opening_balace(Deposit_to_customer request, CancellationToken cancellationToken)
        {
            return await _service.Check_against_opening_balance_async(request.Currency, request.Deposit_amount);
        } 
    }
} 
