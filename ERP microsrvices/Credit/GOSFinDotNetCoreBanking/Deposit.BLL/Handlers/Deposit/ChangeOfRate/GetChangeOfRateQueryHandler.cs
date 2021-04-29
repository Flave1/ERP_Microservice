using AutoMapper;
using Deposit.Contracts.Response.Deposit;

using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.ChangeOfRate
{
    public class GetAllChangeOfRateQuery : IRequest<ChangeOfRatesRespObj>
    {
        public class GetAllChangeOfRateQueryHandler : IRequestHandler<GetAllChangeOfRateQuery, ChangeOfRatesRespObj>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IMapper _mapper;
            
            public GetAllChangeOfRateQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, IMapper mapper)
            {
                _mapper = mapper;
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<ChangeOfRatesRespObj> Handle(GetAllChangeOfRateQuery request, CancellationToken cancellationToken)
            {
                var response = new ChangeOfRatesRespObj { ChangeOfRates = new List<ChangeOfRatesObj>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                 
                var itemList = _dataContext.deposit_changeofrates.Where(d => d.Deleted == false).ToList(); 

                if (itemList.Count() > 0)
                {
                    response.ChangeOfRates = _mapper.Map<List<ChangeOfRatesObj>>(itemList);
                    foreach (var item in response.ChangeOfRates)
                    {
                        item.ProductName = _dataContext.deposit_accountsetup.FirstOrDefault(e => e.DepositAccountId == item.Product)?.AccountName;
                        item.StatusName = Convert.ToString((ApprovalStatus)item.ApprovalStatusId);
                    }
                }

                response.Status.Message.FriendlyMessage = response.ChangeOfRates.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() =>  response);
            }
        }
    }
    
}
