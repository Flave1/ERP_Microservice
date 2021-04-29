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
    public class GetDepositCustomerLiteQuery : IRequest<PersonalInformationResp>
    {
        public class GetDepositCustomerLiteQueryHandler : IRequestHandler<GetDepositCustomerLiteQuery, PersonalInformationResp>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IMapper _mapper;
            private readonly ICustomerService _customer;

            public GetDepositCustomerLiteQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, IMapper mapper, ICustomerService customer)
            {
                _customer = customer;
                _mapper = mapper;
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<PersonalInformationResp> Handle(GetDepositCustomerLiteQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var response = new PersonalInformationResp { CustomerLiteAccountDetails = new List<CustomerLiteAccountDetails>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                     

                    response.CustomerLiteAccountDetails = (from a in _dataContext.deposit_customer_accountdetails
                                                           join b in _dataContext.deposit_accountopening on a.CustomerId equals b.CustomerId
                                                           where a.Deleted == false
                                                           select new CustomerLiteAccountDetails
                                                           {
                                                               AccountNumber = a.AccountNumber,
                                                               CustomerId = a.CustomerId,
                                                               Email = b.Email,
                                                               Currencies = _customer.Return_this_account_operating_currencies(a.Currencies??"00000"),
                                                               AvailableBalance = a.AvailableBalance,
                                                               CustomerTypeId = b.CustomerTypeId,
                                                               Status = DateTime.UtcNow.Date > a.Date_to_go_dormant ? "Dormant" : "Active",
                                                               Name = _customer.Return_customer_name(a.CustomerId),
                                                               CustomerTypeName = b.CustomerTypeId == (int)CustomerType.Corporate ? "Corportate" : "Individual",
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