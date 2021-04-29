using Deposit.Contracts.GeneralExtension;
using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.AccoutSetup
{

    public class AddUpdateAccountTypeObjVal : AbstractValidator<AddUpdateAccountTypeObj>
    {
        private readonly DataContext _dataContext;
        public AddUpdateAccountTypeObjVal(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty(); 
            RuleFor(e => e.Name).NotEmpty();
            RuleFor(e => e.AccountNunmberPrefix).NotEmpty().WithMessage("Account number prefix required").MinimumLength(3).MaximumLength(3);
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Name Already exist");
            RuleFor(r => r.AccountNunmberPrefix).MustAsync(IsAllnumber).WithMessage("Invalid character detected in prefix");

        }

        private async Task<bool> IsAllnumber(string prefix, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                return await Task.Run(() => CustomValidators.IsNumeric(prefix));
            }
            return await Task.Run(() => true);
        }
            private async Task<bool> NoDuplicateAsync(AddUpdateAccountTypeObj request, CancellationToken cancellationToken)
        {
            if (request.AccountTypeId > 0)
            {
                var item = _dataContext.deposit_accountype.FirstOrDefault(e => e.Name.ToLower() == request.Name.ToLower() && e.AccountTypeId != request.AccountTypeId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_accountype.Count(e => e.Name.ToLower() == request.Name.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
