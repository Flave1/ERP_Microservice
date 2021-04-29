using AutoMapper;
using Deposit.Contracts.Response.Deposit;

using Deposit.Requests;
using Deposit.Data;
using GODP.Entities.Models;
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
    public class GetSingleCustomerDetailsQuery : IRequest<GetDepositCustomerDetailsResp>
    {
        public int CustomerId { get; set; }
        public class GetSingleCustomerDetailsQueryHandler : IRequestHandler<GetSingleCustomerDetailsQuery, GetDepositCustomerDetailsResp>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IMapper _mapper; 

            public GetSingleCustomerDetailsQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, IMapper mapper)
            {
                _mapper = mapper;
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<GetDepositCustomerDetailsResp> Handle(GetSingleCustomerDetailsQuery request, CancellationToken cancellationToken)
            {
                var response = new GetDepositCustomerDetailsResp { Status = new APIResponseStatus { Message = new APIResponseMessage() } };


                var personnalDeta = _dataContext.deposit_accountopening.Where(d => d.CustomerId == request.CustomerId && d.Deleted == false).ToList();
                if (personnalDeta.Count() > 0)
                {
                    response.CustomerAccountDetails = _mapper.Map<List<CustomerAccountDetails>>(personnalDeta);
                }


                var directors = _dataContext.deposit_directors.Where(d => d.CustomerId == request.CustomerId && d.Deleted == false).ToList();
                if (directors.Count() > 0)
                {
                    response.Directors = _mapper.Map<List<Directors>>(directors);
                }


                var identifications = _dataContext.deposit_customerIdentification.Where(d => d.CustomerId == request.CustomerId && d.Deleted == false).ToList();
                if (identifications.Count() > 0)
                {
                    response.Identification = _mapper.Map<List<IdentificationObj>>(identifications);
                }

                //var keyContacts = _dataContext.deposit_keycontactpersons.Where(d => d.CustomerId == request.CustomerId && d.Deleted == false).ToList();
                //if (keyContacts.Count() > 0)
                //{
                //    response.KeyContactPersons = _mapper.Map<List<Contracts.Response.Deposit.AccountOpening.KeyContactPersons>>(keyContacts);
                //}

                var kyc = _dataContext.deposit_kyc.Where(d => d.CustomerId == request.CustomerId && d.Deleted == false).ToList();
                if (kyc.Count() > 0)
                {
                    response.KYC = _mapper.Map<List<KYC>>(kyc);
                }

                //var signatory = _dataContext.deposit_signatories.Where(d => d.CustomerId == request.CustomerId && d.Deleted == false).ToList();
                //if (signatory.Count() > 0)
                //{
                //    response.Signatory = _mapper.Map<List<Signatory>>(signatory);
                //}

                var nextOfKin = _dataContext.deposit_nextofkin.Where(d => d.CustomerId == request.CustomerId && d.Deleted == false).ToList();
                if (nextOfKin.Count() > 0)
                {
                    response.NextOfKins = _mapper.Map<List<NextOfKin>>(nextOfKin);
                }

                var actInfor = _dataContext.deposit_customer_accountdetails.Where(d => d.CustomerId == request.CustomerId && d.Deleted == false).ToList();
                if (actInfor.Count() > 0)
                { 
                    response.AccountInformations = _mapper.Map<List<AccountInformationObj>>(actInfor);
                    foreach(var item in response.AccountInformations)
                    {
                        item.CurrencyArray = item.Currencies.Split(",").Select(int.Parse).ToArray();
                    }
                }

                response.Status.Message.FriendlyMessage = response.CustomerAccountDetails.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() => response);
            }
        }
    }

}
