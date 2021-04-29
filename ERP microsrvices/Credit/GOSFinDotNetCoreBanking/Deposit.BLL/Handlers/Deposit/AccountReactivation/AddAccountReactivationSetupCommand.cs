using Deposit.Contracts.Command;
using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.BankClosure
{

    public class AddUpdateReactivationAccountSetupCommand : IRequest<AccountReactivationSetupRegRespObj>
    { 
        public int ReactivationSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? ChargesApplicable { get; set; }
         
        public string Charge { get; set; }

        public decimal? Amount { get; set; }
         
        public string ChargeType { get; set; }

        public bool? PresetChart { get; set; }
        public class AddUpdateReactivationAccountSetupCommandHandler : IRequestHandler<AddUpdateReactivationAccountSetupCommand, AccountReactivationSetupRegRespObj>
        {
            private readonly DataContext _dataContext;
            public AddUpdateReactivationAccountSetupCommandHandler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task<AccountReactivationSetupRegRespObj> Handle(AddUpdateReactivationAccountSetupCommand request, CancellationToken cancellationToken)
            {
                var response = new AccountReactivationSetupRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                try
                {
                    var Trate = _dataContext.deposit_accountreactivationsetup.Find(request.ReactivationSetupId);
                    if (Trate == null)
                        Trate = new deposit_accountreactivationsetup();

                    Trate.ReactivationSetupId = request.ReactivationSetupId;
                    Trate.Structure = request.Structure;
                    Trate.Product = request.Product;
                    Trate.ChargesApplicable = request.ChargesApplicable;
                    Trate.Charge = request.Charge;
                    Trate.ChargeType = request.ChargeType;
                    Trate.PresetChart = request.PresetChart; 
                    Trate.Amount = request.Amount;
                    Trate.PresetChart = request.PresetChart; 

                    if (Trate.ReactivationSetupId > 0)
                    {
                        var item = _dataContext.deposit_accountreactivationsetup.Find(Trate.ReactivationSetupId);
                        if (item != null)
                            _dataContext.Entry(item).CurrentValues.SetValues(Trate);
                    }
                    else
                        _dataContext.deposit_accountreactivationsetup.Add(Trate);
                    _dataContext.SaveChanges();

                    response.ReactivationSetupId = Trate.ReactivationSetupId;
                    response.Status.Message.FriendlyMessage = "Successful";
                    return response;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
    
}
