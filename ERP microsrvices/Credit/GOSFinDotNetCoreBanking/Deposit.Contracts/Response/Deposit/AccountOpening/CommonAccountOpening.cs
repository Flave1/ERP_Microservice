using Deposit.Contracts.Response.Common;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deposit.Contracts.Response.Deposit.AccountOpening
{

    public class GetCustomerQueryRepsonse
    {
        public GetCustomerQueryRepsonse() { }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerTypeId { get; set; }
        public string AccountNumber { get; set; }
        public string Email { get; set; }
        public List<CustomerThumbs> CustomerThumbs { get; set; } = new List<CustomerThumbs>();
        public List<AccountInformation> AccountInformation { get; set; } = new List<AccountInformation>();
        public List<KYC> KYC { get; set; } = new List<KYC>();
        public List<Signatory> Signatories { get; set; } = new List<Signatory>();
        public GetCustomerQueryRepsonse(deposit_individual_customer_information db)
        {
            CustomerId = db.CustomerId;
            CustomerTypeId = db.deposit_customer_lite_information.CustomerTypeId;
            CustomerName = db.Firstname + " " + db.Surname;
            AccountNumber = string.Join(", ", db.deposit_customer_lite_information.deposit_customer_account_information.Select(e => e?.AccountNumber));
            Email = db.Email;
        }
    }

    public class Add_update_thump_printCommand : IRequest<AccountResponse<CustomerThumbs>>
    {
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }

    public class CustomerThumbs
    {
        public CustomerThumbs() { }
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string File { get; set; }
        public CustomerThumbs(Deposit_customer_thumbs db)
        {
            CustomerId = db.CustomerId;
            Name = db.FileName;
            File = db.FilePath;
        }
    }
    
    public class AccountInformation
    {
        public int AccountdetailId { get; set; }
        public long CustomerId { get; set; } 
        public bool? InternetBanking { get; set; } 
        public bool? EmailStatement { get; set; } 
        public bool? Card { get; set; } 
        public bool? SmsAlert { get; set; } 
        public bool? EmailAlert { get; set; } 
        public bool? Token { get; set; }
        public int AccountTypeId { get; set; }
        public int[] CurrencyArray { get; set; }
        public string Currencies { get; set; }
        public int AccountCategoryId { get; set; }
    }
    
    public class Create_update_account_informationCommand : IRequest<AccountResponse>
    {
        public long AccountInformationId { get; set; }
        public long CustomerId { get; set; }

        public bool InternetBanking { get; set; }

        public bool EmailStatement { get; set; }

        public bool Card { get; set; }

        public bool SmsAlert { get; set; }

        public bool EmailAlert { get; set; }

        public bool Token { get; set; }
        public int AccountTypeId { get; set; }
        public int[] Currencies { get; set; }
        public int CategoryId { get; set; }
        public int CustomerTypeId { get; set; }
        public int? RelationshipOfficerId { get; set; }
    }

    public class AddUpdateKYCCommand : IRequest<AccountResponse<KYC>>
    {
        public long kycId { get; set; }
        public long CustomerId { get; set; }
        public bool Financiallydisadvantaged { get; set; }
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
        public string DeferralFullName { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime DocumentUploadDate { get; set; }
        public string DocumentPath { get; set; }
        public string NameOfDocument { get; set; }
    }

    public class KYC
    {
        public KYC() { }
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
        public string DeferralFullName { get; set; }
        public string NameOfDocument { get; set; }
        public DateTime DocumentUploadDate { get; set; }
        public string DocumentPath { get; set; }

        public KYC(deposit_customer_kyc domain)
        {
            CustomerId = domain.CustomerId;
            kycId = domain.kycId;
            AddressVisited = domain.AddressVisited;
            CommentOnLocation = domain.CommentOnLocation;
            Confirmaiotnname = domain.Confirmaiotnname;
            ConfirmationDate = domain.ConfirmationDate;
            Confirmed = domain.Confirmed;
            DateOfVisitation = domain.DateOfVisitation;
            DoesTheCustomerEnjoyTieredKYC = domain.DoesTheCustomerEnjoyTieredKYC;
            DulyCompletedAccountOpenningForm = domain.DulyCompletedAccountOpenningForm;
            FullNameOfVisitingStaff = domain.FullNameOfVisitingStaff;
            IsCustomerPoliticalyExposed = domain.IsCustomerPoliticalyExposed;
            isUtilityBillSubmitted = domain.isUtilityBillSubmitted;
            Location_ColorOfbuilding = domain.Location_ColorOfbuilding;
            Location_DescriptionOfBuilding = domain.Location_DescriptionOfBuilding;
            OtherDocumentsObtained = domain.OtherDocumentsObtained;
            PoliticalyExposedDetails = domain.PoliticalyExposedDetails;
            RecentPassportPhotograph = domain.RecentPassportPhotograph;
            RiskCategory = domain.RiskCategory;
            SociallyOrFinanciallyDisadvantaged = domain.SociallyOrFinanciallyDisadvantaged;
            DeferralFullName = domain.DeferralFullName;
            DeferralDate = domain.ConfirmationDate;
            NameOfDocument = domain.NameOfDocument;
            DocumentUploadDate = domain.DocumentUploadDate;
            DocumentPath = domain.DocumentPath;
        }
    }
 

    public class Signatory
    {
        public Signatory() { }
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

        public int SignatureType { get; set; }
        public DateTime Date { get; set; }
        public string IdentificationTypeName { get; set; }
        public string SignatoryClassName { get; set; }

        public Signatory(deposit_customer_signatories domain, CommonRespObj common)
        {

            AccountName = domain.AccountName;
            SignatoriesId = domain.SignatoriesId;
            Surname = domain.Surname;
            FirstName = domain.FirstName;
            ClassOfSignatory = domain.ClassOfSignatory;
            OtherNames = domain.OtherNames;
            IdentificationType = domain.IdentificationType;
            IdentificationNumber = domain.IdentificationNumber;
            Telephone = domain.Telephone;
            SignatureFile = domain.SignatureFile;
            Date = domain.Date;
            CustomerId = domain.CustomerId;
            IdentificationTypeName = common.commonLookups.FirstOrDefault(e => e.LookupId == domain.SignatoriesId)?.LookupName;
        }

    }

    public class AddUpdateSignatoryCommand : IRequest<AccountResponse<Signatory>>
    {
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
        public IFormFile SignatureFile { get; set; }
        public DateTime Date { get; set; }


    }

    public class AccountResponse
    {
        public AccountResponse()
        {
            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() };

        }
        public long CustomerId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

}
