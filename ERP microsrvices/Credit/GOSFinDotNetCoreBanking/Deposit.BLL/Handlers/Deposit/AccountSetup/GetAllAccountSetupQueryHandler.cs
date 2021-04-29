using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.AccountSetup
{
    public class GetAllAccountSetupQuery : IRequest<AccountSetupRespObj>
    {
        public class GetAllAccountSetupQueryHandler : IRequestHandler<GetAllAccountSetupQuery, AccountSetupRespObj>
        {
            private readonly DataContext _dataContext;
            public GetAllAccountSetupQueryHandler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task<AccountSetupRespObj> Handle(GetAllAccountSetupQuery request, CancellationToken cancellationToken)
            {
                var response = new AccountSetupRespObj { DepositAccounts = new List<DepositAccountObj>(), Status = new APIResponseStatus { Message = new APIResponseMessage() } };
                response.DepositAccounts = (from a in _dataContext.deposit_accountsetup
                                where a.Deleted == false
                                select
                               new DepositAccountObj
                               {
                                   DepositAccountId = a.DepositAccountId,
                                   AccountName = a.AccountName,
                                   Description = a.Description,
                                   AccountTypeId = a.AccountTypeId,
                                   AccountTypename = _dataContext.deposit_accountype.Where(x => x.AccountTypeId == a.AccountTypeId).FirstOrDefault().Name,
                                   DormancyDays = a.DormancyDays,
                                   InitialDeposit = a.InitialDeposit,
                                   CategoryId = a.CategoryId,
                                   CategoryName = _dataContext.deposit_category.Where(x => x.CategoryId == a.CategoryId).FirstOrDefault().Name,
                                   BusinessCategoryId = a.BusinessCategoryId,
                                   InterestRate = a.InterestRate,
                                   InterestType = a.InterestType,
                                   CheckCollecting = a.CheckCollecting,
                                   MaturityType = a.MaturityType,
                                   GLMapping = a.GLMapping,
                                   BankGl = a.BankGl,
                                   PreTerminationLiquidationCharge = a.PreTerminationLiquidationCharge,
                                   InterestAccrual = a.InterestAccrual,
                                   InterestAccrualName = a.InterestAccrual == 1 ? "Day 0" : "Day 1",
                                   Status = a.Status,
                                   OperatedByAnother = a.OperatedByAnother,
                                   CanNominateBenefactor = a.CanNominateBenefactor,
                                   UsePresetChartofAccount = a.UsePresetChartofAccount,
                                   TransactionPrefix = a.TransactionPrefix,
                                   CancelPrefix = a.CancelPrefix,
                                   RefundPrefix = a.RefundPrefix,
                                   Useworkflow = a.Useworkflow,
                                   CanPlaceOnLien = a.CanPlaceOnLien
                               }).ToList()??new List<DepositAccountObj>();
                response.Status.Message.FriendlyMessage = response.DepositAccounts.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() =>  response);
            }
        }
    }
    
}
