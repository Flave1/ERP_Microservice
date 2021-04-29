namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_transactioncorrectionsetup : GeneralEntity
    {
        [Key]
        public int TransactionCorrectionSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        public int? JobTitleId { get; set; } 
    }
}
