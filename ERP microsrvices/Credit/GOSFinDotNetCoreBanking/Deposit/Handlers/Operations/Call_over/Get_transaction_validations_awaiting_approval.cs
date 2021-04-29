using Deposit.Contracts.Response.Approvals;
using Deposit.Contracts.Response.Deposit.Call_over;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GODP.Entities.Models;
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
    public class Get_transaction_validations_awaiting_Query : IRequest<cashierteller_call_over_response>
    {
      
        public class Get_transaction_validations_awaiting_QueryHandler : IRequestHandler<Get_transaction_validations_awaiting_Query, cashierteller_call_over_response>
        { 
            private readonly IIdentityServerRequest _serverRequest;
            private readonly ICustomerService _service;
            private readonly DataContext _dataContext;
            public Get_transaction_validations_awaiting_QueryHandler(DataContext dataContext,

                IIdentityServerRequest identityServerRequest, ICustomerService Reactivation)
            {

                _serverRequest = identityServerRequest;
                _service = Reactivation;
                _dataContext = dataContext;
            }

            public async Task<List<deposit_cashierteller_form>> Get_transaction_validations_awaiting_approval_Async(List<long> targetIds, List<string> tokens)
            {
                var item = await _dataContext.deposit_cashierteller_form
                    .Where(s => targetIds.Contains(s.Id)
                    && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
                return item;
            }
            public async Task<cashierteller_call_over_response> Handle(Get_transaction_validations_awaiting_Query request, CancellationToken cancellationToken)
            {
                var response = new cashierteller_call_over_response();
                var call_over_resp_list = new List<cashierteller_call_over>();
                var call_over_resp = new cashierteller_call_over();

                try
                { 
                    var result = await _serverRequest.GetAnApproverItemsFromIdentityServer();
                    var staff_repsonse = await _serverRequest.GetAllStaffAsync();
                    var company = await _serverRequest.GetAllCompanyAsync();
                    var currencies = await _serverRequest.GetCurrencyAsync();

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
                    var transaction_validations = await Get_transaction_validations_awaiting_approval_Async(pendingTaskIds, pendingTaskTokens);

                    var this_transactions_initiating_staff = transaction_validations.Select(e => e.Employee_ID).ToList();

                    var currency_and_amount_list = await _dataContext.deposit_call_over_currecies_and_amount
                       .Where(e => e.Call_over_date.Date == DateTime.UtcNow.Date 
                       && this_transactions_initiating_staff.Contains(Convert.ToInt32(e.User_id))).ToListAsync();
                     
                    if (transaction_validations.Any())
                    {
                        foreach (var trans_val in transaction_validations)
                        {
                            var deposits = await _dataContext.deposit_form.Where(e => e.CreatedOn.Value.Date == DateTime.UtcNow.Date
                            && e.Is_call_over_done == false
                            && e.Structure <= trans_val.Structure).ToListAsync();

                            var withdrawal = await _dataContext.deposit_withdrawal_form.Where(e => e.CreatedOn.Value.Date == DateTime.UtcNow.Date
                            && e.Is_call_over_done == false
                            && e.Structre <= trans_val.Structure).ToListAsync();

                            call_over_resp.SubStructure = trans_val.SubStructure ?? string.Empty;
                            call_over_resp.Date = trans_val.Date;
                            call_over_resp.Employee_ID = trans_val.Employee_ID;
                            call_over_resp.Staff_name = 
                                $"{staff_repsonse.staff.FirstOrDefault(e => e.staffId == trans_val.Employee_ID)?.firstName} " +
                                $"{staff_repsonse.staff.FirstOrDefault(e => e.staffId == trans_val.Employee_ID)?.lastName}";
                            call_over_resp.Structure_name = company.companyStructures.FirstOrDefault(e => e.companyStructureId == trans_val.Structure)?.name;
                            call_over_resp.Structure = trans_val.Structure;
                            call_over_resp.Id = trans_val.Id;

                            call_over_resp.Currencie_and_amount = currency_and_amount_list.Select(ob => new call_over_currecies_and_amount
                            {
                                Opening_bal = ob.Amount,
                                Cr_amount = deposits.Where(e => e.Currency == ob.Currency).Sum(r => r.Deposit_amount),
                                Currency = ob.Currency,
                                Currency_name = currencies.commonLookups.FirstOrDefault(r => r.LookupId == ob.Currency)?.LookupName,
                                Dr_amount = withdrawal.Where(e => e.Currency == ob.Currency).Sum(r => r.Amount),
                                Closing_bal = 
                                    ob.Amount + deposits.Where(e => e.Currency == ob.Currency).Sum(r => r.Deposit_amount) + 
                                    withdrawal.Where(e => e.Currency == ob.Currency).Sum(r => r.Amount)
                            }).ToList();
                             
                            response.Transaction_validations.Add(call_over_resp);
                        }
                    }
                     
                    return await Task.Run(() => response);

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
    }
}
