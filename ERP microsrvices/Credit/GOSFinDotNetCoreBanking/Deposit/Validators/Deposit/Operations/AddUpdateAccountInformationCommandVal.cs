
using Deposit.Contracts.Response.Deposit.AccountOpening;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.Deposit.Operations
{
    public class Create_update_account_informationCommandVal : AbstractValidator<Create_update_account_informationCommand>
    {
        public Create_update_account_informationCommandVal()
        {
            RuleFor(r => r.AccountTypeId).NotEmpty();
            RuleFor(r => r.CustomerId).NotEmpty();
            RuleFor(r => r.CategoryId).NotEmpty().WithMessage("Category not selected");
            RuleFor(r => r.AccountTypeId).NotEmpty().WithMessage("Account type not selected");
            RuleFor(r => r.RelationshipOfficerId).NotEmpty().WithMessage("Relationship officer required"); ;
            RuleFor(r => r.CustomerTypeId).NotEmpty().WithMessage("Customer type not selected"); ;
            RuleFor(r => r.AccountTypeId).NotEmpty();
            RuleFor(r => r.AccountTypeId).NotEmpty();
            RuleFor(r => r.Currencies).Must(Must_operate_on_currency).WithMessage("No currency selected for this account");
        }

        private bool Must_operate_on_currency(int[] currencies)
        {
            return currencies.Count() > 0;
        }
    }
}
