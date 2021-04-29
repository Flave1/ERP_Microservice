using Deposit.Contracts.GeneralExtension;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Deposit
{
    public class Deposit_bankClosureSetupObjs : GeneralEntity
    {
        public int BankClosureSetupId { get; set; }

        public int? Structure { get; set; }

        public int? ProductId { get; set; }

        public bool? ClosureChargeApplicable { get; set; }

        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        public string ChargeType { get; set; }

        public bool? SettlementBalance { get; set; }

        public bool? PresetChart { get; set; } 

        public string CompanyName { get; set; }
        public string ProductName { get; set; }
        public double Percentage { get; set; }
        public int ExcelLine { get; set; }
    }

    public class Deposit_bankClosureSetupRegRespObj
    {
        public int BankClosureSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class Deposit_BankClosureSetupRespObj
    {
        public List<Deposit_bankClosureSetupObjs> BankClosureSetups { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
