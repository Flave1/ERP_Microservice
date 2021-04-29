using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using System.Linq;

namespace Deposit.Handlers.Operations.AccountOpeneing.Indiviadual.PersonalInformation
{
    public class Create_update_individual_personal_informationHandler : IRequestHandler<Create_update_individual_personal_information, AccountResponse>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        public Create_update_individual_personal_informationHandler(
            ILoggerService logger,
            DataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }


        public async Task<AccountResponse> Handle(Create_update_individual_personal_information request, CancellationToken cancellationToken)
        {
            var response = new AccountResponse();
            try
            {
                var customer_lite = _dataContext.deposit_customer_lite_information.Find(request.CustomerId);
                if (customer_lite == null)
                    customer_lite = new deposit_customer_lite_information();

                customer_lite.CustomerId = request.CustomerId;
                customer_lite.CustomerTypeId = request.CustomerTypeId;

                if (request.CustomerId == 0)
                    _dataContext.deposit_customer_lite_information.Add(customer_lite);
                await _dataContext.SaveChangesAsync();

                var domain = _dataContext.deposit_individual_customer_information?.FirstOrDefault(e => e.CustomerId == customer_lite.CustomerId) ?? null;
                if (domain == null)
                    domain = new deposit_individual_customer_information();

                domain.CustomerId = customer_lite.CustomerId;
                domain.Title = request.Title;
                domain.Surname = request.Surname;
                domain.Firstname = request.Firstname;
                domain.Othername = request.Othername;
                domain.MaritalStatusId = request.MaritalStatusId;
                domain.GenderId = request.GenderId;
                domain.Email = request.Email;
                domain.DOB = request.DOB;
                domain.MotherMaidenName = request.MotherMaidenName;
                domain.TaxIDNumber = request.TaxIDNumber;
                domain.BVN = request.BVN;
                domain.ResidentPermitNumber = request.ResidentPermitNumber;
                domain.PermitIssueDate = request.PermitIssueDate;
                domain.SocialSecurityNumber = request.SocialSecurityNumber;
                domain.StateOfOrigin = request.StateOfOrigin;
                domain.Nationality = request.Nationality;
                domain.PermitExpiryDate = request.PermitExpiryDate;
                domain.ResidentialState = request.ResidentialState;
                domain.ResidentialCity = request.ResidentialCity;
                domain.ResidentialLGA = request.ResidentialLGA;
                domain.PhoneNo = request.PhoneNo;
                domain.LGA_of_origin = request.LGA_of_origin;

                if (domain.IndividualCustomerId == 0)
                    _dataContext.deposit_individual_customer_information.Add(domain);
                await _dataContext.SaveChangesAsync();


                response.CustomerId = domain.CustomerId;
                response.Status.IsSuccessful = true;
                response.Status.Message.FriendlyMessage = "successful";
                return response;
            }
            catch (Exception e)
            {
                response.Status.IsSuccessful = false;
                response.Status.Message.FriendlyMessage = e?.Message ?? e.InnerException?.Message;
                response.Status.Message.TechnicalMessage = e.ToString();
                _logger.Error(e.ToString());
                return response;
            }
        }
    }
}
