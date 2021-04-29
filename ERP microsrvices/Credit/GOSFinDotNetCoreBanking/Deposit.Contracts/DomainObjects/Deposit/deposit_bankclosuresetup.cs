namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_bankclosuresetup : GeneralEntity
    {
        [Key]
        public int BankClosureSetupId { get; set; }

        public int? Structure { get; set; }

        public int? ProductId { get; set; }

        public bool? ClosureChargeApplicable { get; set; }

        [StringLength(50)]
        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string ChargeType { get; set; }

        public bool? SettlementBalance { get; set; } 
        public bool? PresetChart { get; set; } 
        public double Percentage { get; set; }
    }
}
