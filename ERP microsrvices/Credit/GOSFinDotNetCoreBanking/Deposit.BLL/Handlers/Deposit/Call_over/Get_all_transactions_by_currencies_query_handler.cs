using Deposit.Contracts.Response.Deposit.Call_over;
using Deposit.Repository.Interface.Deposit;
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
    public class Get_all_transactions_by_currencies_query : IRequest<Transaction_response_by_currencies>
    {
        public int Structure_id { get; set; }
        public long Currency { get; set; }
        public class Get_all_transactions_by_currencies_query_handlerHandler : IRequestHandler<Get_all_transactions_by_currencies_query, Transaction_response_by_currencies>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly ICustomerService _customer;
            public Get_all_transactions_by_currencies_query_handlerHandler(DataContext dataContext, IIdentityServerRequest serverRequest, ICustomerService customer)
            {
                _customer = customer;
                _serverRequest = serverRequest;
                _dataContext = dataContext;
            }
            public async Task<Transaction_response_by_currencies> Handle(Get_all_transactions_by_currencies_query request, CancellationToken cancellationToken)
            {
                try
                {
                    var response = new Transaction_response_by_currencies();
                    var user_details = await _serverRequest.UserDataAsync();

                    var deposits = await _dataContext.deposit_form.Where(e => e.CreatedOn.Value.Date == DateTime.UtcNow.Date 
                    && e.Is_call_over_done == false && e.Currency == request.Currency
                    && e.Structure <= user_details.CompanyId).ToListAsync(); //request.Structure_id to be used when it's fxed from the front end

                    var withdrawal = await _dataContext.deposit_withdrawal_form.Where(e => e.CreatedOn.Value.Date == DateTime.UtcNow.Date 
                    && e.Is_call_over_done == false && e.Currency ==  request.Currency
                    && e.Structre <= user_details.CompanyId).ToListAsync(); //request.Structure_id to be used when it's fxed from the front end

                    var currencies = await _serverRequest.GetCurrencyAsync();
                    var company = await _serverRequest.GetAllCompanyAsync();

                    if (deposits.Any()) 
                        response.Transactions.AddRange(deposits.Select(e => new Transactions
                        {
                            Account_number = e.Account_number,
                            Amount = e.Deposit_amount,
                            CR_amount = e.Deposit_amount,
                            Currency = e.Currency,
                            DB_amount = 0,
                            Instruent_number = e.Instrument_number,
                            Transaction_Id = e.TransactionId 
                        }).ToList()); 

                    if (withdrawal.Any()) 
                        response.Transactions.AddRange(withdrawal.Select(e => new Transactions
                        {
                            Account_number = e.Account_number,
                            Amount = e.Amount,
                            CR_amount = 0,
                            Currency = e.Currency,
                            DB_amount = e.Amount,
                            Instruent_number = e.Withdrawal_instrument,
                            Transaction_Id = e.Transaction_Id 
                        }).ToList()); 

                    return await Task.Run(() => response);
                }
                catch (Exception e) { throw e; }
              
            }
        }
    }

}
