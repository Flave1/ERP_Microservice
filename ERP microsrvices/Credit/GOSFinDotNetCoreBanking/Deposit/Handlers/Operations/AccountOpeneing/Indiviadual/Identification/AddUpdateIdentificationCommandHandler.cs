
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Identifications
{
    public class AddUpdateIdentificationCommandHandler : IRequestHandler<AddUpdateIdentificationCommand, AccountResponse>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        public AddUpdateIdentificationCommandHandler(ILoggerService logger, DataContext dataContext, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<AccountResponse> Handle(AddUpdateIdentificationCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountResponse();
            try
            {
                var individual_customer_details = await _dataContext.deposit_individual_customer_information
                   .Include(r => r.deposit_customerIdentifications)
                   .SingleOrDefaultAsync(e => e.CustomerId == request.CustomerId && e.Deleted == false);

                if (individual_customer_details == null)
                {
                    response.Status.Message.FriendlyMessage = "Unable to identify customer";
                    return response;
                }

                var domain = individual_customer_details.deposit_customerIdentifications.SingleOrDefault(t => t.Identification == request.Identification && t.Deleted == true);
                if (domain == null)
                    domain = new deposit_customerIdentifications();
                  
                domain.IdentificationId = request.IdentificationId;
                domain.CustomerId = request.CustomerId;
                domain.Identification = request.Identification;
                domain.IDNumber = request.IdentificationNumber;
                domain.DateIssued = request.DateIssued;
                domain.ExpiryDate = request.ExpiryDate;
                domain.IndividualCustomerId = individual_customer_details.IndividualCustomerId;

                if (domain.IdentificationId == 0)
                    _dataContext.deposit_customerIdentification.Add(domain);
                await _dataContext.SaveChangesAsync();

                response.Status.IsSuccessful = true;
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
