
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Identifications
{
    public class AddUpdateIdentificationCommandHandler : IRequestHandler<AddUpdateIdentificationCommand, AccountOpeningRegRespObj>
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
        public async Task<AccountOpeningRegRespObj> Handle(AddUpdateIdentificationCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountOpeningRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var domain = _dataContext.deposit_customerIdentification.Find(request.IdentificationId);
                if (domain == null)
                    domain = new deposit_customerIdentifications();


                domain.IdentificationId = request.IdentificationId;
                domain.CustomerId = request.CustomerId;
                domain.Identification = request.Identification;
                domain.IDNumber = request.IdentificationNumber;
                domain.DateIssued = request.DateIssued;
                domain.ExpiryDate = request.ExpiryDate;

                if (domain.IdentificationId > 0)
                    _dataContext.Entry(domain).CurrentValues.SetValues(domain);
                else
                    _dataContext.deposit_customerIdentification.Add(domain);
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
