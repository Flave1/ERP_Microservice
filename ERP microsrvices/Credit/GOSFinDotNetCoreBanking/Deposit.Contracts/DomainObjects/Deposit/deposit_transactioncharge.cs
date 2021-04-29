namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using Deposit.DomainObjects.Deposit;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_transactioncharge : GeneralEntity
    {

        [Key]
        public int TransactionChargeId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string FixedOrPercentage { get; set; }

        public decimal? Amount_Percentage { get; set; }

        [StringLength(500)]
        public string Description { get; set; } 

        public virtual ICollection<Account_setup_transaction_charges> Account_setup_transaction_charges { get; set; }
         
    }
}
