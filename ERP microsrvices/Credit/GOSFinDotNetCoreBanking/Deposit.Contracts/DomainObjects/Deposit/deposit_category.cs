namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using Deposit.DomainObjects.Deposit;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_category : GeneralEntity
    { 
        public deposit_category()
        {
            deposit_accountsetup = new HashSet<deposit_accountsetup>();
            deposit_customer_account_information = new HashSet<deposit_customer_account_information>();
        }

        [Key]
        public int CategoryId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; } 
        public virtual ICollection<deposit_customer_account_information> deposit_customer_account_information { get; set; }
        public virtual ICollection<deposit_accountsetup> deposit_accountsetup { get; set; }
    }
}
