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
    public class AddUpdateCategoryObjVal : AbstractValidator<AddUpdateCategoryObj> 
    {
        private readonly DataContext _dataContext;
        public AddUpdateCategoryObjVal(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Name).NotEmpty();
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Name Already exist");
        }

        private async Task<bool> NoDuplicateAsync(AddUpdateCategoryObj request, CancellationToken cancellationToken)
        {
            if(request.CategoryId > 0)
            {
                var item = _dataContext.deposit_category.FirstOrDefault(e => e.Name.ToLower() == request.Name.ToLower() && e.CategoryId != request.CategoryId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true); 
            }
            if(_dataContext.deposit_category.Count(e => e.Name.ToLower() == request.Name.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class AddUpdateBusinessCategoryObjVal : AbstractValidator<AddUpdateBusinessCategoryObj>
    {
        private readonly DataContext _dataContext;
        public AddUpdateBusinessCategoryObjVal(DataContext dataContext)
        { 
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.Name).NotEmpty();
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Name Already exist");
        }
        private async Task<bool> NoDuplicateAsync(AddUpdateBusinessCategoryObj request, CancellationToken cancellationToken)
        {
            if (request.BusinessCategoryId > 0)
            {
                var item = _dataContext.deposit_businesscategory.FirstOrDefault(e => e.Name.ToLower() == request.Name.ToLower() && e.BusinessCategoryId != request.BusinessCategoryId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_businesscategory.Count(e => e.Name.ToLower() == request.Name.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    public class AddUpdateTransactionTaxObjVal : AbstractValidator<AddUpdateTransactionTaxObj>
    {
        private readonly DataContext _dataContext;
        public AddUpdateTransactionTaxObjVal(DataContext dataContext)
        { 
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.FixedOrPercentage).NotEmpty().WithMessage("Fixed or percentage required"); 
            RuleFor(e => e.Name).NotEmpty();
            RuleFor(e => e.Amount_Percentage).NotEmpty().MustAsync(ValidPercentageAsync).WithMessage("Invalid Amount Percentage");
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Name Already exist");
        }

        private async Task<bool> ValidPercentageAsync(decimal? percentage, CancellationToken cancellationToken)
        {
            if (percentage > 100 || percentage < 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> NoDuplicateAsync(AddUpdateTransactionTaxObj request, CancellationToken cancellationToken)
        {
            if (request.TransactionTaxId > 0)
            {
                var item = _dataContext.deposit_transactiontax.FirstOrDefault(e => e.Name.ToLower() == request.Name.ToLower() && e.TransactionTaxId != request.TransactionTaxId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_transactiontax.Count(e => e.Name.ToLower() == request.Name.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }


    public class AddUpdateTransactionChargeObjVal : AbstractValidator<AddUpdateTransactionChargeObj>
    {
        private readonly DataContext _dataContext;
        public AddUpdateTransactionChargeObjVal(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.FixedOrPercentage).NotEmpty().WithMessage("Fixed or percentage required");
            RuleFor(e => e.Name).NotEmpty();
            RuleFor(e => e.Amount_Percentage).NotEmpty().MustAsync(ValidPercentageAsync).WithMessage("Invalid Amount Percentage");
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Name Already exist");
        }

        private async Task<bool> ValidPercentageAsync(decimal? percentage, CancellationToken cancellationToken)
        {
            if (percentage > 100 || percentage < 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> NoDuplicateAsync(AddUpdateTransactionChargeObj request, CancellationToken cancellationToken)
        {
            if (request.TransactionChargeId > 0)
            {
                var item = _dataContext.deposit_transactioncharge.FirstOrDefault(e => e.Name.ToLower() == request.Name.ToLower() && e.TransactionChargeId != request.TransactionChargeId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_transactioncharge.Count(e => e.Name.ToLower() == request.Name.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
    
}
