using Deposit.Contracts.Command;
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
    public class AddUpdateAccountSetupCommandVal : AbstractValidator<AddUpdateAccountSetupObj>
    {
        private readonly DataContext _dataContext;
        public AddUpdateAccountSetupCommandVal(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(r => r.TransactionPrefix).NotEmpty().WithMessage("Transaction Prefix Required");
            RuleFor(r => r.RefundPrefix).NotEmpty().WithMessage("Refund Prefix Required");
            RuleFor(r => r.MaturityType).NotEmpty().WithMessage("Maturity Type Required");
            RuleFor(r => r.InterestType).NotEmpty().WithMessage("Interest Type Required");
            RuleFor(r => r.InterestRate).NotEmpty().WithMessage("Interest Rate Required");
            RuleFor(r => r.InterestAccrual).NotEmpty().WithMessage("Interest Accrual Required");
            RuleFor(r => r.InitialDeposit).NotEmpty().WithMessage("Initial Deposit Required"); 
            RuleFor(r => r.DormancyDays).NotEmpty().WithMessage("Dormancy Days Required");
            RuleFor(r => r.Description).NotEmpty().WithMessage("Description Required"); 
            RuleFor(r => r.CurrencyId).NotEmpty().WithMessage("Currency  Required");
            RuleFor(r => r.CategoryId).NotEmpty().WithMessage("Category Required");
            RuleFor(r => r.CancelPrefix).NotEmpty().WithMessage("Cancel Prefix Required");
            RuleFor(r => r.BusinessCategoryId).NotEmpty().WithMessage("Business Category Required");
            RuleFor(r => r.BankGl).NotEmpty().WithMessage(" Bank Gl Required");
            RuleFor(r => r.ApplicableTaxId).NotEmpty().WithMessage("Applicable Tax Required");
            RuleFor(r => r.ApplicableChargesId).NotEmpty().WithMessage("Applicable Charges Required"); 
            RuleFor(r => r.AccountName).NotEmpty().WithMessage("Account Name Required");
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Account setup with same name already exist");
            
        }

        private async Task<bool> NoDuplicateAsync(AddUpdateAccountSetupObj request, CancellationToken cancellationToken)
        {
             
            if (request.DepositAccountId > 0)
            {
                var item = _dataContext.deposit_accountsetup.FirstOrDefault(e => e.AccountName.ToLower() == request.AccountName.ToLower() && e.DepositAccountId != request.DepositAccountId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_accountsetup.Count(e => e.AccountName.ToLower() == request.AccountName.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    

    public class DepositformObjVal : AbstractValidator<DepositformObj>
    {
        private readonly DataContext _dataContext;
        public DepositformObjVal(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private async Task<bool> NoDuplicateAsync(AddUpdateAccountSetupObj request, CancellationToken cancellationToken)
        {

            if (_dataContext.deposit_accountsetup.Count(w => w.AccountName.ToLower().Trim() == request.AccountName.ToLower().Trim() && request.AccountTypeId != w.AccountTypeId) == 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => false);
        }
    }
}
