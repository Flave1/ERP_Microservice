using Deposit.Contracts.GeneralExtension;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using Deposit.Managers.Interface;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.AccountInformations
{
    public class Create_update_account_informationCommandHandler : IRequestHandler<Create_update_account_informationCommand, AccountResponse>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IAccountInformationService _accountService;
        public Create_update_account_informationCommandHandler(ILoggerService logger, DataContext dataContext, IAccountInformationService account)
        {
            _accountService = account;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<AccountResponse> Handle(Create_update_account_informationCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountResponse();
            try
            {
                var accountype_setup = _dataContext.deposit_accountype.Include(e => e.deposit_accountsetup).Where(r => r.AccountTypeId == request.AccountTypeId && r.Deleted == false).FirstOrDefault();
                if (accountype_setup == null)
                {
                    response.Status.Message.FriendlyMessage = "Selected account type not found";
                    return response;
                } 
                var domain = _dataContext.deposit_customer_account_information.Find(request.AccountInformationId); 
                if (domain == null)
                {
                    domain = new deposit_customer_account_information(); 
                    domain.AccountNumber = _accountService.Create_account_number(accountype_setup.AccountNunmberPrefix);  
                }

                var this_account_type_setup_dormancy_days = accountype_setup?.deposit_accountsetup?.FirstOrDefault()?.DormancyDays??0;

                if (request.Currencies.Count() > 0) domain.Currencies = string.Join(",", request?.Currencies);
                domain.CustomerId = request.CustomerId;  
                domain.CategoryId = request.CategoryId; 
                domain.AccountTypeId = request.AccountTypeId;
                domain.Card = request.Card;
                domain.EmailAlert = request.EmailAlert;
                domain.EmailStatement = request.EmailStatement;
                domain.InternetBanking = request.InternetBanking;
                domain.SmsAlert = request.SmsAlert;
                domain.Token = request.Token;
                domain.RelationshipOfficerId = request.RelationshipOfficerId; 
                domain.CustomerTypeId = request.CustomerTypeId;
                domain.AvailableBalance = 0.ToString();
                //domain.deposit_customer_lite_information.CustomerTypeId = request.CustomerTypeId;
                domain.Date_to_go_dormant = DateTime.UtcNow.AddDays(this_account_type_setup_dormancy_days);
                if (domain.AccountInformationId == 0) 
                    _dataContext.deposit_customer_account_information.Add(domain);
                await _dataContext.SaveChangesAsync();

                response.CustomerId = domain.CustomerId;
                response.Status.Message.FriendlyMessage = "successful";
                return response;
            }
            catch (Exception e)
            {
                response.Status.IsSuccessful = false;
                response.Status.Message.FriendlyMessage = e?.Message ?? e.InnerException?.Message;
                _logger.Error(e.ToString());
                response.Status.Message.TechnicalMessage = e.ToString();
                return response;
            }
        }
    }
}
