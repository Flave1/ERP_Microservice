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

namespace Deposit.Handlers.Deposit.Identification
{
    public class GetAllIdentificationQuery : IRequest<IdentificationResp>
    {
        public int customerId { get; set; }
        public class GetAllIdentificationQueryHandler : IRequestHandler<GetAllIdentificationQuery, IdentificationResp>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IMapper _mapper;
            
            public GetAllIdentificationQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, IMapper mapper)
            {
                _mapper = mapper;
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<IdentificationResp> Handle(GetAllIdentificationQuery request, CancellationToken cancellationToken)
            {
                var response = new IdentificationResp { Identification = new List<IdentificationObj>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                var idents = await _serverRequest.GetIdentiticationTypeAsync(); 
                var itemList = _dataContext.deposit_customerIdentification.Where(d => d.Deleted == false && d.CustomerId == request.customerId).ToList(); 

                if (itemList.Count() > 0)
                {
                    response.Identification = _mapper.Map<List<IdentificationObj>>(itemList);
                    foreach (var item in response.Identification)
                    {
                        item.IdentificationName = idents.commonLookups.FirstOrDefault(e => e.LookupId == item.Identification)?.LookupName; 
                    }  
                } 
                response.Status.Message.FriendlyMessage = response.Identification.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() =>  response);
            }
        }
    }
    
}
