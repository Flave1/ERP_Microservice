namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class deposit_customersignature : GeneralEntity
    {
        [Key]
        public int SignatureId { get; set; }

        public int CustomerId { get; set; }

        public int SignatoryId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public byte[] SignatureImg { get; set; } 

        //public virtual deposit_accountopening deposit_accountopening { get; set; }

        //public virtual deposit_customersignatory deposit_customersignatory { get; set; }
    }
}
