
using Deposit.Repository.Implement.Deposit;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.Response.Deposit.AccountOpening;

namespace Deposit.Handlers.PersonalInformations
{
    public class AddUpdatePersonalInformationCommandHandler : IRequestHandler<AddUpdatePersonalInformationCommand, AccountOpeningRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IFilesHandlerService _uploadService;
        public AddUpdatePersonalInformationCommandHandler(ILoggerService logger, 
            DataContext dataContext, 
            IHttpContextAccessor accessor,
            IFilesHandlerService filesHandlerService)
        {
            _accessor = accessor;
            _dataContext = dataContext;
            _logger = logger;
            _uploadService = filesHandlerService;
        }

       
        public async Task<AccountOpeningRegRespObj> Handle(AddUpdatePersonalInformationCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountOpeningRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                
                var domain = _dataContext.deposit_accountopening.Find(request.CustomerId);
                if (domain == null) 
                    domain = new deposit_accountopening();  

                domain.RegAddifDifffromopAdd = request.RegAddifDifffromopAdd;
                domain.AccountTypeId = request.AccountTypeId;
                domain.Category = request.Category;

                domain.ResidentialLGA = request.ResidentialLGA;
                domain.CustomerTypeId = request.CustomerTypeId;
                
                domain.CustomerId = request.CustomerId;

                domain.IsAddedToDepositForm = request.IsAddedToDepositForm;
                domain.IsBankClosureDone = request.IsBankClosureDone;

                domain.EmploymentType = request.EmploymentType;
                domain.BusinessName = request.BusinessName; 
                domain.BusinessAddress = request.BusinessAddress;
                domain.BusinessState = request.BusinessState; 
                domain.JobTitle = request.BusinessAddress;
                domain.Other = request.Other; 
                domain.OtherComment = request.OtherComment;
                domain.PermitExpiryDate = request.PermitExpiryDate;
                domain.RelationshipOfficerId = request.RelationshipOfficerId;
                
                if (request.CustomerTypeId == (int)CustomerType.Individual)
                {
                    //personal details 
                    domain.Title = request.Title;
                    domain.Surname = request.Surname;
                    domain.Firstname = request.Firstname;
                    domain.Othername = request.Othername;
                    domain.MaritalStatusId = request.MaritalStatusId;
                    domain.GenderId = request.GenderId;
                    domain.DOB = request.DOB;
                    domain.MotherMaidenName = request.MotherMaidenName;
                    domain.TaxIDNumber = request.TaxIDNumber;
                    domain.BVN = request.BVN;
                    domain.ResidentPermitNumber = request.ResidentPermitNumber;
                    domain.PermitIssueDate = request.PermitIssueDate;
                    domain.SocialSecurityNumber = request.SocialSecurityNumber;
                    domain.StateOfOrigin = request.StateOfOrigin;
                    domain.Nationality = request.Nationality;
                    domain.City = request.City;
                    domain.StateId = request.StateId;


                    //contact address
                    domain.ResidentAddress1 = request.ResidentAddress1;
                    domain.ResidentAddress2 = request.ResidentAddress2;
                    domain.ResidentCountryId = request.ResidentCountryId;
                    domain.ResidentOfCountry = request.ResidentOfCountry; 
                    domain.Email = request.Email;
                    domain.MailingAddress = request.MailingAddress;
                    domain.MobileNumber = request.MobileNumber;
                    domain.PhoneNumber = request.PhoneNumber;
                    domain.ResidentialState = request.ResidentialState;
                    domain.ResidentialCity = request.ResidentialCity;
                    
                }
                domain.LocalGovernment = request.LocalGovernment;

                if (request.CustomerTypeId == (int)CustomerType.Corporate)
                {
                    //personal details 
                    domain.CertOfIncorporationNumber = request.CertOfIncorporationNumber;
                    domain.DateOfIncorporation = DateTime.Parse(request.DateOfIncorporation);
                    domain.JurisdictionOfincorporatoin = request.JurisdictionOfincorporatoin;
                    domain.NatureOfBusiness = request.NatureOfBusiness; 
                    domain.SectorOrIndustry = request.SectorOrIndustry;
                    domain.OperatingAdress1 = request.OperatingAdress1;
                    domain.OperatingAdress2= request.OperatingAdress2;
                    domain.ResidentCountryId = request.ResidentCountryId;
                    domain.StateId = request.StateId;
                    domain.Email = request.Email;
                    domain.Website = request.Website;
                    domain.MobileNumber = request.MobileNumber;
                    domain.PhoneNumber = request.PhoneNumber;
                    domain.TaxIDNumber = request.TaxIDNumber;
                    domain.SCUML = request.SCUML; 
                    domain.CompanyName = request.CompanyName;

                    //estimated turnover
                    domain.EstimatedAnnualRevenue = request.EstimatedAnnualRevenue;
                    domain.IsYourCompanyQuotedOnTheStockExchange = request.IsYourCompanyQuotedOnTheStockExchange;
                    domain.StockExchange = request.StockExchange; 
                }

                if (domain.CustomerId > 0)
                    _dataContext.Entry(domain).CurrentValues.SetValues(domain);
                else
                    _dataContext.deposit_accountopening.Add(domain);
                await _dataContext.SaveChangesAsync();

                 
                response.CustomerId = domain.CustomerId;
                response.Status.Message.FriendlyMessage = "successful";
                return response;
            }
            catch (Exception e)
            {
                response.Status.IsSuccessful = false;
                response.Status.Message.FriendlyMessage = e?.Message ?? e.InnerException?.Message;
                response.Status.Message.TechnicalMessage = e.ToString();
                return response;
            }
        }
    }
}
