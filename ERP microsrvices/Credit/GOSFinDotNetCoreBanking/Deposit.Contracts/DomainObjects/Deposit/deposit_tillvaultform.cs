namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_tillvaultform : GeneralEntity
    {
        [Key]
        public int TillVaultId { get; set; }

        public int? TillId { get; set; }

        public int? Currency { get; set; }

        public decimal? OpeningBalance { get; set; }

        public decimal? IncomingCash { get; set; }

        public decimal? OutgoingCash { get; set; }

        public decimal? ClosingBalance { get; set; }

        public decimal? CashAvailable { get; set; }

        [StringLength(10)]
        public string Shortage { get; set; } 
    }
}
