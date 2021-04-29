using Deposit.Contracts.GeneralExtension;
using System; 
using System.ComponentModel.DataAnnotations; 

namespace Deposit.DomainObjects.Deposit
{
    public class deposit_form : GeneralEntity
    {
        [Key]
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public int Structure { get; set; }
        public string TransactionId { get; set; }
        public string Account_number { get; set; } 
        public long Currency { get; set; }
        public decimal Deposit_amount { get; set; }
        public DateTime Value_date { get; set; }
        public string Transaction_particulars { get; set; }
        public string Remark { get; set; }
        public int Transaction_mode { get; set; }
        public string Instrument_number { get; set; }
        public DateTime Instrument_date { get; set; }
        public bool Is_call_over_done { get; set; }
    }

    public class deposit_reactivation_form : GeneralEntity
    {
        [Key]
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public int Product { get; set; }
        public string Account_number { get; set; }
        public int? Structure { get; set; }
        public int? Substructure { get; set; }
        public decimal Charges { get; set; }
        public decimal Available_balance { get; set; }
        public string Reactivation_reason { get; set; }
        public int ApprovalStatusId { get; set; } 
        public string WorkflowToken { get; set; }
        public int ReactivationSetupId { get; set; }
    }

    public class deposit_withdrawal_form : GeneralEntity
    {
        [Key]
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public int Structre { get; set; }
        public long Currency { get; set; }
        public int Product { get; set; }
        public bool Is_call_over_done { get; set; }
        public string Transaction_Id { get; set; } 
        public string Account_number { get; set; }
        public decimal Widthrawal_setup { get; set; }
        public decimal Amount { get; set; }
        public string Transaction_description { get; set; }
        public DateTime Transaction_date { get; set; }
        public DateTime Value_date { get; set; }
        public int Withdrawal_type { get; set; }
        public string Withdrawal_instrument { get; set; }
        public DateTime Instrument_date { get; set; }
        public decimal Total_charge { get; set; }
        public decimal Available_balance { get; set; } 
    }
}
