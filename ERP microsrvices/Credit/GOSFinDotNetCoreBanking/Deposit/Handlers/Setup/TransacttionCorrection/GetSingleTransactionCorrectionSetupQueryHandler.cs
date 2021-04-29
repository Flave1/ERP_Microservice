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
    public class GetSingleTransactionCorrectionSetupQuery : IRequest<TransactionCorrectionSetupResp>
    {
        public int TransactionCorrectionSetupId { get; set; }
        public class GetSingleTransactionCorrectionSetupQueryHandler : IRequestHandler<GetSingleTransactionCorrectionSetupQuery, TransactionCorrectionSetupResp>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;

            public GetSingleTransactionCorrectionSetupQueryHandler(DataContext dataContext, IIdentityServerRequest identityServerRequest)
            {
                _serverRequest = identityServerRequest;
                _dataContext = dataContext;
            }
            public async Task<TransactionCorrectionSetupResp> Handle(GetSingleTransactionCorrectionSetupQuery request, CancellationToken cancellationToken)
            {
                var response = new TransactionCorrectionSetupResp { TransactionCorrectionSetups = new List<Contracts.Response.Deposit.TransactionCorrectionSetup>(), Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                 
                var itemList = _dataContext.deposit_transactioncorrectionsetup.Where(d => d.TransactionCorrectionSetupId == request.TransactionCorrectionSetupId && d.Deleted == false).ToList();
                if (itemList.Count() > 0)
                {
                    response.TransactionCorrectionSetups = itemList.Select(e => new Contracts.Response.Deposit.TransactionCorrectionSetup
                    {
                        PresetChart = e.PresetChart,
                        Structure = e.Structure,  
                        JobTitleId = e.JobTitleId,
                        TransactionCorrectionSetupId = e.TransactionCorrectionSetupId, 
                    }).ToList();
                }

                response.Status.Message.FriendlyMessage = response.TransactionCorrectionSetups.Count() > 0 ? "" : "Search Complete!! No Record Found";
                return await Task.Run(() => response);
            }
        }
    }

}
