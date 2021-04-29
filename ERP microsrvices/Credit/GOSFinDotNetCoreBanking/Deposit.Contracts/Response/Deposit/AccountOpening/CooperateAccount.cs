using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Deposit.AccountOpening
{
    
    public class Create_update_corporate_information : IRequest<AccountResponse>
    { 
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
        public int CustomerTypeId { get; set; }
    }
    public class DirectorsDetailsResp
    {
        public List<Directors> Directors { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class ContactPersonsResp
    {
        public List<KeyContactPersons> KeyContactPersons { get; set; }
        public APIResponseStatus Status{ get; set; }
    }
    public   class KeyContactPersons
    {
        public int KeyContactPersonId { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OfficeAddress { get; set; }

    }


    public class AddUpdateKeyContactPersonCommand : IRequest<AccountResponse>
    {
        public long CustomerId { get; set; }
        public long KeyContactPersonId { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OfficeAddress { get; set; }
    }

    public class DetailSignatories
    {
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


    public class Directors 
    {
        public int DirectorsId { get; set; }
        public int CustomerId { get; set; }
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

    public class AddUpdateDirectorsCommand : DetailSignatories, IRequest<AccountResponse> { public int DirectorsId { get; set; } }

    public class OtherDetails
    {
        //o	Additional details 
        public string NameOfAffiliatedCompanies { get; set; }
        public string CountryOfIncorporation { get; set; }
        ///o	Accounts held with other banks
        public string NameOfBank { get; set; }
        public string AccountName { get; set; }

        public  string AccountNumber { get; set; }

        public bool AuthorityToDebitForLegalSearch { get; set; }
        //o	Document uploads
        public string NameOfDocument { get; set; }
        public IFormFile UploadFile { get; set; }
        public string UploadDate { get; set; } 
    }
}
