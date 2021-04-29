namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_withdrawalform : GeneralEntity
    {
        [Key]
        public int WithdrawalFormId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        [StringLength(50)]
        public string TransactionReference { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public int? AccountType { get; set; }

        public int? Currency { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string TransactionDescription { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValueDate { get; set; }

        [StringLength(50)]
        public string WithdrawalType { get; set; }

        [StringLength(50)]
        public string InstrumentNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InstrumentDate { get; set; }

        public decimal? ExchangeRate { get; set; }

        public decimal? TotalCharge { get; set; } 
    }
}
