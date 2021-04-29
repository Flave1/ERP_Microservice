namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_accountreactivation : GeneralEntity
    {
        [Key]
        public int ReactivationId { get; set; }

        public int? Structure { get; set; }

        public int? Substructure { get; set; }

        [StringLength(50)]
        public string AccountName { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public decimal? AccountBalance { get; set; }

        public int? Currency { get; set; }

        public decimal? Balance { get; set; }

        [StringLength(50)]
        public string Reason { get; set; }

        [StringLength(50)]
        public string Charges { get; set; }

        [StringLength(50)]
        public string ApproverName { get; set; }

        [StringLength(50)]
        public string ApproverComment { get; set; } 
    }
}
