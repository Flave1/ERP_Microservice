using AutoMapper;
using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.AccountSetup
{
    public class GetSingleAccountSetupQuery : IRequest<AccountSetupRespObj>
    {
        public int AccountSetupId { get; set; }
        
        public class GetSingleAccountSetupQueryHandler : IRequestHandler<GetSingleAccountSetupQuery, AccountSetupRespObj>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public GetSingleAccountSetupQueryHandler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }
            public async Task<AccountSetupRespObj> Handle(GetSingleAccountSetupQuery request, CancellationToken cancellationToken)
            {
                var response = new AccountSetupRespObj
                {
                    DepositAccounts = new List<DepositAccountObj>(),
                    Status = new APIResponseStatus { Message = new APIResponseMessage(), IsSuccessful = true, }
                };
                var setup = await _dataContext.deposit_accountsetup.FindAsync(request.AccountSetupId);

                if(setup != null)
                {
                    var item = _mapper.Map<DepositAccountObj>(setup);
                    response.DepositAccounts.Add(item);
                }
                response.Status.Message.FriendlyMessage = response.DepositAccounts.Count() > 0 ? "" : "Search Complete!! Record Not Found";
                return response;
            }
        }
    }
  
}
