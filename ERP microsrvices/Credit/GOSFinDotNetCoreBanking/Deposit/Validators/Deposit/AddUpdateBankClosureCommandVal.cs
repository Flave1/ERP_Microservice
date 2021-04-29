using Deposit.Contracts.Command;
using Deposit.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.Deposit
{


    public class AddUpdateBankClosureSetupCommandVal : AbstractValidator<AddUpdateBankClosureSetupCommand>
    {
        private readonly DataContext _dataContext;
        public AddUpdateBankClosureSetupCommandVal(DataContext dataContext)
        {
            _dataContext = dataContext;
         
            RuleFor(e => e.Charge).NotEmpty().WithMessage("Charge required");
            RuleFor(e => e.Percentage).NotEmpty().WithMessage("Percentage required").MustAsync(ValidPercentageAsync).WithMessage("Invalid percentage value");  
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Duplicate setup detected");
            RuleFor(e => e.ProductId).NotEmpty().WithMessage("Product required").NotNull().WithMessage("Product required");
        }

        private async Task<bool> ValidPercentageAsync(double percentage, CancellationToken cancellationToken)
        {
            if (percentage > 100 || percentage < 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> NoDuplicateAsync(AddUpdateBankClosureSetupCommand request, CancellationToken cancellationToken)
        {
            if (request.BankClosureSetupId > 0)
            {
                var item = _dataContext.deposit_bankclosuresetup.FirstOrDefault(e => e.ProductId == request.ProductId && e.Structure == request.Structure && e.BankClosureSetupId!= request.BankClosureSetupId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_bankclosuresetup.Count(e => e.ProductId == request.ProductId && e.Structure == request.Structure && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
