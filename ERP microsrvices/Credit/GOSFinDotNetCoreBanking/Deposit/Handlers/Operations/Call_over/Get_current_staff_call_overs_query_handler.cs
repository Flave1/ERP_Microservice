using Deposit.Contracts.Response.Deposit.Call_over;
using Deposit.Requests;
using Deposit.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.Call_over
{
    public class Get_current_staff_call_overs_query : IRequest<cashierteller_call_over>
    {
        public int Structure_id { get; set; }
        public class Get_current_staff_call_overs_query_handlerHandler : IRequestHandler<Get_current_staff_call_overs_query, cashierteller_call_over>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            public Get_current_staff_call_overs_query_handlerHandler(DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _serverRequest = serverRequest;
                _dataContext = dataContext;
            }
            public async Task<cashierteller_call_over> Handle(Get_current_staff_call_overs_query request, CancellationToken cancellationToken)
            {
                try
                {
                    var response = new cashierteller_call_over();


                    var user_details = await _serverRequest.UserDataAsync();
                    var currencies = await _serverRequest.GetCurrencyAsync();

                    var call_over_detail_setup = await _dataContext.deposit_cashiertellersetup.Where(e => e.Employee_ID == user_details.StaffId).ToListAsync();
                    if (call_over_detail_setup.Count() == 0)
                        return new cashierteller_call_over();

                    var company = await _serverRequest.GetAllCompanyAsync();

                    var deposits = await _dataContext.deposit_form.Where(e => e.CreatedOn.Value.Date == DateTime.UtcNow.Date 
                    && e.Is_call_over_done == false 
                    && e.Structure <= user_details.CompanyId).ToListAsync(); //request.Structure_id to be used when it's fxed from the front end

                    var withdrawal = await _dataContext.deposit_withdrawal_form.Where(e => e.CreatedOn.Value.Date == DateTime.UtcNow.Date 
                    && e.Is_call_over_done == false
                    && e.Structre <= user_details.CompanyId).ToListAsync(); //request.Structure_id to be used when it's fxed from the front end

                    var this_staff_currency_and_amount_list = await _dataContext.deposit_call_over_currecies_and_amount
                        .Where(e => e.Call_over_date.Date == DateTime.UtcNow.Date
                        && Convert.ToInt32(e.User_id) == user_details.StaffId).ToListAsync();


                    response.SubStructure = call_over_detail_setup.FirstOrDefault().Sub_strructure ?? string.Empty;
                    response.Date = DateTime.UtcNow;
                    response.Employee_ID = user_details.StaffId;
                    response.Staff_name = $"{user_details.StaffName}";
                    response.Structure_name = company.companyStructures.FirstOrDefault(e => e.companyStructureId == user_details.CompanyId)?.name;
                    response.Structure = user_details.CompanyId;

                    response.Currencie_and_amount = this_staff_currency_and_amount_list.Select(ob => new call_over_currecies_and_amount
                    {
                        Opening_bal = ob.Amount,
                        Cr_amount = deposits.Where(e => e.Currency == ob.Currency).Sum(r => r.Deposit_amount),
                        Currency = ob.Currency,
                        Currency_name = currencies.commonLookups.FirstOrDefault(r => r.LookupId == ob.Currency)?.LookupName,
                        Dr_amount = withdrawal.Where(e => e.Currency == ob.Currency).Sum(r => r.Amount),
                        Closing_bal = ob.Amount + deposits.Where(e => e.Currency == ob.Currency).Sum(r => r.Deposit_amount) + withdrawal.Where(e => e.Currency == ob.Currency).Sum(r => r.Amount)
                    }).ToList();

                    return await Task.Run(() => response);
                }
                catch (Exception e) { throw e; } 
            }
        }
    }

}
