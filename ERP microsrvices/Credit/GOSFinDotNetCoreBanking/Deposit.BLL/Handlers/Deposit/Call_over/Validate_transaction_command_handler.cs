using Deposit.Contracts.Response.Approvals;
using Deposit.Contracts.Response.Deposit.Deposit_form;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.Response.Deposit.Call_over;

namespace Deposit.Handlers.Deposit.AccountSetup
{


    public class Validate_transaction_commandHandler : IRequestHandler<Validate_transaction, Account_response>
    {
        private readonly DataContext _dataContext;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly ICustomerService _service;
        public Validate_transaction_commandHandler(DataContext dataContext, ILoggerService logger, ICustomerService service, IIdentityServerRequest identityServerRequest)
        {
            _serverRequest = identityServerRequest;
            _dataContext = dataContext;
            _logger = logger;
            _service = service;
        }

        public GoForApprovalRequest Build_approval_request(deposit_cashierteller_form item, int staff, int comp)
        {

            var targetList = new List<long>();
            targetList.Add(item.Id);
            return new GoForApprovalRequest
            {
                Comment = "Cashier/Teller Balancing",
                OperationId = (int)OperationsEnum.ChangeOfRate,
                TargetId = targetList,
                ApprovalStatus = (int)ApprovalStatus.Processing,
                DeferredExecution = true,
                StaffId = staff,
                CompanyId = comp,
                EmailNotification = false,
                ExternalInitialization = false,
                StatusId = (int)ApprovalStatus.Processing,
            };
        }


        public async Task<Account_response> Handle(Validate_transaction request, CancellationToken cancellationToken)
        {
            var response = new Account_response();
            try
            {
                var user = await _serverRequest.UserDataAsync(); 
  
                var item = new deposit_cashierteller_form();

                var deposits = await _dataContext.deposit_form.Where(e => e.Value_date.Date == DateTime.UtcNow.Date
                    && e.Is_call_over_done == false
                    && e.Structure <= request.Structure 
                    && request.Currencies.Contains(e.Currency)).ToListAsync();

                var withdrawal = await _dataContext.deposit_withdrawal_form.Where(e => e.Value_date.Date == DateTime.UtcNow.Date
                && e.Is_call_over_done == false
                && e.Structre <= request.Structure
                && request.Currencies.Contains(e.Currency)).ToListAsync();

                if(withdrawal.Count() == 0 && deposits.Count() == 0)
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = "No transactions found";
                    return response;
                }

                item.Date = DateTime.UtcNow;
                item.Employee_ID = user.StaffId;
                item.Structure = request.Structure;
                item.SubStructure = request.Sub_structure;
                item.Transaction_IDs = $"{string.Join(",", deposits.Select(e => e.TransactionId))},{string.Join(",", withdrawal.Select(e => e.Transaction_Id))}";

                await _dataContext.deposit_cashierteller_form.AddAsync(item);
                using (var _transaction = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {  
                        await _dataContext.SaveChangesAsync();
                        var approval_request = Build_approval_request(item, user.StaffId, user.CompanyId);

                        var result = await _serverRequest.GotForApprovalAsync(approval_request);
                        var stringData = await result.Content.ReadAsStringAsync();
                        var approval_response = JsonConvert.DeserializeObject<GoForApprovalRespObj>(stringData);

                        if (!result.IsSuccessStatusCode)
                        {
                            await _transaction.RollbackAsync();
                            response.Status.Message.FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}";
                            response.Status.Message.TechnicalMessage = result.ToString();
                            return response;
                        }

                        if (approval_response.ApprovalProcessStarted)
                        {
                            item.WorkflowToken = approval_response.Status.CustomToken;
                            item.Approval_status = (int)ApprovalStatus.Processing;
                            await _dataContext.SaveChangesAsync();
                            await _transaction.CommitAsync();
                            response.Status.IsSuccessful = true;
                            response.Status.Message.FriendlyMessage = "Transaction validation successfully sent for approval";
                            return response;
                        }

                        if (approval_response.EnableWorkflow || !approval_response.HasWorkflowAccess)
                        {
                            await _transaction.RollbackAsync();
                            response.Status.IsSuccessful = false;
                            response.Status.Message = approval_response.Status.Message;
                            return response;
                        }

                        if (!approval_response.EnableWorkflow)
                        { 
                            await _service.Perform_transaction_teller_and_balancing(item);
                            await _dataContext.SaveChangesAsync();
                            await _transaction.CommitAsync();
                            response.Status.IsSuccessful = true;
                            response.Status.Message.FriendlyMessage = "Transactions successfully validated";
                            return response;
                        }
                        response.Status.IsSuccessful = approval_response.Status.IsSuccessful;
                        response.Status.Message = approval_response.Status.Message;
                        return response;

                    }
                    catch (Exception e)
                    {
                        await _transaction.RollbackAsync();
                        response.Status.Message.FriendlyMessage = e?.Message ?? e?.InnerException?.Message;
                        response.Status.Message.TechnicalMessage = e.ToString();
                        return response;
                    }
                    finally { await _transaction.DisposeAsync(); }
                }
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
