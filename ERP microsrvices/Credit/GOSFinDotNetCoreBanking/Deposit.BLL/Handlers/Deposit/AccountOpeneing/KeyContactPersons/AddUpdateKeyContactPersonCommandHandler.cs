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

namespace Deposit.Handlers.KeyContactPersons
{
    public class AddUpdateKeyContactPersonCommandHandler : IRequestHandler<AddUpdateKeyContactPersonCommand, AccountOpeningRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        public AddUpdateKeyContactPersonCommandHandler(ILoggerService logger, DataContext dataContext, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<AccountOpeningRegRespObj> Handle(AddUpdateKeyContactPersonCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountOpeningRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var domain = _dataContext.deposit_keycontactpersons.Find(request.KeyContactPersonId);
                if (domain == null)
                    domain = new deposit_keycontactpersons();



                domain.KeyContactPersonId = request.KeyContactPersonId;
                domain.CustomerId = request.CustomerId;
                domain.Email = request.Email;
                domain.FullName = request.FullName;
                domain.JobTitle = request.JobTitle;
                domain.OfficeAddress = request.OfficeAddress;
                domain.PhoneNumber = request.PhoneNumber;

                if (domain.KeyContactPersonId > 0)
                    _dataContext.Entry(domain).CurrentValues.SetValues(domain);
                else
                    _dataContext.deposit_keycontactpersons.Add(domain);
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
