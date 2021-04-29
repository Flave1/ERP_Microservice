using Deposit.Contracts.Command;
using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.AccountSetup
{


    public class AddUpdateAccountSetupCommandHandler : IRequestHandler<AddUpdateAccountSetupCommand, AccountSetupRegRespObj>
    {
        private readonly DataContext _dataContext;
        private readonly ILoggerService _logger;
        public AddUpdateAccountSetupCommandHandler(DataContext dataContext, ILoggerService logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<AccountSetupRegRespObj> Handle(AddUpdateAccountSetupCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountSetupRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var setup = await _dataContext.deposit_accountsetup.FindAsync(request.AccountTypeId);
                if (setup == null)
                    setup = new deposit_accountsetup();
                setup.DepositAccountId = request.DepositAccountId > 0 ? request.DepositAccountId : 0; 
                setup.Description = request.Description;
                setup.AccountName = request.AccountName;
                setup.AccountTypeId = request.AccountTypeId;
                setup.DormancyDays = request.DormancyDays;
                setup.DomancyDateCount = DateTime.UtcNow.AddDays(request.DormancyDays);
                setup.InitialDeposit = request.InitialDeposit;
                setup.CategoryId = request.CategoryId;
                setup.BusinessCategoryId = request.BusinessCategoryId;
                setup.GLMapping = request.GLMapping;
                setup.CurrencyId = request.CurrencyId;
                setup.BankGl = request.BankGl;
                setup.InterestRate = request.InterestRate;
                setup.InterestType = request.InterestType;
                setup.CheckCollecting = request.CheckCollecting;
                setup.MaturityType = request.MaturityType;
                setup.PreTerminationLiquidationCharge = request.PreTerminationLiquidationCharge;
                setup.InterestAccrual = request.InterestAccrual;
                setup.Status = request.Status;
                setup.OperatedByAnother = request.OperatedByAnother;
                setup.CanNominateBenefactor = request.CanNominateBenefactor;
                setup.UsePresetChartofAccount = request.UsePresetChartofAccount;
                setup.TransactionPrefix = request.TransactionPrefix;
                setup.CancelPrefix = request.CancelPrefix;
                setup.RefundPrefix = request.RefundPrefix;
                setup.Useworkflow = request.Useworkflow;
                setup.CanPlaceOnLien = request.CanPlaceOnLien; 
                setup.UpdatedOn = request.DepositAccountId > 0 ? DateTime.Today : DateTime.Today;

                if(setup.AccountTypeId > 0)
                {
                   var item = await _dataContext.deposit_accountsetup.FindAsync(request.AccountTypeId);
                    if (item != null)
                        _dataContext.Entry(item).CurrentValues.SetValues(setup);
                }
                else
                    await _dataContext.deposit_accountsetup.AddAsync(setup);
                await _dataContext.SaveChangesAsync();
                response.Status.Message.FriendlyMessage = "Successful";
                return response;
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
                response.Status.Message.FriendlyMessage = $"Error Occurred: { e?.Message}";
                response.Status.Message.TechnicalMessage = e.ToString();
                response.Status.IsSuccessful = false;
                return response;
            }
        }
    }
}
