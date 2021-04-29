using Deposit.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GODP.Entities.Models
{
    public class deposit_customer_lite_information: GeneralEntity
    {
        public deposit_customer_lite_information()
        {
            deposit_customer_account_information = new HashSet<deposit_customer_account_information>();
            Deposit_customer_thumbs = new HashSet<Deposit_customer_thumbs>();
            deposit_customer_signatories = new HashSet<deposit_customer_signatories>(); 
        }
        [Key]
        public long CustomerId { get; set; }
        public int CustomerTypeId { get; set; } 
        [ForeignKey("KycId")]
        public deposit_customer_kyc deposit_customer_kyc { get; set; }
        public long IndividualCustomerId { get; set; }
        [ForeignKey("IndividualCustomerId")]
        public deposit_individual_customer_information deposit_individual_customer_information { get; set; }
        public virtual ICollection<deposit_customer_account_information> deposit_customer_account_information { get; set; }
        public virtual ICollection<Deposit_customer_thumbs> Deposit_customer_thumbs { get; set; } 
        public virtual ICollection<deposit_customer_signatories> deposit_customer_signatories { get; set; }
    }
   
    public class deposit_customer_account_information : GeneralEntity
    {
        [Key]
        public long AccountInformationId { get; set; }
        public string AccountNumber { get; set; }
        public DateTime Date_to_go_dormant { get; set; }
        public string AvailableBalance { get; set; } 
        public bool InternetBanking { get; set; } 
        public bool EmailStatement { get; set; } 
        public bool Card { get; set; } 
        public bool SmsAlert { get; set; } 
        public bool EmailAlert { get; set; } 
        public bool Token { get; set; }
        public int AccountTypeId { get; set; }
        public string Currencies { get; set; }
        public int CategoryId { get; set; }
        public int CustomerTypeId { get; set; } 
        public int? RelationshipOfficerId { get; set; }
        [ForeignKey("AccountTypeId")]
        public deposit_accountype deposit_accountype { get; set; }
        public long CustomerId { get; set; }
        [ForeignKey("CategoryId")]
        public deposit_category deposit_category { get; set; }
        [ForeignKey("CustomerId")]
        public deposit_customer_lite_information deposit_customer_lite_information { get; set; } = new deposit_customer_lite_information();

    }
    
    public  class deposit_individual_customer_information : GeneralEntity
    {
        public deposit_individual_customer_information()
        { 
            deposit_customerIdentifications = new HashSet<deposit_customerIdentifications>();
        }
        [Key]
        public long IndividualCustomerId { get; set; }
        public int? Title { get; set; }
        public string Email { get; set; }

        [StringLength(100)]
        public string Surname { get; set; }

        [StringLength(100)]
        public string Firstname { get; set; }

        [StringLength(50)]
        public string Othername { get; set; } 
        public int? MaritalStatusId { get; set; }
        public int? GenderId { get; set; }
        public long? BirthCountryId { get; set; }
        public DateTime? DOB { get; set; }
        [StringLength(50)]
        public string MotherMaidenName { get; set; }
        [StringLength(200)]
        public string TaxIDNumber { get; set; }
        [StringLength(100)]
        public string BVN { get; set; }
        public long? Nationality { get; set; }
        [StringLength(50)]
        public string ResidentPermitNumber { get; set; }
        public DateTime? PermitIssueDate { get; set; }
        public DateTime? PermitExpiryDate { get; set; }
        [StringLength(100)]
        public string SocialSecurityNumber { get; set; }
        public long StateOfOrigin { get; set; }
        public string LGA_of_origin { get; set; }
        public string PhoneNo { get; set; } 
        public string ResidentialLGA { get; set; }
        public long ResidentialState { get; set; }
        public long ResidentialCity { get; set; }
        public long ContactDetailId { get; set; } 
        public long CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public deposit_customer_lite_information deposit_customer_lite_information { get; set; }   
        public virtual ICollection<deposit_customerIdentifications> deposit_customerIdentifications { get; set; }
    } 
    
    public class deposit_corporate_customer_information : GeneralEntity
    {

        [Key]
        public long CorporateCustomerId { get; set; } 
        public string CompanyName { get; set; } 
        public string CertOfIncorporationNumber { get; set; }
        public DateTime? DateOfIncorporation { get; set; }
        public string JurisdictionOfincorporatoin { get; set; }
        public string NatureOfBusiness { get; set; }
        public string SectorOrIndustry { get; set; } 
        public string OperatingAdress1 { get; set; }
        public string OperatingAdress2 { get; set; }
        public string RegisteredAddress { get; set; }
        public string LGA { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string MobileNumber { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public string SCUML { get; set; } 
        public long CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public deposit_customer_lite_information deposit_customer_lite_information { get; set; }


        ///o	Estimated Turnover

        //public string EstimatedAnnualRevenue { get; set; }
        //public bool IsYourCompanyQuotedOnTheStockExchange { get; set; }
        //public string StockExchange { get; set; }


        ///employment details


        //public bool IsAddedToDepositForm { get; set; }
        //public bool IsBankClosureDone { get; set; }

        //public int? EmploymentType { get; set; }

        //[StringLength(50)]
        //public string BusinessName { get; set; }

        //[StringLength(50)]
        //public string BusinessAddress { get; set; }

        //[StringLength(50)]
        //public string BusinessState { get; set; }

        //[StringLength(50)]
        //public string JobTitle { get; set; }

        //[StringLength(50)]
        //public string Other { get; set; }
        //public string OtherComment { get; set; }
    }

    public class deposit_customer_contact_detail : GeneralEntity
    {
        [Key]
        public long ContactDetailId { get; set; }
        public string ResidentialAddressLine1 { get; set; }
        public string ResidentialAddressLine2 { get; set; }
        public string ResidentialCity { get; set; }
        public long ResidentialCountry { get; set; }
        public long ResidentialState { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string MailngAddress { get; set; } 
        public long IndividualCustomerId { get; set; }
        [ForeignKey("IndividualCustomerId")]
        public deposit_individual_customer_information deposit_individual_customer_information { get; set; }

    }
     
    public class deposit_customerIdentifications : GeneralEntity
    {
        [Key]
        public long IdentificationId { get; set; }
        public long CustomerId { get; set; }
        public int Identification { get; set; }
        public string IDNumber { get; set; }
        public DateTime? DateIssued { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public long IndividualCustomerId { get; set; }
        [ForeignKey("IndividualCustomerId")]
        public deposit_individual_customer_information deposit_individual_customer_information { get; set; }  
    }

    public class deposit_employer_details : GeneralEntity
    {
        [Key]
        public long EmploymentDetailId { get; set; }
        public bool IsEmployed { get; set; }
        public bool IsSelfEmployed { get; set; }
        public bool IsUnEmployed { get; set; }
        public bool IsRetired { get; set; }
        public bool IsStudent { get; set; }
        public string OtherComments { get; set; }
        public string EmployerName { get; set; }
        public string EmployerAddress { get; set; }
        public int? EmployerState { get; set; }
        public string Occupation { get; set; } 
        public long IndividualCustomerId { get; set; }
        [ForeignKey("IndividualCustomerId")]
        public deposit_individual_customer_information deposit_individual_customer_information { get; set; }
    }

    public class deposit_nextofkin : GeneralEntity
    {
        [Key]
        public long NextOfKinId { get; set; }
        public string NextOfKinTitle { get; set; }
        public string NextOfKinSurname { get; set; }
        public string NextOfKinFirstName { get; set; }
        public string NextOfKinOtherNames { get; set; }
        public string NextOfKinDateOfBirth { get; set; }
        public string NextOfKinGender { get; set; }
        public string NextOfKinRelationship { get; set; }
        public string NextOfKinMobileNumber { get; set; }
        public string NextOfKinEmailAddress { get; set; }
        public string NextOfKinAddress { get; set; }
        public string NextOfKinCity { get; set; }
        public int NextOfKinState { get; set; }
        public long IndividualCustomerId { get; set; }
        [ForeignKey("IndividualCustomerId")]
        public deposit_individual_customer_information deposit_individual_customer_information { get; set; }
    }

    public class Deposit_customer_thumbs
    {
        [Key]
        public long ThumbId { get; set; }
        public string FileName { get; set; }
        public string Extention { get; set; }
        public string FilePath { get; set; }
        public string Type { get; set; }
        public long CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public deposit_customer_lite_information deposit_customer_lite_information { get; set; }
    }

     
    public class deposit_customer_signatories : GeneralEntity
    {
        [Key]
        public long SignatoriesId { get; set; }
        public long CustomerId { get; set; }
        public string AccountName { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public int ClassOfSignatory { get; set; }
        public int IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string Telephone { get; set; }
        public string SignatureFile { get; set; }
        public DateTime Date { get; set; } 
        [ForeignKey("CustomerId")]
        public deposit_customer_lite_information deposit_customer_lite_information { get; set; }
    }
     
    public enum SignatoryType
    {
        Signatory = 1,
        OtherCorporateSignatory
    }
    public class deposit_signatures : GeneralEntity
    {
        [Key]
        public long SignatureId { get; set; }
        public int IdentificationId { get; set; }
        public string SignatureOrMarkName { get; set; }
        public byte[] SignatureOrMarkUpload { get; set; }
        public string Extension { get; set; }
        public deposit_customerIdentifications Deposit_CustomerIdentification { get; set; }
    }
    public enum Employmenttype
    {
        Employed = 1,
        SelfEmployed,
        UnEmployed,
        Retired, 
        Student,
        Others

    }

    public class deposit_customer_kyc : GeneralEntity
    {
        [Key]
        public long kycId { get; set; } 
        public long CustomerId { get; set; }
        public bool SociallyOrFinanciallyDisadvantaged { get; set; }
        public string OtherDocumentsObtained { get; set; }
        public bool DoesTheCustomerEnjoyTieredKYC { get; set; }
        public string RiskCategory { get; set; }

        public bool IsCustomerPoliticalyExposed { get; set; }
        public string PoliticalyExposedDetails { get; set; }

        public string AddressVisited { get; set; }
        public string CommentOnLocation { get; set; }
        public string Location_ColorOfbuilding { get; set; }
        public string Location_DescriptionOfBuilding { get; set; }
        public string FullNameOfVisitingStaff { get; set; }
        public DateTime DateOfVisitation { get; set; }

        public bool isUtilityBillSubmitted { get; set; }
        public bool DulyCompletedAccountOpenningForm { get; set; }
        public bool RecentPassportPhotograph { get; set; }
        public bool Confirmed { get; set; }
        public string Confirmaiotnname { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime DeferralDate { get; set; }
        public string NameOfDocument { get; set; }
        public DateTime DocumentUploadDate { get; set; }
        public string DocumentPath { get; set; }
        public string DeferralFullName { get; set; }
        [ForeignKey("CustomerId")]
        public deposit_customer_lite_information deposit_customer_lite_information { get; set; }
    }
 
  

    public class deposit_directors : GeneralEntity
    {
        [Key]
        public long DirectorsId { get; set; }
        public long CustomerId { get; set; }
        public string AccountName { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string MaritalStatus { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string POB { get; set; }
        public string MotherMaidienName { get; set; }
        public string NameOfNextOfKin { get; set; }
        public string LGA { get; set; }
        public string State { get; set; }
        public string TaxIdentitfication { get; set; }
        public int MeansOfIdentification { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime IDIssuedate { get; set; }
        public string Occupation { get; set; }
        public string JobTitle { get; set; }
        public string Position { get; set; }
        public string Nationality { get; set; }
        public string ResidentPermitNumber { get; set; }
        public string PermitIssueDate { get; set; }
        public string PermitExpiryDate { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string ResidentialLGA { get; set; }
        public string ResidentialCity { get; set; }
        public string ResidentialState { get; set; }
        public bool MailingAddressSameWithResidentialAddress { get; set; }
        public string MailingLGA { get; set; }
        public string MailingCity { get; set; }
        public string MailingState { get; set; }
        public string MobileNumber { get; set; }

        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string SignatureName { get; set; }
        public string SignatureFile { get; set; }
    }
  
}
