using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Deposit.Contracts.Response.Deposit.AccountOpening
{

    public class Individual_customer_response
    {
        public Create_update_individual_personal_information Individual_personal_information { get; set; }
        public Create_update_individual_contact_dteails Individual_contact_dteails { get; set; }
        public Create_emplyment_details Emplyment_details { get; set; }
        public AddUpdateNextOfKinCommand NextOfKinCommand { get; set; }
    }

    public class Create_update_individual_personal_information : IRequest<AccountResponse>
    {
        public long CustomerId { get; set; }
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
        public int? BirthCountryId { get; set; }
        public DateTime? DOB { get; set; }
        [StringLength(50)]
        public string MotherMaidenName { get; set; }
        [StringLength(200)]
        public string TaxIDNumber { get; set; }
        [StringLength(100)]
        public string BVN { get; set; }
        public int? Nationality { get; set; }
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
        public int ResidentialState { get; set; }
        public int ResidentialCity { get; set; }
        public int CustomerTypeId { get; set; }
        public int IndividualCustomerId { get; set; }
    }


    public class Create_update_individual_contact_dteails : IRequest<AccountResponse> 
    {
        public long ContactDetailId { get; set; }
        public long CustomerId { get; set; }
        public string ResidentialAddressLine1 { get; set; }
        public string ResidentialAddressLine2 { get; set; }
        public string ResidentialCity { get; set; }
        public long ResidentialCountry { get; set; }
        public long ResidentialState { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string MailngAddress { get; set; }
    }

    public class Create_emplyment_details : IRequest<AccountResponse>
    {
        public long EmploymentDetailId { get; set; }
        public bool IsEmployed { get; set; }
        public string EmployerName { get; set; }
        public string EmployerAddress { get; set; }
        public int? EmployerState { get; set; }
        public string Occupation { get; set; }
        public bool IsSelfEmployed { get; set; }
        public bool IsUnEmployed { get; set; }
        public bool IsRetired { get; set; }
        public bool IsStudent { get; set; }
        public string OtherComments { get; set; }
        public long CustomerId { get; set; }
    }
     
    public class AddUpdateNextOfKinCommand : IRequest<AccountResponse>
    {
        //Next of kin
        public long NextOfKinId { get; set; }
        public long CustomerId { get; set; }
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
         
    }


     
    public class AddUpdateIdentificationCommand : IRequest<AccountResponse>
    {
        public long IdentificationId { get; set; }
        public long CustomerId { get; set; }
        public int Identification { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime? DateIssued { get; set; }
        public DateTime? ExpiryDate { get; set; }
          
    }

    public class IdentificationObj
    {
        public int IdentificationId { get; set; }
        public int CustomerId { get; set; }
        public int Identification { get; set; }
        public string IdentificationName { get; set; }
        public string IDNumber { get; set; }
        public DateTime? DateIssued { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public IdentificationObj()
        {

        }
    }

    public class IdentificationResp
    {
        public List<IdentificationObj> Identification { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class DepositSignature
    {
        public int SignatureId { get; set; }
        public int IdentificationId { get; set; }
        public string SignatureOrMarkName { get; set; }
        public IFormFile FormFile { get; set; }
        public byte[] SignatureOrMarkUpload { get; set; }
    }
}
