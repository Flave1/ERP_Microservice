namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_tillvaultsetup : GeneralEntity
    {
        [Key]
        public int TillVaultSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        [StringLength(50)]
        public string StructureTillIdPrefix { get; set; }

        [StringLength(50)]
        public string TellerTillIdPrefix { get; set; } 
    }
}
