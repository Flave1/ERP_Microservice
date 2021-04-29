using AutoMapper;
using Deposit.Contracts.Response.Deposit;
using Deposit.Requests;
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
    public class GetSingleTillVaultSetupQuery : IRequest<TillVaultSetupRespObj>
    {
        public int TillVaulSetuptId { get; set; }
        public class GetSingleTillVaultSetupQueryHandler : IRequestHandler<GetSingleTillVaultSetupQuery, TillVaultSetupRespObj>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;

            public GetSingleTillVaultSetupQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest)
            {
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<TillVaultSetupRespObj> Handle(GetSingleTillVaultSetupQuery request, CancellationToken cancellationToken)
            {
                var response = new TillVaultSetupRespObj { TillVaultSetups = new List<TillVaultSetupObj>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                 
                var itemList = _dataContext.deposit_tillvaultsetup.Where(d => d.TillVaultSetupId == request.TillVaulSetuptId && d.Deleted == false).ToList();
                if (itemList.Count() > 0)
                {
                    response.TillVaultSetups = itemList.Select(e => new TillVaultSetupObj
                    {
                        PresetChart = e.PresetChart,
                        Structure = e.Structure, 
                        StructureTillIdPrefix = e.StructureTillIdPrefix,
                        TellerTillIdPrefix = e.TellerTillIdPrefix,
                        TillVaultSetupId = e.TillVaultSetupId,

                    }).ToList();
                }

                response.Status.Message.FriendlyMessage = response.TillVaultSetups.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() => response);
            }
        }
    }

}
