using Deposit.Contracts.Response.Deposit.Operation;
using Deposit.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.Deposit.Operations
{
    public class Reactivate_Customer_commandVal : AbstractValidator<Reactivate_Customer_command>
    {
        private readonly DataContext _dataContext;
        public Reactivate_Customer_commandVal(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(rr => rr.Currency).NotEmpty();
            RuleFor(rr => rr.CustomerId).NotEmpty();
            RuleFor(rr => rr.Account_number).NotEmpty(); 
            RuleFor(rr => rr.Product).NotEmpty();
            RuleFor(rr => rr.Reactivation_reason).NotEmpty(); 
        }

       

    }
}
