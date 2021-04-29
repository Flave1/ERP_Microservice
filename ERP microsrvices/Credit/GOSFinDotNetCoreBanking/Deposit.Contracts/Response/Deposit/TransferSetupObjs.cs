using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Deposit.Contracts.Response.Deposit
{
    public class TransferSetupObj
    {
        public int TransferSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? PresetChart { get; set; }

        public int? AccountType { get; set; }

        public string DailyWithdrawalLimit { get; set; }

        public bool? ChargesApplicable { get; set; }

        public string Charges { get; set; }

        public decimal? Amount { get; set; }

        public string ChargeType { get; set; }
         
        public string CompanyName { get; set; }
        public string ProductName { get; set; }
        public string AccountTypeName { get; set; }
    }

    public class AddUpdateTransferSetupObj
    {
        public int TransferSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? PresetChart { get; set; }

        public int? AccountType { get; set; }

        [StringLength(50)]
        public string DailyWithdrawalLimit { get; set; }

        public bool? ChargesApplicable { get; set; }

        [StringLength(50)]
        public string Charges { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string ChargeType { get; set; }
    }

    public class TransferSetupRegRespObj
    {
        public int TransferSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TransferSetupRespObj
    {
        public List<TransferSetupObj> TransferSetups { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
