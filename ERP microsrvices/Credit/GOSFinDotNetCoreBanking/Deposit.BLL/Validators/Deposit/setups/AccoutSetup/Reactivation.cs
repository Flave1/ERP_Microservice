using Deposit.Handlers.Deposit.BankClosure;
using Deposit.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.AccoutSetup
{
    public class AddUpdateReactivationAccountSetupCommandVal : AbstractValidator<AddUpdateReactivationAccountSetupCommand>
    {
        private readonly DataContext _dataContext;
        public AddUpdateReactivationAccountSetupCommandVal(DataContext data)
        {
            _dataContext = data;  
            RuleFor(r => r.Product).NotEmpty().WithMessage("Product Required");
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Duplicate setup detected");
            
        }

        private async Task<bool> NoDuplicateAsync(AddUpdateReactivationAccountSetupCommand request, CancellationToken cancellationToken)
        {
            if (request.ReactivationSetupId > 0)
            {
                var item = _dataContext.deposit_accountreactivationsetup.FirstOrDefault(e => e.Structure == request.Structure && e.Product == request.Product && e.ReactivationSetupId != request.ReactivationSetupId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_accountreactivationsetup.Count(e => e.Structure == request.Structure && e.Product == request.Product && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
