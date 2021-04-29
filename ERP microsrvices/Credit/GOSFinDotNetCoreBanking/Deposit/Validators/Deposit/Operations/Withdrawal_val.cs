using Deposit.Contracts.Response.Deposit.Operation;
using Deposit.Repository.Interface.Deposit;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.Deposit.Operations
{
    public class Withdrawal_val : AbstractValidator<Withdrwal_from_customer_accountCommand>
    { 
        private readonly ICustomerService _service; 
        public Withdrawal_val(ICustomerService customerService)
        { 
            _service = customerService;
            RuleFor(e => e.Product).NotEmpty();
            RuleFor(e => e.Withdrawal_type).NotEmpty();
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.CustomerId).NotEmpty();
            RuleFor(e => e.Currency).NotEmpty();
            RuleFor(e => e.Amount).NotEmpty();
            RuleFor(e => e.Account_number).NotEmpty();
            RuleFor(e => e).MustAsync(Check_for_opening_balance).WithMessage("You are already running out of opening balance for selected currency");
        }

        private async Task<bool> Check_for_opening_balance(Withdrwal_from_customer_accountCommand request, CancellationToken cancellationToken)
        {
            return await _service.Check_against_opening_balance_async(request.Currency, request.Amount);
        }
    }
}
