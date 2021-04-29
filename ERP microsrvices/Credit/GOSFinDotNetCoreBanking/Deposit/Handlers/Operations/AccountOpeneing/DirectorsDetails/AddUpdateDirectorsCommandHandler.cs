using Deposit.Contracts.Response.Deposit;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Directorss
{
    public class AddUpdateDirectorsCommandHandler : IRequestHandler<AddUpdateDirectorsCommand, AccountResponse>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        public AddUpdateDirectorsCommandHandler(ILoggerService logger, DataContext dataContext, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<AccountResponse> Handle(AddUpdateDirectorsCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountResponse { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var domain = _dataContext.deposit_directors.Find(request.DirectorsId);
                if (domain == null)
                    domain = new deposit_directors();



                domain.AccountName = request.AccountName; 
                domain.CustomerId = request.CustomerId;   
                domain.DOB = request.DOB;
                domain.EmailAddress = request.EmailAddress;
                domain.FirstName = request.FirstName;
                domain.Gender = request.Gender;
                domain.IdentificationNumber = request.IdentificationNumber; 
                domain.IDIssuedate = request.IDIssuedate;
                domain.LGA = request.LGA;
                domain.MailingAddressSameWithResidentialAddress = request.MailingAddressSameWithResidentialAddress;
                domain.MailingCity = request.MailingCity;
                domain.MailingLGA = request.MailingLGA;
                domain.MailingState = request.MailingState;
                domain.MaritalStatus = request.MaritalStatus;
                domain.MeansOfIdentification = request.MeansOfIdentification;
                domain.MobileNumber = request.MobileNumber;
                domain.MotherMaidienName = request.MotherMaidienName;   
                domain.NameOfNextOfKin = request.NameOfNextOfKin;
                domain.Nationality = request.Nationality;
                domain.Occupation = request.Occupation;
                domain.OtherNames = request.OtherNames;
                domain.PermitExpiryDate = request.PermitExpiryDate;
                domain.PermitIssueDate = request.PermitIssueDate;
                domain.POB = request.POB;
                domain.Position = request.Position;
                domain.ResidentialCity = request.ResidentialCity;
                domain.ResidentialLGA = request.ResidentialLGA;
                domain.ResidentialState = request.ResidentialState;
                domain.ResidentPermitNumber = request.ResidentPermitNumber; 
                domain.SignatureName = request.SignatureName;
                domain.SocialSecurityNumber = request.SocialSecurityNumber;
                domain.State = request.State;
                domain.Surname = request.Surname;
                domain.TaxIdentitfication = request.TaxIdentitfication; 

                if (domain.DirectorsId > 0)
                    _dataContext.Entry(domain).CurrentValues.SetValues(domain);
                else
                    _dataContext.deposit_directors.Add(domain);
                await _dataContext.SaveChangesAsync();


                response.CustomerId = domain.CustomerId;
                response.Status.Message.FriendlyMessage = "successful";
                return response;
            }
            catch (Exception e)
            {
                response.Status.IsSuccessful = false;
                response.Status.Message.FriendlyMessage = e?.Message ?? e.InnerException?.Message;
                response.Status.Message.TechnicalMessage = e.ToString();
                return response;
            }
        }
    }
}
