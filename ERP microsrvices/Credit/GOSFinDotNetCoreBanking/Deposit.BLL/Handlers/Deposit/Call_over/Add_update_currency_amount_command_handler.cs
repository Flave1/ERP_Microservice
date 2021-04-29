using Deposit.Contracts.Command;
using Deposit.Contracts.Response.Deposit;
using Deposit.Contracts.Response.Deposit.Call_over;
using Deposit.Contracts.Response.Deposit.Deposit_form;
using Deposit.Requests;
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

namespace Deposit.Handlers.Deposit.AccountSetup
{


    public class Add_update_currency_amount_commandHandler : IRequestHandler<add_call_over_currecies_and_amount, Account_response>
    {
        private readonly DataContext _dataContext;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        public Add_update_currency_amount_commandHandler(DataContext dataContext, ILoggerService logger, IIdentityServerRequest identityServerRequest)
        {
            _serverRequest = identityServerRequest;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<Account_response> Handle(add_call_over_currecies_and_amount request, CancellationToken cancellationToken)
        {
            var response = new Account_response();
            try
            {
                var user = await _serverRequest.UserDataAsync();
                var setup = _dataContext.deposit_call_over_currecies_and_amount.FirstOrDefault(e => e.Currency == request.Currency);

                if(setup != null)
                { 
                    if(request.Amount < setup.Amount)
                    {
                        response.Status.Message.FriendlyMessage = "Amount can not be lesser than existing balance";
                        return response;
                    }
                }

                if (setup == null)
                    setup = new deposit_call_over_currecies_and_amount();

                setup.Currency = request.Currency;
                setup.Amount = request.Amount;
                setup.Call_over_date = DateTime.UtcNow;
                setup.User_id = user.StaffId.ToString();

                if(setup != null && setup.Id == 0)await _dataContext.deposit_call_over_currecies_and_amount.AddAsync(setup);

                await _dataContext.SaveChangesAsync();
                response.Status.IsSuccessful = true;
                response.Status.Message.FriendlyMessage = "Successful";
                return response;
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                response.Status.Message.FriendlyMessage = $"Error Occurred: { e?.Message}";
                response.Status.Message.TechnicalMessage = e.ToString(); 
                return response;
            }
        }
    }
}
