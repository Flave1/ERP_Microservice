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
    public class Create_emplyment_detailsHandler : IRequestHandler<Create_emplyment_details, AccountResponse>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext; 
        public Create_emplyment_detailsHandler(
            ILoggerService logger, 
            DataContext dataContext)
        { 
            _dataContext = dataContext;
            _logger = logger; 
        }

       
        public async Task<AccountResponse> Handle(Create_emplyment_details request, CancellationToken cancellationToken)
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
                var domain = _dataContext.deposit_employer_details.SingleOrDefault(t =>  
                t.Deleted == false && t.EmploymentDetailId == request.EmploymentDetailId);

                if (domain == null)
                    domain = new deposit_employer_details();
                 
                domain.EmploymentDetailId = request.EmploymentDetailId;
                domain.IsEmployed = request.IsEmployed;
                domain.EmployerName = request.EmployerName;
                domain.EmployerAddress = request.EmployerAddress;
                domain.EmployerState = request.EmployerState;
                domain.Occupation = request.Occupation; 
                domain.IndividualCustomerId = individual_customer_details.IndividualCustomerId;
                domain.IsSelfEmployed = request.IsSelfEmployed;
                domain.IsUnEmployed = request.IsUnEmployed;
                domain.IsRetired = request.IsRetired;
                domain.IsStudent = request.IsStudent;
                domain.OtherComments = request.OtherComments; 
                 
                if (domain.EmploymentDetailId == 0)
                    _dataContext.deposit_employer_details.Add(domain);
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
