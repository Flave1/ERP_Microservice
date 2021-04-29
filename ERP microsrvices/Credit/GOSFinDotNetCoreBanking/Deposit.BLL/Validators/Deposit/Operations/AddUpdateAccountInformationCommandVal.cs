
using Deposit.Contracts.Response.Deposit.AccountOpening;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.Deposit.Operations
{
    public class AddUpdateAccountInformationCommandVal : AbstractValidator<AddUpdateAccountInformationCommand>
    {
        public AddUpdateAccountInformationCommandVal()
        {
            RuleFor(r => r.AccountTypeId).NotEmpty();
            RuleFor(r => r.Currencies).Must(Must_operate_on_currency).WithMessage("No currency selected for this account");
        }

        private bool Must_operate_on_currency(int[] currencies)
        {
            return currencies.Any();
        }
    }
}
