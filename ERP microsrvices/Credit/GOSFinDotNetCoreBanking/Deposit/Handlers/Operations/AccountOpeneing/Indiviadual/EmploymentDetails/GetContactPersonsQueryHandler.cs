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

namespace Deposit.Handlers.Deposit.ContactPersons
{
    public class GetAllContactPersonsQuery : IRequest<ContactPersonsResp>
    {
        public int customerId { get; set; }
        public class GetAllContactPersonsQueryHandler : IRequestHandler<GetAllContactPersonsQuery, ContactPersonsResp>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IMapper _mapper;
            
            public GetAllContactPersonsQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, IMapper mapper)
            {
                _mapper = mapper;
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<ContactPersonsResp> Handle(GetAllContactPersonsQuery request, CancellationToken cancellationToken)
            {
                var response = new ContactPersonsResp { KeyContactPersons = new List<Contracts.Response.Deposit.AccountOpening.KeyContactPersons>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                //var idents = await _serverRequest.GetIdentiticationTypeAsync(); 
                //var itemList = _dataContext.deposit_keycontactpersons.Where(d => d.Deleted == false && d.CustomerId == request.customerId).ToList(); 

                //if (itemList.Count() > 0)
                //{
                //    response.KeyContactPersons = _mapper.Map<List<Contracts.Response.Deposit.AccountOpening.KeyContactPersons>>(itemList); 
                //} 
                response.Status.Message.FriendlyMessage = response.KeyContactPersons.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() =>  response);
            }
        }
    }
    
}
