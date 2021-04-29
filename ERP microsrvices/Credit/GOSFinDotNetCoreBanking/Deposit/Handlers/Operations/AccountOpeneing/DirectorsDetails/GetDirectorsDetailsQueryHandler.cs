using AutoMapper;

using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.Response.Deposit.AccountOpening;

namespace Deposit.Handlers.Deposit.DirectorsDetails
{
    public class GetAllDirectorsDetailsQuery : IRequest<DirectorsDetailsResp>
    {
        public int customerId { get; set; }
        public class GetAllDirectorsDetailsQueryHandler : IRequestHandler<GetAllDirectorsDetailsQuery, DirectorsDetailsResp>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IMapper _mapper;
            
            public GetAllDirectorsDetailsQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, IMapper mapper)
            {
                _mapper = mapper;
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<DirectorsDetailsResp> Handle(GetAllDirectorsDetailsQuery request, CancellationToken cancellationToken)
            {
                var response = new DirectorsDetailsResp { Directors = new List<Directors>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                var idents = await _serverRequest.GetIdentiticationTypeAsync(); 
                var itemList = _dataContext.deposit_directors.Where(d => d.Deleted == false && d.CustomerId == request.customerId).ToList(); 

                if (itemList.Count() > 0)
                {
                    response.Directors = _mapper.Map<List<Directors>>(itemList);
                } 
                response.Status.Message.FriendlyMessage = response.Directors.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() =>  response);
            }
        }
    }
    
}
