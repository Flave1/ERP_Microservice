namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_tillvaultsummary : GeneralEntity
    {
        [Key]
        public int TillVaultSummaryId { get; set; }

        public int? TillVaultId { get; set; }

        public int? TransactionCount { get; set; }

        public decimal? TotalAmountCurrency { get; set; }

        public decimal? TransferAmount { get; set; } 
    }
}
