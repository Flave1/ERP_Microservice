using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Deposit.Handlers.PersonalInformations
{
    public class Create_update_individual_contact_dteailsHandler : IRequestHandler<Create_update_individual_contact_dteails, AccountResponse>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext; 
        public Create_update_individual_contact_dteailsHandler(
            ILoggerService logger, 
            DataContext dataContext)
        { 
            _dataContext = dataContext;
            _logger = logger; 
        }

       
        public async Task<AccountResponse> Handle(Create_update_individual_contact_dteails request, CancellationToken cancellationToken)
        {
            var response = new AccountResponse();
            try
            {
                var individual_customer_details = await _dataContext.deposit_individual_customer_information
                    .SingleOrDefaultAsync(e => e.CustomerId == request.CustomerId && e.Deleted == false);

                if(individual_customer_details == null)
                {
                    response.Status.Message.FriendlyMessage = "Unable to identify customer";
                    return response;
                }
                var domain = _dataContext.deposit_customer_contact_detail.SingleOrDefault(t =>  t.Deleted == false && t.ContactDetailId == request.ContactDetailId);
                if (domain == null)
                    domain = new deposit_customer_contact_detail();


                domain.ContactDetailId = request.ContactDetailId;
                domain.ResidentialAddressLine1 = request.ResidentialAddressLine1;
                domain.Email = request.Email;
                domain.ResidentialAddressLine2 = request.ResidentialAddressLine2;
                domain.ResidentialCity = request.ResidentialCity;
                domain.ResidentialCountry = request.ResidentialCountry;
                domain.ResidentialState = request.ResidentialState;
                domain.Email = request.Email;
                domain.PhoneNumber = request.PhoneNumber;
                domain.MobileNumber = request.MobileNumber;
                domain.MailngAddress = request.MailngAddress;
                domain.IndividualCustomerId = individual_customer_details.IndividualCustomerId;

                if (domain.ContactDetailId == 0)
                    _dataContext.deposit_customer_contact_detail.Add(domain);
                await _dataContext.SaveChangesAsync();

                response.CustomerId = request.CustomerId;
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
