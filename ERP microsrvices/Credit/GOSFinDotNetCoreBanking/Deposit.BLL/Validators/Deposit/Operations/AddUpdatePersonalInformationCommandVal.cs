
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using FluentValidation;
using GOSLibraries.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.Deposit.Operations
{
    public class AddUpdatePersonalInformationCommandVal : AbstractValidator<AddUpdatePersonalInformationCommand>
    {
        private readonly DataContext _dataContext;
        public AddUpdatePersonalInformationCommandVal(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(e => e.CustomerTypeId).NotEmpty();
            RuleFor(r => r).MustAsync(NoDuplicateAsync).WithMessage("Customer with this email already exist");
           // RuleFor(r => r).MustAsync(FirstAndLastNameRequiredAsync).WithMessage("First or last name must not be empty");
            RuleFor(r => r).MustAsync(CompanyNameRequiredAsync).WithMessage("Company name required");
        }


        private async Task<bool> CompanyNameRequiredAsync(AddUpdatePersonalInformationCommand request, CancellationToken cancellationToken)
        { 
            if(request.CustomerTypeId == (int)CustomerType.Corporate && string.IsNullOrEmpty(request.CompanyName))
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }

        private async Task<bool> FirstAndLastNameRequiredAsync(AddUpdatePersonalInformationCommand request, CancellationToken cancellationToken)
        {
            if (request.CustomerTypeId == (int)CustomerType.Individual && (string.IsNullOrEmpty(request.Firstname) || string.IsNullOrEmpty(request.Surname)))
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }


        private async Task<bool> NoDuplicateAsync(AddUpdatePersonalInformationCommand request, CancellationToken cancellationToken)
        {
            if (request.CustomerId > 0)
            {
                var item = _dataContext.deposit_accountopening.FirstOrDefault(e => e.Email.ToLower() == request.Email.ToLower() && e.CustomerId != request.CustomerId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_accountopening.Count(e => e.Email.ToLower() == request.Email.ToLower() && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
