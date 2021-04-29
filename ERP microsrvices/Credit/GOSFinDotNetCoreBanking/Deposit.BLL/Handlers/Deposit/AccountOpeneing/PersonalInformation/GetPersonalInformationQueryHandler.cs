using AutoMapper;

using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.Response.Deposit.AccountOpening;

namespace Deposit.Handlers.Deposit.PersonalInformation
{
    public class GetAllPersonalInformationQuery : IRequest<PersonalInformationResp>
    {
        public class GetAllPersonalInformationQueryHandler : IRequestHandler<GetAllPersonalInformationQuery, PersonalInformationResp>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            private readonly IMapper _mapper;
            
            public GetAllPersonalInformationQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest, IMapper mapper)
            {
                _mapper = mapper;
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<PersonalInformationResp> Handle(GetAllPersonalInformationQuery request, CancellationToken cancellationToken)
            {
                var response = new PersonalInformationResp { CustomerLiteAccountDetails = new List<CustomerLiteAccountDetails>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                //var comp = await _serverRequest.GetAllCompanyAsync();
                //var titles = await _serverRequest.GetAllJobTileAsync();
                var itemList = _dataContext.deposit_accountopening.Where(d => d.Deleted == false).ToList(); 

                if (itemList.Count() > 0)
                {
                    response.CustomerLiteAccountDetails = _mapper.Map<List<CustomerLiteAccountDetails>>(itemList);
                    foreach (var item in response.CustomerLiteAccountDetails)
                    {
                        item.CustomerTypeName = item.CustomerTypeId == (int)CustomerType.Corporate ? "Corportate" : "Individual";
                        item.Name = item.CustomerTypeId == (int)CustomerType.Corporate ? itemList.FirstOrDefault(e => e.CustomerId == item.CustomerId)?.CompanyName : $"{itemList.FirstOrDefault(e => e.CustomerId == item.CustomerId)?.Firstname} {itemList.FirstOrDefault(e => e.CustomerId == item.CustomerId)?.Surname}";
                    }  
                }

                response.Status.Message.FriendlyMessage = response.CustomerAccountDetails.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() =>  response);
            }
        }
    }
    
}
