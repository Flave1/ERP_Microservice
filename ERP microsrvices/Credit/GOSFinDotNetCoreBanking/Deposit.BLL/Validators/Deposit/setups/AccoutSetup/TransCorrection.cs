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


    public class AddUpdateTransactionCorrectionSetupCommandVal : AbstractValidator<AddUpdateTransactionCorrectionSetupCommand>
    {
        private readonly DataContext _dataContext;
        public AddUpdateTransactionCorrectionSetupCommandVal(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Structure).NotEmpty();
            RuleFor(e => e.JobTitleId).NotEmpty().WithMessage("Job title required");  
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Duplicate setup detected");
        }

         
        private async Task<bool> NoDuplicateAsync(AddUpdateTransactionCorrectionSetupCommand request, CancellationToken cancellationToken)
        {
            if (request.TransactionCorrectionSetupId > 0)
            {
                var item = _dataContext.deposit_transactioncorrectionsetup.FirstOrDefault(e => e.Structure == request.Structure && e.JobTitleId == request.JobTitleId  && e.TransactionCorrectionSetupId != request.TransactionCorrectionSetupId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_transactioncorrectionsetup.Count(e => e.Structure  == request.Structure && e.JobTitleId == request.JobTitleId && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
