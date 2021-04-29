namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using Deposit.DomainObjects.Deposit;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_changeofratesetup : GeneralEntity
    {
        [Key]
        public int ChangeOfRateSetupId { get; set; }

        public int? Structure { get; set; } 

        public bool? CanApply { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public  deposit_accountsetup deposit_accountsetup { get; set; }
    }
}
