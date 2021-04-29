namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_accountreactivationsetup : GeneralEntity
    {
        [Key]
        public int ReactivationSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? ChargesApplicable { get; set; }

        [StringLength(50)]
        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string ChargeType { get; set; }

        public bool? PresetChart { get; set; } 
    }
}
