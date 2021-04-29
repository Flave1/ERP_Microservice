namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_transactioncorrectionform : GeneralEntity
    {
        [Key]
        public int TransactionCorrectionId { get; set; }

        public int? Structure { get; set; }

        public int? SubStructure { get; set; }

        public DateTime? QueryStartDate { get; set; }

        public DateTime? QueryEndDate { get; set; }

        public int? Currency { get; set; }

        public decimal? OpeningBalance { get; set; }

        public decimal? ClosingBalance { get; set; } 
    }
}
