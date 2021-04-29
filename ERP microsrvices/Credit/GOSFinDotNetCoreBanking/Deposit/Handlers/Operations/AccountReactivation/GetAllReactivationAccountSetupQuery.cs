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
    public class GetAllReactivationAccountSetupQuery : IRequest<AccountReactivationSetupRespObj>
    {
        public class GetAllReactivationAccountSetupQueryHandler : IRequestHandler<GetAllReactivationAccountSetupQuery, AccountReactivationSetupRespObj>
        {
            private readonly DataContext _context;
            private readonly IIdentityServerRequest _serverRequest;
            public GetAllReactivationAccountSetupQueryHandler(DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _context = dataContext;
                _serverRequest = serverRequest;
            }
            public async Task<AccountReactivationSetupRespObj> Handle(GetAllReactivationAccountSetupQuery request, CancellationToken cancellationToken)
            {
                var response = new AccountReactivationSetupRespObj
                {
                    ReactivationSetup = new List<AccountReactivationSetupObj>(),
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() }
                };
                var comp = await _serverRequest.GetAllCompanyAsync();
                var res = (from a in _context.deposit_accountreactivationsetup
                              where a.Deleted == false
                              select
                             new AccountReactivationSetupObj
                             {
                                 ReactivationSetupId = a.ReactivationSetupId,
                                 Structure = a.Structure,
                                 Product = a.Product,
                                 ChargesApplicable = a.ChargesApplicable,
                                 Charge = a.Charge,
                                 ChargeType = a.ChargeType,
                                 PresetChart = a.PresetChart,
                             }).ToList();

                foreach(var item in res)
                {
                    item.CompanyName = comp.companyStructures.FirstOrDefault(x => x.companyStructureId == item.Structure)?.name;
                    item.ProductName = _context.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == item.Product)?.AccountName;
                }
                response.ReactivationSetup = res;
                return response;
            }
        }
    }
}
