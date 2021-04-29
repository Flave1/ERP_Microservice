namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_transfersetup : GeneralEntity
    {
        [Key]
        public int TransferSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? PresetChart { get; set; }

        public int? AccountType { get; set; }

        [StringLength(50)]
        public string DailyWithdrawalLimit { get; set; }

        public bool? ChargesApplicable { get; set; }

        [StringLength(50)]
        public string Charges { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string ChargeType { get; set; } 
    }
}
