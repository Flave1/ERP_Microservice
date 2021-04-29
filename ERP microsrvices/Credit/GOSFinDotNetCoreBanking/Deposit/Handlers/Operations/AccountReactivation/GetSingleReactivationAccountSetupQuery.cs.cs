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

namespace Deposit.Handlers.Deposit.BankClosure
{
    public class GetSingleReactivationAccountSetupQuery : IRequest<AccountReactivationSetupRespObj>
    {
        public int ReactivationSetupId { get; set; }
        public class GetSingleReactivationAccountSetupQueryHandler : IRequestHandler<GetSingleReactivationAccountSetupQuery, AccountReactivationSetupRespObj>
        {
            private readonly DataContext _context;
            private readonly IIdentityServerRequest _serverRequest;
            public GetSingleReactivationAccountSetupQueryHandler(DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _context = dataContext;
                _serverRequest = serverRequest;
            }
            public async Task<AccountReactivationSetupRespObj> Handle(GetSingleReactivationAccountSetupQuery request, CancellationToken cancellationToken)
            {
                var response = new AccountReactivationSetupRespObj
                {
                     ReactivationSetup = new List<AccountReactivationSetupObj>(),
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() }
                };
                 
                var res = (from a in _context.deposit_accountreactivationsetup
                                where a.Deleted == false && a.ReactivationSetupId == request.ReactivationSetupId
                                select
                               new AccountReactivationSetupObj
                               {
                                   ReactivationSetupId = a.ReactivationSetupId,
                                   Structure = a.Structure,
                                   Product = a.Product,
                                   ChargesApplicable = a.ChargesApplicable,
                                   Charge = a.Charge,
                                   ChargeType = a.ChargeType,
                                   PresetChart = a.PresetChart
                               }).FirstOrDefault();
                if (res != null)
                    response.ReactivationSetup.Add(res); 
                return response;
            }
        }
    }
}
