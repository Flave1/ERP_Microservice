namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using Deposit.DomainObjects.Deposit;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_cashiertellersetup : GeneralEntity
    {
        [Key]
        public int DepositCashierTellerSetupId { get; set; }

        public int Structure { get; set; }
        public string Sub_strructure { get; set; } 
        public int ProductId { get; set; }
        public long Employee_ID { get; set; } 
        public bool? PresetChart { get; set; } 
        [ForeignKey("ProductId")]
        public deposit_accountsetup deposit_accountsetup { get; set; }
    }
   
    public class deposit_cashierteller_form : GeneralEntity
    {
        [Key]
        public long Id { get; set; }

        public int Structure { get; set; }

        public string SubStructure { get; set; }

        public long Employee_ID { get; set; }
            
        public DateTime? Date { get; set; } 

        public string Transaction_IDs { get; set; }
        public int Approval_status { get; set; }
        public string WorkflowToken { get; set; } 
    }

    public class deposit_call_over_currecies_and_amount : GeneralEntity
    { 
        public long Id { get; set; }
        public long Currency { get; set; }
        public decimal Amount { get; set; }
        public string User_id { get; set; }
        public DateTime Call_over_date { get; set; }
    }

}
