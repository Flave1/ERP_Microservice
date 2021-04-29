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
    public class AddUpdateCashierTellerSetupObjVal : AbstractValidator<AddUpdateCashierTellerSetupObj>
    {
        private readonly DataContext _dataContext;
        public AddUpdateCashierTellerSetupObjVal(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Structure).NotEmpty(); 
            RuleFor(e => e.ProductId).NotEmpty().WithMessage("Product required"); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Duplicate setup detected");
        }
         
        private async Task<bool> NoDuplicateAsync(AddUpdateCashierTellerSetupObj request, CancellationToken cancellationToken)
        {
            if (request.DepositCashierTellerSetupId > 0)
            {
                var item = _dataContext.deposit_cashiertellersetup.FirstOrDefault(e => e.Employee_ID == request.Employee_ID && e.ProductId == request.ProductId && e.DepositCashierTellerSetupId != request.DepositCashierTellerSetupId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_cashiertellersetup.Count(e => e.Employee_ID  == request.Employee_ID && e.ProductId == e.ProductId && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
