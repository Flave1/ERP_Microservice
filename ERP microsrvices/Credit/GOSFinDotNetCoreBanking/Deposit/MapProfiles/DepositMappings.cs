using AutoMapper;
using Deposit.Contracts.Response.Deposit;
using GODP.Entities.Models;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.DomainObjects.Deposit;

namespace Deposit.MapProfiles
{
    public class DepositMappings : Profile
    {
        public DepositMappings()
        {
            CreateMap<deposit_accountsetup, DepositAccountObj>();
            CreateMap<deposit_withdrawalsetup, WithdrawalSetupObj>();
            CreateMap<deposit_transfersetup, TransferSetupObj>();
             

            CreateMap<deposit_directors, Directors>();
            CreateMap<deposit_customerIdentifications, IdentificationObj>();  

            CreateMap<deposit_changeofrates, ChangeOfRatesObj>(); 
        }
    }
}
