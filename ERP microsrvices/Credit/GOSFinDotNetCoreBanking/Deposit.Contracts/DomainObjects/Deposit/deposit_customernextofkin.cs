namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_customernextofkin : GeneralEntity
    {
        [Key]
        public int NextOfKinId { get; set; }

        public int CustomerId { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string OtherName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        public int? GenderId { get; set; }

        [StringLength(50)]
        public string Relationship { get; set; }

        [StringLength(50)]
        public string MobileNumber { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; } 
        //public virtual deposit_accountopening deposit_accountopening { get; set; }
    }
}
