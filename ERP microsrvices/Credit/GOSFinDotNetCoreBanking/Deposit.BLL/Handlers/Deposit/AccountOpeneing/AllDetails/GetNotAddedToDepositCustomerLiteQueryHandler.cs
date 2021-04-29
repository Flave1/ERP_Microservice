using AutoMapper;
using Deposit.Contracts.Response.Deposit;

using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.Response.Deposit.AccountOpening;

namespace Deposit.Handlers.Deposit.AccountSetup
{
    public class GetNotAddedToDepositCustomerLiteQuery : IRequest<PersonalInformationResp>
    { 
        public class GetNotAddedToDepositCustomerLiteQueryHandler : IRequestHandler<GetNotAddedToDepositCustomerLiteQuery, PersonalInformationResp>
        {
            private readonly DataContext _dataContext; 
            private readonly ICustomerService _service;

            public GetNotAddedToDepositCustomerLiteQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, ICustomerService service, IMapper mapper)
            {  
                _dataContext = dataContext;
                _service = service;
            }
         
            public async Task<PersonalInformationResp> Handle(GetNotAddedToDepositCustomerLiteQuery request, CancellationToken cancellationToken)
            {
                var response = new PersonalInformationResp { CustomerLiteAccountDetails = new List<CustomerLiteAccountDetails>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                try
                {
                    response.CustomerLiteAccountDetails = (from a in _dataContext.deposit_customer_accountdetails 
                                                           join b in _dataContext.deposit_accountopening on a.CustomerId equals b.CustomerId
                                                           where a.Deleted == false
                                                           select new CustomerLiteAccountDetails
                                                           {
                                                               Name = _service.Return_customer_name(b.CustomerId),
                                                               CustomerId = b.CustomerId, 
                                                               CustomerTypeId = b.CustomerTypeId,
                                                               AccountNumber = a.AccountNumber,
                                                               Currencies = _service.Return_this_account_operating_currencies(a.Currencies ?? "00000"),
                                                               AvailableBalance = a.AvailableBalance,
                                                               Charges = _service.Return_bank_closure_charges_if_applicable(a.AvailableBalance, a.AccountTypeId),
                                                               Status = DateTime.UtcNow.Date > a.Date_to_go_dormant.Date ? "Domant" : "Active",
                                                               CustomerTypeName = Convert.ToString((CustomerType)b.CustomerTypeId),
                                                               Product = _dataContext.deposit_accountsetup.FirstOrDefault(e => e.AccountTypeId == a.AccountTypeId).DepositAccountId
                                                           }).ToList();

                    response.Status.Message.FriendlyMessage = response.CustomerLiteAccountDetails.Count() > 0 ? "" : "Search Complete!! No Record Found";
                    return await Task.Run(() => response);
                }
                catch (Exception e)
                {

                    throw e;
                }
               
            }
        }
    }

}
