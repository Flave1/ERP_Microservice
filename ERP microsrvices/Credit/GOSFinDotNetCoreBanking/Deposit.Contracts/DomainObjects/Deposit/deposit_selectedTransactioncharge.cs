namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_selectedTransactioncharge : GeneralEntity
    {
        [Key]
        public int SelectedTransChargeId { get; set; }

        public int? TransactionChargeId { get; set; }

        public int? AccountId { get; set; } 

        //public virtual deposit_transactioncharge deposit_transactioncharge { get; set; }
    }
}
