using Deposit.Contracts.GeneralExtension;
using Deposit.DomainObjects.Deposit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace GODP.Entities.Models
{ 
    public partial class deposit_accountype : GeneralEntity
    { 
        public deposit_accountype()
        {
            deposit_accountsetup = new HashSet<deposit_accountsetup>();
            deposit_customer_account_information = new HashSet<deposit_customer_account_information>();
        }

        [Key]
        public int AccountTypeId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string AccountNunmberPrefix { get; set; }

        [StringLength(500)]
        public string Description { get; set; } 
        public virtual ICollection<deposit_customer_account_information> deposit_customer_account_information { get; set; }
        public virtual ICollection<deposit_accountsetup> deposit_accountsetup { get; set; }
        public virtual ICollection<deposit_withdrawalsetup> deposit_withdrawalsetup { get; set; }
    }
}
