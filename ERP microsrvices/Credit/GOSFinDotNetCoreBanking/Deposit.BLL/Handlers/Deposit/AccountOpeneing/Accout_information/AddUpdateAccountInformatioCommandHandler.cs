using Deposit.Contracts.GeneralExtension;
using Deposit.Contracts.Response.Deposit;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.AccountInformations
{
    public class AddUpdateAccountInformationCommandHandler : IRequestHandler<AddUpdateAccountInformationCommand, AccountOpeningRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        public AddUpdateAccountInformationCommandHandler(ILoggerService logger, DataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<AccountOpeningRegRespObj> Handle(AddUpdateAccountInformationCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountOpeningRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var domain = _dataContext.deposit_customer_accountdetails.Find(request.AccountdetailId);
                if (domain == null)
                {
                    domain = new deposit_customer_accountdetails();
                    var setup = _dataContext.deposit_accountype.Find(request.AccountTypeId);
                    if (setup != null)
                    {
                        domain.AccountNumber = AccountNumber.Generate(setup.AccountNunmberPrefix); 
                    }
                }

                var this_account_type_setup_dormancy_days = _dataContext.deposit_accountsetup.FirstOrDefault(e => e.AccountTypeId == request.AccountTypeId)?.DormancyDays ?? 0;

                if (request.Currencies.Count() > 0) domain.Currencies = string.Join(",", request.Currencies);
                domain.CustomerId = request.CustomerId;  
                domain.AccountCategoryId = request.AccountCategoryId;
                domain.AccountdetailId = request.AccountdetailId; 
                domain.AccountTypeId = request.AccountTypeId;
                domain.Card = request.Card;
                domain.EmailAlert = request.EmailAlert;
                domain.EmailStatement = request.EmailStatement;
                domain.InternetBanking = request.InternetBanking;
                domain.SmsAlert = request.SmsAlert;
                domain.Token = request.Token;
                domain.Date_to_go_dormant = DateTime.UtcNow.AddDays(this_account_type_setup_dormancy_days);
                if (domain.AccountdetailId > 0)
                    _dataContext.Entry(domain).CurrentValues.SetValues(domain);
                else
                    _dataContext.deposit_customer_accountdetails.Add(domain);
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
