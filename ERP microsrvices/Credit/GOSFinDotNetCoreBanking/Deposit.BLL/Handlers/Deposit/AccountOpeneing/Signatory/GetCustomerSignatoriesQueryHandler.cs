using AutoMapper;

using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.Response.Deposit.AccountOpening;

namespace Deposit.Handlers.Deposit.CustomerSignatories
{
    public class GetAllCustomerSignatoriesQuery : IRequest<CustomerSignatoriesResp>
    {
        public int customerId { get; set; }
        public class GetAllCustomerSignatoriesQueryHandler : IRequestHandler<GetAllCustomerSignatoriesQuery, CustomerSignatoriesResp>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IMapper _mapper;
            
            public GetAllCustomerSignatoriesQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, IMapper mapper)
            {
                _mapper = mapper;
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<CustomerSignatoriesResp> Handle(GetAllCustomerSignatoriesQuery request, CancellationToken cancellationToken)
            {
                var response = new CustomerSignatoriesResp { Signatories = new List<Signatory>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                var idents = await _serverRequest.GetIdentiticationTypeAsync(); 
                var itemList = await _dataContext.deposit_signatories.Where(d => d.Deleted == false && d.CustomerId == request.customerId).ToListAsync(); 

                if (itemList.Count() > 0)
                {
                    response.Signatories = _mapper.Map<List<Signatory>>(itemList);
                    foreach (var item in response.Signatories)
                    {
                        item.IdentificationTypeName = idents.commonLookups.FirstOrDefault(e => e.LookupId == item.IdentificationType)?.LookupName;
                        if (item.ClassOfSignatory == 1)
                            item.SignatoryClassName = "CLASS A";
                        if (item.ClassOfSignatory == 2)
                            item.SignatoryClassName = "CLASS B";
                        if (item.ClassOfSignatory == 2)
                            item.SignatoryClassName = "CLASS C";
                    }  
                } 
                response.Status.Message.FriendlyMessage = response.Signatories.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() =>  response);
            }
        }
    }
    
}
