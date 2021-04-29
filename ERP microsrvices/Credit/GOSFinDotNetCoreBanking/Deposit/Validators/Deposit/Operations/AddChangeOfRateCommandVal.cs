using Deposit.Contracts.Response.Deposit;
using Deposit.Requests;
using Deposit.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.Deposit.Operations
{
    public class AddChangeOfRateCommandVal : AbstractValidator<AddChangeOfRateCommand>
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        public AddChangeOfRateCommandVal(DataContext dataContext, IIdentityServerRequest serverRequest)
        {
            _serverRequest = serverRequest;
            _dataContext = dataContext;

            
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Change of rate already exist for this company level and product"); 
        } 
        private async Task<bool> NoDuplicateAsync(AddChangeOfRateCommand request, CancellationToken cancellationToken)
        {
            var user = await _serverRequest.UserDataAsync();
            if (request.ChangeOfRateId > 0)
            {
                var item = _dataContext.deposit_changeofrates.FirstOrDefault(e => e.Structure == user.CompanyId && e.Structure == request.Product && e.Product != request.ChangeOfRateId&& e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_changeofrates.Count(e => user.CompanyId == e.Structure && e.Product == request.Product && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }

    

    public class AddUpdateChangeOfRateSetupObjVal : AbstractValidator<AddUpdateChangeOfRateSetupObj>
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        public AddUpdateChangeOfRateSetupObjVal(DataContext dataContext, IIdentityServerRequest serverRequest)
        {
            _serverRequest = serverRequest;
            _dataContext = dataContext;


            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Change of rate already exist for this company level and product");
        }
        private async Task<bool> NoDuplicateAsync(AddUpdateChangeOfRateSetupObj request, CancellationToken cancellationToken)
        { 
            if (request.ChangeOfRateSetupId > 0)
            {
                var item = _dataContext.deposit_changeofratesetup.FirstOrDefault(e => e.Structure ==  request.Structure && request.ProductId == e.ProductId && e.ChangeOfRateSetupId != request.ChangeOfRateSetupId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_changeofratesetup.Count(e => e.Structure == e.Structure && e.ProductId == request.ProductId && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
