using Deposit.Contracts.GeneralExtension;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Deposit
{
   
    public class Deposit_bankClosureObj 
    { 
        public long BankClosureId { get; set; } 
        public int? Structure { get; set; } 
        public int? SubStructure { get; set; } 
        public string AccountName { get; set; } 
        public string AccountNumber { get; set; } 
        public bool? Status { get; set; } 
        public string AccountBalance { get; set; } 
        public int? Currency { get; set; } 
        public DateTime? ClosingDate { get; set; } 
        public string Reason { get; set; } 
        public decimal? Charges { get; set; } 
        public string FinalSettlement { get; set; } 
        public string Beneficiary { get; set; } 
        public int ModeOfSettlement { get; set; }
        public string ModeOfSettlementName { get; set; } 
        public string TransferAccount { get; set; } 
        public string Status_name { get; set; } 

       
        public string ApproverComment { get; set; }
        //..........................
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string CurrencyName  { get; set; }

        public int ApprovalStatusId { get; set; }
        public string WorkflowToken { get; set; }

    }

   

    public class Deposit_bankClosureRegRespObj
    {
        public int BankClosureId { get; set; } 
        public APIResponseStatus Status { get; set; }
    }
    public class Deposit_BankClosureRespObj
    {
        public List<Deposit_bankClosureObj> BankClosures { get; set; } 
        public APIResponseStatus Status { get; set; }
    } 
}
