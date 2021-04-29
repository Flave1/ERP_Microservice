
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using FluentValidation;
using GOSLibraries.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.Deposit.Operations
{
    public class Create_update_individual_personal_informationVal : AbstractValidator<Create_update_individual_personal_information>
    {
        private readonly DataContext _dataContext;
        public Create_update_individual_personal_informationVal(DataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(e => e.CustomerTypeId).NotEmpty();
            RuleFor(e => e.PhoneNo).NotEmpty();
            RuleFor(e => e.Email).NotEmpty();
            RuleFor(r => r).Must(IsPhoneNumberUsed).WithMessage("Customer with this mobile number already exist");
            RuleFor(r => r).Must(IsEmailUsed).WithMessage("Customer with this email already exist");
            // RuleFor(r => r).MustAsync(FirstAndLastNameRequiredAsync).WithMessage("First or last name must not be empty");
            //RuleFor(r => r).MustAsync(CompanyNameRequiredAsync).WithMessage("Company name required");
        }


        //private async Task<bool> CompanyNameRequiredAsync(Create_update_individual_personal_information request, CancellationToken cancellationToken)
        //{ 
        //    if(request.CustomerTypeId == (int)CustomerType.Corporate && string.IsNullOrEmpty(request.CompanyName))
        //    {
        //        return false);
        //    }
        //    return true);
        //}

        private bool IsEmailUsed(Create_update_individual_personal_information request)
        {
            if (request.IndividualCustomerId > 0)
            {
                var item = _dataContext.deposit_individual_customer_information.FirstOrDefault(e => e.Email == request.Email && e.IndividualCustomerId != request.IndividualCustomerId && e.Deleted == false);
                if (item != null) 
                    return  false; 
                return  true;
            }
            if (_dataContext.deposit_individual_customer_information.Count(e => e.Email == request.Email && e.Deleted == false) >= 1) 
                return   false; 
            return   true;
        }


        private bool IsPhoneNumberUsed (Create_update_individual_personal_information request)
        {
            if (request.IndividualCustomerId > 0)
            {
                var item = _dataContext.deposit_individual_customer_information.FirstOrDefault(e => e.PhoneNo == request.PhoneNo && e.IndividualCustomerId != request.IndividualCustomerId && e.Deleted == false);
                if (item != null) 
                    return false ; 
                return true ;
            }
            if (_dataContext.deposit_individual_customer_information.Count(e => e.PhoneNo == request.PhoneNo && e.Deleted == false) >= 1) 
                return false ; 
            return true ;
        }
    }
}
