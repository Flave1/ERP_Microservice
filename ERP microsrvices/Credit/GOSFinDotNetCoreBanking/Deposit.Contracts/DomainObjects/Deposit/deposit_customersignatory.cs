namespace GODP.Entities.Models
{
    using Deposit.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class deposit_customersignatory : GeneralEntity
    { 
        public deposit_customersignatory()
        {
            deposit_customersignature = new HashSet<deposit_customersignature>();
        }

        [Key]
        public int SignatoryId { get; set; }

        public int CustomerId { get; set; }

        public int? TitleId { get; set; }

        public int? GenderId { get; set; }

        public int? MaritalStatusId { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Othername { get; set; }

        [StringLength(50)]
        public string ClassofSignatory { get; set; }

        [StringLength(50)]
        public string IdentificationType { get; set; }

        [StringLength(50)]
        public string IdentificationNumber { get; set; }

        [StringLength(50)]
        public string Telephone { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public byte[] SignatureUpload { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DoB { get; set; }

        [StringLength(50)]
        public string PlaceOfBirth { get; set; }

        [StringLength(50)]
        public string MaidenName { get; set; }

        [StringLength(50)]
        public string NextofKin { get; set; }

        public int? LGA { get; set; }

        public int? StateOfOrigin { get; set; }

        [StringLength(50)]
        public string TaxIDNumber { get; set; }

        [StringLength(50)]
        public string MeansOfID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IDExpiryDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IDIssueDate { get; set; }

        [StringLength(50)]
        public string Occupation { get; set; }

        [StringLength(50)]
        public string JobTitle { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        public int? Nationality { get; set; }

        [StringLength(50)]
        public string ResidentPermit { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PermitIssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PermitExpiryDate { get; set; }

        [StringLength(50)]
        public string SocialSecurityNumber { get; set; }

        [StringLength(50)]
        public string Address1 { get; set; }

        [StringLength(50)]
        public string City1 { get; set; }

        [StringLength(50)]
        public string State1 { get; set; }

        [StringLength(50)]
        public string Country1 { get; set; }

        [StringLength(50)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string City2 { get; set; }

        [StringLength(50)]
        public string State2 { get; set; }

        [StringLength(50)]
        public string Country2 { get; set; } 

        //public virtual deposit_accountopening deposit_accountopening { get; set; } 
        public virtual ICollection<deposit_customersignature> deposit_customersignature { get; set; }
    }
}
