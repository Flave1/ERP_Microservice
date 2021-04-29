using Deposit.Contracts.Response.Approvals;
using Deposit.Contracts.Response.Deposit.Deposit_form;
using Deposit.Contracts.Response.Deposit.Operation;
using Deposit.DomainObjects.Deposit;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.Enums;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.Response.Deposit.Operation;

namespace Deposit.Handlers.Deposit.Reactivation
{

    public class Reactivate_Customer_commandHandler : IRequestHandler<Reactivate_Customer_command, Account_response>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly ICustomerService _customer;
        private readonly IIdentityServerRequest _serverRequest; 

        public Reactivate_Customer_commandHandler(ILoggerService logger, DataContext dataContext, IIdentityServerRequest request, ICustomerService customer)
        {
            _dataContext = dataContext;
            _logger = logger;
            _customer = customer;
            _serverRequest = request;
        }

     

        public deposit_reactivation_form Build_DB_object(Reactivate_Customer_command request, deposit_reactivation_form db_item)
        {

            db_item.Id = request.Id;
            db_item.Account_number = request.Account_number;
            db_item.CustomerId= request.CustomerId; 
            db_item.Product = request.Product;
            db_item.Reactivation_reason = request.Reactivation_reason;
            db_item.Charges = _customer.Return_reactivation_charges_if_applicable(request.Product);
            db_item.Substructure = request.Substructure;
            db_item.Structure = request.Structure; 
            return db_item;
        }

      

        public GoForApprovalRequest Build_approval_request(deposit_reactivation_form item, int staff)
        {
            
            var targetList = new List<long>();
            targetList.Add(item.Id);
            return  new GoForApprovalRequest
            {
                Comment = "Account Re-activation",
                OperationId = (int)OperationsEnum.ChangeOfRate,
                TargetId = targetList,
                ApprovalStatus = (int)ApprovalStatus.Processing,
                DeferredExecution = true,
                StaffId = staff,
                CompanyId = item.CompanyId,
                EmailNotification = false,
                ExternalInitialization = false,
                StatusId = (int)ApprovalStatus.Processing,
            };
        }

     
        public async Task<Account_response> Handle(Reactivate_Customer_command request, CancellationToken cancellationToken)
        {
            var response = new Account_response();
            try
            {

                var item = _dataContext.deposit_reactivation_form.Find(request.Id);
                if (item == null) item = new deposit_reactivation_form();
                item = Build_DB_object(request, item);
                if (item.Id < 1) _dataContext.deposit_reactivation_form.Add(item);
                
                using (var _transaction = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var user = _serverRequest.UserDataAsync().Result;
                        request.Substructure = user.CompanyId;
                        request.Structure = user.CompanyId;
                        item.CompanyId = user.CompanyId;
                        _customer.Reactivate_customer_account(item, request.Currency);
                        await _dataContext.SaveChangesAsync();
                        var approval_request = Build_approval_request(item, user.StaffId);

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
                            item.ApprovalStatusId = (int)ApprovalStatus.Processing;
                            await _dataContext.SaveChangesAsync();
                            await _transaction.CommitAsync(); 
                            response.Status.IsSuccessful = true;
                            response.Status.Message.FriendlyMessage = "Account successfully sent for approval";
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
                            
                            item.ApprovalStatusId = (int)ApprovalStatus.Approved; 
                            _customer.Reactivate_customer_account(item, request.Currency);
                            await _dataContext.SaveChangesAsync();
                            await _transaction.CommitAsync();
                            response.Status.IsSuccessful = true;
                            response.Status.Message.FriendlyMessage = "Account successfully reactivated";
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
                response.Status.Message.FriendlyMessage = e?.Message ?? e.InnerException?.Message;
                response.Status.Message.TechnicalMessage = e.ToString();
                _logger.Error(e.ToString());
                return response;
            }
        }
    }
}
