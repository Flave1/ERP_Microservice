using Deposit.Contracts.Response.Approvals;
using Deposit.Contracts.Response.Deposit; 
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.BankClosure
{
    public class GetBankClosureAwaitingApprovalQuery : IRequest<Deposit_BankClosureRespObj>
    {
      
        public class GetBankClosureAwaitingApprovalQueryHandler : IRequestHandler<GetBankClosureAwaitingApprovalQuery, Deposit_BankClosureRespObj>
        { 
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IDepositBankClosure _deposit;
            private readonly ICustomerService _service;
            public GetBankClosureAwaitingApprovalQueryHandler(

                IIdentityServerRequest identityServerRequest, IDepositBankClosure depositBankClosure, ICustomerService   customer)
            {

                _serverRequest = identityServerRequest;
                _deposit = depositBankClosure;
                _service = customer;
            }

            public async Task<Deposit_BankClosureRespObj> Handle(GetBankClosureAwaitingApprovalQuery request, CancellationToken cancellationToken)
            {
                var response = new Deposit_BankClosureRespObj
                { BankClosures = new List<Deposit_bankClosureObj>(), Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                try
                {
                    
                    var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                    if (!result.IsSuccessStatusCode)
                    {
                        var data1 = await result.Content.ReadAsStringAsync();
                        var res1 = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data1); 
                        response.Status.Message.FriendlyMessage = $"{result.ReasonPhrase} {result.StatusCode}";
                        return response;
                    }

                    var data = await result.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<WorkflowTaskRespObj>(data);
 

                    if (res.workflowTasks.Count() < 1)
                    {
                        response.Status.Message.FriendlyMessage = "No Pending Approval";
                        return response; 
                    }

                    var pendingTaskIds = res.workflowTasks.Select(x => x.TargetId).ToList();
                    var pendingTaskTokens = res.workflowTasks.Select(s => s.WorkflowToken).ToList();
                     

                    var closures = await _deposit.GetBankAccountClosureAwaitingApprovalAsync(pendingTaskIds, pendingTaskTokens);

                    response.BankClosures = closures.Select(d => new Deposit_bankClosureObj
                    {
                        ApprovalStatusId = d.ApprovalStatusId, 
                        BankClosureId = d.BankClosureId,
                        AccountBalance = d.AccountBalance,
                        AccountName = _service.Return_customer_name(d.AccountId),
                        AccountNumber = d.AccountNumber,
                        Beneficiary = d.Beneficiary,
                        Charges = d.Charges,
                        ClosingDate = d.ClosingDate,
                        FinalSettlement = d.FinalSettlement,
                        Reason = d.Reason,
                        ModeOfSettlement = d.ModeOfSettlement, 
                        Structure = d.Structure,
                        WorkflowToken = d.WorkflowToken,
                        Status_name = Convert.ToString((ApprovalStatus)d.ApprovalStatusId) 
                    }).ToList();

                    if(response.BankClosures.Count() > 0)
                    {
                        foreach(var item in response.BankClosures)
                        {
                            switch (item.ModeOfSettlement)
                            {
                                case 1:
                                    item.ModeOfSettlementName = "Bank Transfer";
                                    break;
                                case 2:
                                    item.ModeOfSettlementName = "Cheque Issue";
                                    break;
                                case 3:
                                    item.ModeOfSettlementName = "Cash Payment";
                                    break; 
                            }
                        }
                    }
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = closures.Count() < 1 ? "No Item awaiting approvals" : null;
                    return response;

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }
}
