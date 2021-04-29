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

namespace Deposit.Handlers.Deposit.TillVaultSetup
{
    public class GetAllTillVaultSetupQuery : IRequest<TillVaultSetupRespObj>
    {
        public class GetAllTillVaultSetupQueryHandler : IRequestHandler<GetAllTillVaultSetupQuery, TillVaultSetupRespObj>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            
            public GetAllTillVaultSetupQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest)
            {
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<TillVaultSetupRespObj> Handle(GetAllTillVaultSetupQuery request, CancellationToken cancellationToken)
            {
                var response = new TillVaultSetupRespObj { TillVaultSetups = new List<TillVaultSetupObj>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                var comp = await _serverRequest.GetAllCompanyAsync();
                var itemList = _dataContext.deposit_tillvaultsetup.Where(d => d.Deleted == false).ToList();
                if(itemList.Count() > 0)
                {
                    response.TillVaultSetups = itemList.Select(e => new TillVaultSetupObj
                    {
                        PresetChart = e.PresetChart,
                        Structure = e.Structure,
                        CompanyName = comp.companyStructures.FirstOrDefault(r => r.companyStructureId == e.Structure)?.name,
                        StructureTillIdPrefix = e.StructureTillIdPrefix,
                        TellerTillIdPrefix = e.TellerTillIdPrefix,
                        TillVaultSetupId = e.TillVaultSetupId,
                        
                    }).ToList();
                } 

                response.Status.Message.FriendlyMessage = response.TillVaultSetups.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() =>  response);
            }
        }
    }
    
}
