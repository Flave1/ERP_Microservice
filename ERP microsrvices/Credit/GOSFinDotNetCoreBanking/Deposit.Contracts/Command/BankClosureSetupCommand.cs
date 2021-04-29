using Deposit.Contracts.GeneralExtension;
using Deposit.Contracts.Response.Deposit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Command
{
    public class AddUpdateBankClosureSetupCommand : IRequest<Deposit_bankClosureSetupRegRespObj>
    {
        public int BankClosureSetupId { get; set; }

        public int? Structure { get; set; }

        public int ProductId { get; set; }

        public bool? ClosureChargeApplicable { get; set; }

        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        public string ChargeType { get; set; }

        public bool? SettlementBalance { get; set; }

        public bool? PresetChart { get; set; }
        public double Percentage { get; set; }
    }
    public class DeleteBankClosureSetupCommand : IRequest<Delete_response>
    {
        public List<int> BankClosureSetupIds { get; set; }
    }
}
