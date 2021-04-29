namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_customeridentification : GeneralEntity
    {
        [Key]
        public int CustomerIdentityId { get; set; }

        public int CustomerId { get; set; }

        public int MeansOfID { get; set; }

        [StringLength(50)]
        public string IDNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateIssued { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }
        //public virtual deposit_accountopening deposit_accountopening { get; set; }
    }
}
