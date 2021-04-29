using Deposit.Contracts.Response.Approvals;
using Deposit.Contracts.Response.Deposit;
using Deposit.DomainObjects;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.ChangeOfRate
{
    public class GetChangeOfRateAwaitingApprovalQuery : IRequest<ChangeOfRatesRespObj>
    {
      
        public class GetChangeOfRateAwaitingApprovalQueryHandler : IRequestHandler<GetChangeOfRateAwaitingApprovalQuery, ChangeOfRatesRespObj>
        { 
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IChangeOfRate _service;
            private readonly DataContext _dataContext;
            public GetChangeOfRateAwaitingApprovalQueryHandler(DataContext dataContext,

                IIdentityServerRequest identityServerRequest, IChangeOfRate changeOfRate)
            {

                _serverRequest = identityServerRequest;
                _service = changeOfRate;
                _dataContext = dataContext;
            }

            public async Task<ChangeOfRatesRespObj> Handle(GetChangeOfRateAwaitingApprovalQuery request, CancellationToken cancellationToken)
            {
                var response = new ChangeOfRatesRespObj
                { ChangeOfRates = new List<ChangeOfRatesObj>(), Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
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

                    if (res == null)
                    {
                        response.Status = res.Status;
                        return response;
                    }

                    if (res.workflowTasks.Count() < 1)
                    {
                        response.Status.Message.FriendlyMessage = "No Pending Approval";
                        return response; 
                    }

                    var pendingTaskIds = res.workflowTasks.Select(x => x.TargetId).ToList();
                    var pendingTaskTokens = res.workflowTasks.Select(s => s.WorkflowToken).ToList();
                    var deps = await _service.GetChangeOfRateAwaitingApprovalAsync(pendingTaskIds, pendingTaskTokens);
                    
                    response.ChangeOfRates = deps.Select(d => new ChangeOfRatesObj
                    {
                        ApprovalStatusId = d.ApprovalStatusId, 
                        ChangeOfRateId = d.ChangeOfRateId,  
                        Structure = d.Structure,
                        WorkflowToken = d.WorkflowToken,
                        CurrentRate = d.CurrentRate,
                        ProposedRate = d.ProposedRate,
                        Reasons = d.Reasons,
                        Product = d.Product
                    }).ToList(); 
                    if(response.ChangeOfRates.Count() > 0)
                    {
                        foreach(var item in response.ChangeOfRates)
                        {
                            item.ProductName = _dataContext.deposit_accountsetup.FirstOrDefault(e => e.DepositAccountId == item.Product)?.AccountName;
                        }
                    }
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = deps.Count() < 1 ? "No Item awaiting approvals" : null;
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
