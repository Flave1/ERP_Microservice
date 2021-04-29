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

namespace Deposit.Handlers.Deposit.TransactionCorrectionSetup
{
    public class GetAllTransactionCorrectionSetupQuery : IRequest<TransactionCorrectionSetupResp>
    {
        public class GetAllTransactionCorrectionSetupQueryHandler : IRequestHandler<GetAllTransactionCorrectionSetupQuery, TransactionCorrectionSetupResp>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            
            public GetAllTransactionCorrectionSetupQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest)
            {
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<TransactionCorrectionSetupResp> Handle(GetAllTransactionCorrectionSetupQuery request, CancellationToken cancellationToken)
            {
                var response = new TransactionCorrectionSetupResp { TransactionCorrectionSetups = new List<Contracts.Response.Deposit.TransactionCorrectionSetup>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

                var comp = await _serverRequest.GetAllCompanyAsync();
                var titles = await _serverRequest.GetAllJobTileAsync();
                var itemList = _dataContext.deposit_transactioncorrectionsetup.Where(d => d.Deleted == false).ToList();
                if(itemList.Count() > 0)
                {
                    response.TransactionCorrectionSetups = itemList.Select(e => new Contracts.Response.Deposit.TransactionCorrectionSetup
                    {   
                        PresetChart = e.PresetChart,
                        Structure = e.Structure,
                        CompanyName = comp.companyStructures.FirstOrDefault(r => r.companyStructureId == e.Structure)?.name, 
                        TransactionCorrectionSetupId = e.TransactionCorrectionSetupId,
                        JobTitleId = e.JobTitleId,
                        JobTitleName = titles.commonLookups.FirstOrDefault(f => f.LookupId == e.JobTitleId)?.LookupName

                    }).ToList();
                } 

                response.Status.Message.FriendlyMessage = response.TransactionCorrectionSetups.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() =>  response);
            }
        }
    }
    
}
