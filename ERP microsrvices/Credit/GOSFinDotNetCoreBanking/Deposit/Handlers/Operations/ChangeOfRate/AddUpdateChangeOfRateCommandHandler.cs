using Deposit.Contracts.Response.Approvals;
using Deposit.Contracts.Response.Deposit;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GODP.Entities.Models;
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

namespace Deposit.Handlers.PersonalInformations
{
    public class AddChangeOfRateCommandHandler : IRequestHandler<AddChangeOfRateCommand, ChangeOfRatesRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IChangeOfRate _service;
        private readonly IIdentityServerRequest _serverRequest;
        public AddChangeOfRateCommandHandler(ILoggerService logger, DataContext dataContext, IChangeOfRate changeOfRate, IIdentityServerRequest serverRequest)
        {
            _serverRequest = serverRequest;
            _service = changeOfRate;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<ChangeOfRatesRegRespObj> Handle(AddChangeOfRateCommand request, CancellationToken cancellationToken)
        {
            var response = new ChangeOfRatesRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var domain = _dataContext.deposit_changeofrates.Find(request.ChangeOfRateId);
                if (domain == null)
                    domain = new deposit_changeofrates();

                var user = await _serverRequest.UserDataAsync();
                domain.Structure = user.CompanyId;
                domain.Product = request.Product;
                domain.CurrentRate = request.CurrentRate;
                domain.ProposedRate = request.ProposedRate;
                domain.Reasons = request.Reasons; 

                using (var _transaction = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _service.AddUpdateChangeOfRate(domain);
                        var targetList = new List<long>();
                        targetList.Add(domain.ChangeOfRateId);
                        GoForApprovalRequest wfRequest = new GoForApprovalRequest
                        {
                            Comment = "Change of rate",
                            OperationId = (int)OperationsEnum.ChangeOfRate,
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
                            domain.WorkflowToken = res.Status.CustomToken;
                            domain.ApprovalStatusId = (int)ApprovalStatus.Processing;
                            await _service.AddUpdateChangeOfRate(domain);
                            await _transaction.CommitAsync();

                            response.Status.IsSuccessful = res.Status.IsSuccessful;
                            response.Status.Message = res.Status.Message;
                            return response;
                        }

                        if (res.EnableWorkflow || !res.HasWorkflowAccess)
                        {
                            await _transaction.RollbackAsync();
                            response.Status.IsSuccessful = false;
                            response.Status.Message = res.Status.Message;
                            return response;
                        }
                        if (!res.EnableWorkflow)
                        {
                            domain.ApprovalStatusId = (int)ApprovalStatus.Approved;
                            await _service.AddUpdateChangeOfRate(domain);
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
