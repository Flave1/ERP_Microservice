using Deposit.Contracts.Response.Approvals;
using Deposit.Contracts.Response.Deposit.Operation;
using Deposit.DomainObjects.Deposit;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.Reactivation
{
    public class GetReactivationAwaitingApprovalQuery : IRequest<Reactivated_customers_response>
    {
      
        public class GetReactivationAwaitingApprovalQueryHandler : IRequestHandler<GetReactivationAwaitingApprovalQuery, Reactivated_customers_response>
        { 
            private readonly IIdentityServerRequest _serverRequest;
            private readonly ICustomerService _service;
            private readonly DataContext _dataContext;
            public GetReactivationAwaitingApprovalQueryHandler(DataContext dataContext,

                IIdentityServerRequest identityServerRequest, ICustomerService Reactivation)
            {

                _serverRequest = identityServerRequest;
                _service = Reactivation;
                _dataContext = dataContext;
            }

            public async Task<IEnumerable<deposit_reactivation_form>> Get_reactivated_account_awaiting_approval_Async(List<long> targetIds, List<string> tokens)
            {
                var item = await _dataContext.deposit_reactivation_form
                    .Where(s => targetIds.Contains(s.Id)
                    && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
                return item;
            }
            public async Task<Reactivated_customers_response> Handle(GetReactivationAwaitingApprovalQuery request, CancellationToken cancellationToken)
            {
                var response = new Reactivated_customers_response();
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
                    var reactivations = await Get_reactivated_account_awaiting_approval_Async(pendingTaskIds, pendingTaskTokens);

                    response.Reactivated_customers = (from a in reactivations
                                                      join b in _dataContext.deposit_customer_accountdetails on a.CustomerId equals b.CustomerId
                                                      select new Reactivated_customers 
                                                      {
                                                            AccountBalance = b.AvailableBalance,
                                                            Account_name = _service.Return_customer_name(a.CustomerId),
                                                            Account_number = b.AccountNumber,
                                                            Charges = a.Charges,
                                                            Currency = b.Currencies.Split(",").ToList().Select(int.Parse).ToArray(),
                                                            Id = a.Id, 
                                                            Reactivation_reason = a.Reactivation_reason, 
                                                            
                                                      }).ToList(); 
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = response.Reactivated_customers.Count() < 1 ? "No Item awaiting approvals" : null;
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
