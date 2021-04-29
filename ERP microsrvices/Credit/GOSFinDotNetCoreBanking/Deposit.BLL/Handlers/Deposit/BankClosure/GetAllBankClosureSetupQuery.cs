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
    public class GetAllBankClosureSetupQuery : IRequest<Deposit_BankClosureSetupRespObj>
    {
        public class GetAllBankClosureSetupQueryHandler : IRequestHandler<GetAllBankClosureSetupQuery, Deposit_BankClosureSetupRespObj>
        {
            private readonly DataContext _context;
            private readonly IIdentityServerRequest _serverRequest;
            public GetAllBankClosureSetupQueryHandler(DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _context = dataContext;
                _serverRequest = serverRequest;
            }
            public async Task<Deposit_BankClosureSetupRespObj> Handle(GetAllBankClosureSetupQuery request, CancellationToken cancellationToken)
            {
                var response = new Deposit_BankClosureSetupRespObj
                {
                    BankClosureSetups = new List<Deposit_bankClosureSetupObjs>(),
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() }
                };
                var comp = await _serverRequest.GetAllCompanyAsync();
                var res = (from a in _context.deposit_bankclosuresetup
                              where a.Deleted == false
                              select
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

                foreach(var item in res)
                {
                    item.CompanyName = comp.companyStructures.FirstOrDefault(x => x.companyStructureId == item.Structure)?.name;
                    item.ProductName = _context.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == item.ProductId)?.AccountName;
                }
                response.BankClosureSetups = res;
                return response;
            }
        }
    }
}
