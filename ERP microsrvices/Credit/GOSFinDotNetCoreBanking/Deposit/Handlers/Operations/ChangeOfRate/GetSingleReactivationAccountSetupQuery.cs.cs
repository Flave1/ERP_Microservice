using AutoMapper;
using Deposit.Contracts.Response.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.BankClosure
{
    public class GetSingleChangeOfRateQuery : IRequest<ChangeOfRatesRespObj>
    {
        public int ChangeOfRateId { get; set; }
        public class GetSingleChangeOfRateQueryHandler : IRequestHandler<GetSingleChangeOfRateQuery, ChangeOfRatesRespObj>
        {
            private readonly DataContext _dataContext;

            private readonly IMapper _mapper;
            private readonly IIdentityServerRequest _serverRequest;
            public GetSingleChangeOfRateQueryHandler(DataContext dataContext, IMapper mapper, IIdentityServerRequest serverRequest)
            {
                _dataContext = dataContext;
                _serverRequest = serverRequest;
                _mapper = mapper;
            }
            public async Task<ChangeOfRatesRespObj> Handle(GetSingleChangeOfRateQuery request, CancellationToken cancellationToken)
            {
                var response = new ChangeOfRatesRespObj
                {
                    ChangeOfRates = new List<ChangeOfRatesObj>(),
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() }
                };

                var itemList = _dataContext.deposit_changeofrates.Where(d => d.Deleted == false && d.ChangeOfRateId == request.ChangeOfRateId).ToList();
                if (itemList.Count() > 0)
                {
                    response.ChangeOfRates = _mapper.Map<List<ChangeOfRatesObj>>(itemList);
                    //foreach (var item in response.ChangeOfRates)
                    //{
                    //    item.CustomerTypeName = item.CustomerTypeId == (int)CustomerType.Corporate ? "Corportate" : "Individual";
                    //    item.Name = item.CustomerTypeId == (int)CustomerType.Corporate ? itemList.FirstOrDefault(e => e.CustomerId == item.CustomerId)?.CompanyName : $"{itemList.FirstOrDefault(e => e.CustomerId == item.CustomerId)?.Firstname} {itemList.FirstOrDefault(e => e.CustomerId == item.CustomerId)?.Surname}";
                    //}  
                }
                response.Status.Message.FriendlyMessage = response.ChangeOfRates.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() => response);
            }
        }
    }
}
