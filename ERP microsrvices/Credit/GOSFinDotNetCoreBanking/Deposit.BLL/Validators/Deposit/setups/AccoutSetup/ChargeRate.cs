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


    public class AddUpdateChangeOfRateSetupObjVal : AbstractValidator<AddUpdateChangeOfRateSetupObj>
    {
        private readonly DataContext _dataContext;
        public AddUpdateChangeOfRateSetupObjVal(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Structure).NotEmpty();
            RuleFor(e => e.ProductId).NotEmpty().WithMessage("Product required"); 
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Change of rate already exist");
        }
         
        private async Task<bool> NoDuplicateAsync(AddUpdateChangeOfRateSetupObj request, CancellationToken cancellationToken)
        {
            if (request.ChangeOfRateSetupId > 0)
            {
                var item = _dataContext.deposit_changeofratesetup.FirstOrDefault(e => e.ProductId  == request.ProductId && e.ChangeOfRateSetupId != request.ChangeOfRateSetupId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_changeofratesetup.Count(e => e.ProductId == request.ProductId && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
