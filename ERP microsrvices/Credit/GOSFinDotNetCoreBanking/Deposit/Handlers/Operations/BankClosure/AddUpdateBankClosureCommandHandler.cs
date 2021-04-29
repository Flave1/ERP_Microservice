using AutoMapper;
using Deposit.Contracts.Command;
using Deposit.Contracts.Response.Approvals;
using Deposit.Contracts.Response.Deposit;
using Deposit.DomainObjects;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.BankClosure
{
    public class AddUpdateBankClosureCommandHandler : IRequestHandler<AddUpdateBankClosureCommand, Deposit_bankClosureRegRespObj>
    {
        private readonly DataContext _context; 
        private readonly IIdentityServerRequest _serverRequest;
        private readonly ILoggerService _logger;
        private readonly IDepositBankClosure _deposit;
        public AddUpdateBankClosureCommandHandler(DataContext context, IIdentityServerRequest request, 
            ILoggerService logger, IDepositBankClosure deposit)
        {
            _deposit = deposit;
            _logger = logger;
            _context = context;
            _serverRequest = request;
        }

       
        public async Task<Deposit_bankClosureRegRespObj> Handle(AddUpdateBankClosureCommand request, CancellationToken cancellationToken)
        {
            var response = new Deposit_bankClosureRegRespObj {  Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
           // return null;
            try
            { 
                var user = await _serverRequest.UserDataAsync();
                var Trate = _context.deposit_bankclosure.Find(request.BankClosureId);
                if (Trate == null)
                    Trate = new deposit_bankclosure();
                 
                Trate.SubStructure = user.CompanyId;
                Trate.AccountName = request.AccountName;
                Trate.AccountNumber = request.AccountNumber;
                Trate.Status = request.Status;
                Trate.AccountBalance = request.AccountBalance;
                Trate.Currency = request.Currency;
                Trate.ClosingDate = request.ClosingDate;
                Trate.Reason = request.Reason;
                Trate.Charges = request.Charges;
                Trate.FinalSettlement = Convert.ToString(request.FinalSettlement);
                Trate.Beneficiary = request.Beneficiary;
                Trate.ModeOfSettlement = request.ModeOfSettlement;
                Trate.TransferAccount = request.TransferAccount; 
                Trate.AccountId = request.AccountId;
                Trate.SettlmentAccountNumber = request.SettlmentAccountNumber;  
               
                using (var _transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _deposit.AddUpdateDepositBankClosure(Trate);
                        var targetList = new List<long>();
                        targetList.Add(Trate.BankClosureId);
                        GoForApprovalRequest wfRequest = new GoForApprovalRequest
                        {
                            Comment = "Account Closure",
                            OperationId = (int)OperationsEnum.BankAccountClosure,
                            TargetId = targetList,
                            ApprovalStatus = (int)ApprovalStatus.Processing,
                            DeferredExecution = true,
                            StaffId = user.StaffId,
                            CompanyId = user.CompanyId,
                            EmailNotification = false,
                            ExternalInitialization = false,
                            StatusId = (int)ApprovalStatus.Processing,
                        };

                        var result = await _serverRequest.GotForApprovalAsync(wfRequest);

                        if (!result.IsSuccessStatusCode)
                        {
                            await _transaction.RollbackAsync();
                            response.Status.IsSuccessful = false;
                            response.Status.Message.FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}";
                            return response;
                        }
                        var stringData = await result.Content.ReadAsStringAsync();
                        GoForApprovalRespObj res = JsonConvert.DeserializeObject<GoForApprovalRespObj>(stringData);

                        if (res.ApprovalProcessStarted)
                        {
                            Trate.WorkflowToken = res.Status.CustomToken;
                            Trate.ApprovalStatusId = (int)ApprovalStatus.Processing;
                            await _deposit.AddUpdateDepositBankClosure(Trate);
                            await _transaction.CommitAsync();

                            response.Status.IsSuccessful = res.Status.IsSuccessful;
                            response.Status.Message = res.Status.Message;
                            return response;
                        }

                        if (res.EnableWorkflow || !res.HasWorkflowAccess)
                        {
                            await _transaction.RollbackAsync();
                            response.Status.IsSuccessful = res.Status.IsSuccessful;
                            response.Status.Message = res.Status.Message;
                            return response;
                        }
                        if (!res.EnableWorkflow)
                        { 
                            var customer_account = _context.deposit_customer_account_information.FirstOrDefault(r => r.AccountNumber.ToLower() == Trate.AccountNumber.ToLower());
                            if (customer_account != null)
                            {
                                customer_account.Deleted = true;
                            }
                            Trate.ApprovalStatusId = (int)ApprovalStatus.Approved;
                            await _deposit.AddUpdateDepositBankClosure(Trate);
                            await _transaction.CommitAsync();
                            response.Status.IsSuccessful = true;
                            response.Status.Message.FriendlyMessage = "Successful";
                            return response;
                        }
                        response.Status.IsSuccessful = res.Status.IsSuccessful;
                        response.Status.Message = res.Status.Message;
                        return response;
                    }
                    catch (Exception ex)
                    {
                        await _transaction.RollbackAsync();
                        #region Log error to file 
                        var errorCode = ErrorID.Generate(4);
                        _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                        response.Status.Message.FriendlyMessage = "Error occured!! Please try again later";
                        response.Status.Message.MessageId = errorCode;
                        response.Status.Message.TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}";
                        return response;

                        #endregion
                    }
                    finally { await _transaction.DisposeAsync(); }
                }                   
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
