using AutoMapper;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks; 

namespace Deposit.Handlers.Deposit.AccountInformation
{
    public class GetAllAccountInformationQuery : IRequest<AccountInformationResp>
    {
        public class GetAllAccountInformationQueryHandler : IRequestHandler<GetAllAccountInformationQuery, AccountInformationResp>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IMapper _mapper;
            
            public GetAllAccountInformationQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, IMapper mapper)
            {
                _mapper = mapper;
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<AccountInformationResp> Handle(GetAllAccountInformationQuery request, CancellationToken cancellationToken)
            {
                var response = new AccountInformationResp { AccountInformations = new List<AccountInformationObj>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                 
                var itemList = _dataContext.deposit_customer_accountdetails.Where(d => d.Deleted == false).ToList(); 

                if (itemList.Count() > 0)
                {
                    response.AccountInformations = _mapper.Map<List<AccountInformationObj>>(itemList);
                }

                response.Status.Message.FriendlyMessage = response.AccountInformations.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() =>  response);
            }
        }
    }
    
}
