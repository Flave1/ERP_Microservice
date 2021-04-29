using AutoMapper;
using Deposit.Contracts.Response.Deposit.Operation;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.Reactivation
{

    public class Get_all_reactivated_accountsQuery : IRequest<Reactivated_customers_response>
    {
        public class Get_all_reactivated_accountsQueryHandler : IRequestHandler<Get_all_reactivated_accountsQuery, Reactivated_customers_response>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly ICustomerService _service;

            public Get_all_reactivated_accountsQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, ICustomerService service)
            {
                _service = service;
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<Reactivated_customers_response> Handle(Get_all_reactivated_accountsQuery request, CancellationToken cancellationToken)
            {
                var response = new Reactivated_customers_response();

                response.Reactivated_customers = (from a in _dataContext.deposit_reactivation_form
                                                  join b in _dataContext.deposit_customer_accountdetails on a.CustomerId equals b.CustomerId
                                                  select new Reactivated_customers
                                                  {
                                                      AccountBalance = b.AvailableBalance,
                                                      Account_name = _service.Return_customer_name(a.CustomerId),
                                                      Account_number = b.AccountNumber,
                                                      Charges = a.Charges,
                                                      //Currency = b.Currencies.Split(",").ToList().Select(int.Parse).ToArray(),
                                                      Id = a.Id,
                                                      Reactivation_reason = a.Reactivation_reason,
                                                      ApprovalStatus = a.ApprovalStatusId,
                                                      Approval_status_name = Convert.ToString((ApprovalStatus)a.ApprovalStatusId)
                                                  }).ToList();
                response.Status.IsSuccessful = true;
                response.Status.Message.FriendlyMessage = response.Reactivated_customers.Count() < 1 ? "No Item awaiting approvals" : null;
                return response; 
            }
        }
    }
}
