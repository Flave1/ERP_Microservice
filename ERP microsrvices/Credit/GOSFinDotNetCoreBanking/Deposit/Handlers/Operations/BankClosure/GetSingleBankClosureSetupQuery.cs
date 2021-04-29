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
    public class GetSingleBankClosureSetupQuery : IRequest<Deposit_BankClosureSetupRespObj>
    {
        public int BankClosureSetupId { get; set; }
        public class GetSingleBankClosureSetupQueryHandler : IRequestHandler<GetSingleBankClosureSetupQuery, Deposit_BankClosureSetupRespObj>
        {
            private readonly DataContext _context;
            private readonly IIdentityServerRequest _serverRequest;
            public GetSingleBankClosureSetupQueryHandler(DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _context = dataContext;
                _serverRequest = serverRequest;
            }
            public async Task<Deposit_BankClosureSetupRespObj> Handle(GetSingleBankClosureSetupQuery request, CancellationToken cancellationToken)
            {
                var response = new Deposit_BankClosureSetupRespObj
                {
                    BankClosureSetups = new List<Deposit_bankClosureSetupObjs>(),
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() }
                }; 
                var res = (from a in _context.deposit_bankclosuresetup
                              where a.BankClosureSetupId == request.BankClosureSetupId  select
                             new Deposit_bankClosureSetupObjs
                             {
                                 BankClosureSetupId = a.BankClosureSetupId,
                                 Structure = a.Structure,
                                 ProductId = a.ProductId,
                                 ClosureChargeApplicable = a.ClosureChargeApplicable,
                                 Charge = a.Charge,
                                 ChargeType = a.ChargeType,
                                 PresetChart = a.PresetChart,
                                 SettlementBalance = a.SettlementBalance,
                                 Percentage = a.Percentage
                             }).ToList();
                response.BankClosureSetups = res;
                return response;
            }
        }
    }
}
