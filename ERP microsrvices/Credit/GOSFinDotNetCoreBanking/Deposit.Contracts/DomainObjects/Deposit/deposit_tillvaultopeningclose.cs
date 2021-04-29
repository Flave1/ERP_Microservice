namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_tillvaultopeningclose : GeneralEntity
    {
        [Key]
        public int TillVaultOpeningCloseId { get; set; }

        public DateTime? Date { get; set; }

        public int? Currency { get; set; }

        public decimal? AmountPerSystem { get; set; }

        public decimal? CashAvailable { get; set; }

        public decimal? Shortage { get; set; } 
    }
}
