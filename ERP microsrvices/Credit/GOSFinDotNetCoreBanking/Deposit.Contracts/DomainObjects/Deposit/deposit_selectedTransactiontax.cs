namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_selectedTransactiontax : GeneralEntity
    {
        [Key]
        public int SelectedTransTaxId { get; set; }

        public int? TransactionTaxId { get; set; }

        public int? AccountId { get; set; } 

        //public virtual deposit_transactiontax deposit_transactiontax { get; set; }
    }
}
