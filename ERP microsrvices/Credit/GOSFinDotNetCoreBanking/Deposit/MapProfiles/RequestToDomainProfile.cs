using Deposit.Contracts.Response.Deposit;
using AutoMapper;
using GODP.Entities.Models; 

namespace Deposit.MapProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<AddUpdateAccountTypeObj, deposit_accountype>();
        }
    }
}
