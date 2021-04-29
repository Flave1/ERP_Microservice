using Deposit.Contracts.Response.Auth;

using Deposit.Contracts.Response.Mail;
using Deposit.DomainObjects.Auth;


using Deposit.Requests;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_Financial_Identity;
using GOSLibraries.URI;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Auths
{
    public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, AuthResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IBaseURIs _uRIs;
        private readonly IIdentityServerRequest _serverRequest;
        public RegistrationCommandHandler(
            UserManager<ApplicationUser> userManager,
            ILoggerService loggerService,
            DataContext dataContext,
            IBaseURIs uRIs,
            IIdentityServerRequest serverRequest)
        {
            _userManager = userManager;
            _logger = loggerService;
            _uRIs = uRIs;
            _dataContext = dataContext;
            _serverRequest = serverRequest;
        }
        public async Task<AuthResponse> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var response = new AuthResponse { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
            try
            {
                //var accountNumber = GeneralHelpers.GenerateRandomDigitCode(10);
                //var user = new ApplicationUser
                //{ 
                //    Email = request.Email,
                //    UserName = request.Email,
                //    PhoneNumber = request.PhoneNo,
                //    FirstName = request.FirstName,
                //    LastName = request.LastName,
                //    Address = request.Address,
                //    CustomerTypeId = request.CustomerTypeId,
                //    ApprovalStatusId = 1,
                //    SecurityAnswered = request.SecurityAnswered,
                //    QuestionId = request.QuestionId
                //};

                //var customer = new credit_loancustomer
                //{
                //    Email = request.Email,
                //    UserIdentity = user.Id,
                //    PhoneNo = request.PhoneNo,
                //    FirstName = request.FirstName,
                //    LastName = request.LastName,
                //    Address = request.Address,
                //    CustomerTypeId = request.CustomerTypeId,
                //    CASAAccountNumber = accountNumber,
                //    ApprovalStatusId = 1,
                //    ProfileStatus = 0,
                //    Active = true,
                //    Deleted = false,
                //    CreatedBy = user.Email,
                //    CreatedOn = DateTime.Now,
                //    RegistrationSource = "Website"
                //};
                //_dataContext.credit_loancustomer.Add(customer);                             
                //_dataContext.SaveChanges();
                //updateCASA(customer, customer.CustomerId);
                //updateDepositcustomer(customer);



                //var createdUser = await _userManager.CreateAsync(user, request.Password);

                //if (!createdUser.Succeeded)
                //{
                //    response.Status.Message.FriendlyMessage = createdUser.Errors.Select(x => x.Description).FirstOrDefault();
                //    return response;
                //}

                //response.Status.IsSuccessful = true;
                //response.Status.Message.FriendlyMessage = "Confirmation email has just been sent to your email";
                ////var loginResponse = await _identityService.LoginAsync(user);

                //await SendMailToNewSuppliersAsync(user);
                return response;
            }
            catch (Exception ex)
            {
                return response;
            }
        }

        //private async Task SendMailToNewSuppliersAsync(ApplicationUser user)
        //{

        //    var accountId = user.Id;
        //    var name = user.FirstName;
        //    var path = $"{_uRIs.SelfClient}/#/auth/login";
             
        //    var content1 = "Welcome to GOS Credit! There's just one step before you get to complete your customer account registration. Verify you have the right email address by clicking on the button below.";
        //    var content2 = "Once your account creation is completed, your can explore our services and have a seamless experience.";
        //    var body = GeneralHelpers.MailBody(name, path, content1, content2);

        //    var addresses = new ToAddress
        //    {
        //        name = user.FirstName + " " + user.LastName,
        //        address = user.Email
        //    };
        //    var addressList = new List<ToAddress> { addresses };
        //    var mailObj = new MailObj
        //    {
        //        subject = "Email Verification",
        //        content = body,
        //        toAddresses = addressList,
        //        fromAddresses = new List<FromAddress> { },
        //        sendIt = true,
        //        saveIt = false
        //    };

        //    var res = await _serverRequest.SendMail(mailObj); 
        //}

        //private void updateCASA(credit_loancustomer entity, int customerId)
        //{
        //    decimal bal = _dataContext.deposit_accountsetup.Where(x => x.DepositAccountId == 3).FirstOrDefault().InitialDeposit;
        //    decimal Ledgerbal = _dataContext.deposit_accountsetup.Where(x => x.DepositAccountId == 3).FirstOrDefault().InitialDeposit;
        //    decimal? irate = _dataContext.deposit_accountsetup.Where(x => x.DepositAccountId == 3).FirstOrDefault().InterestRate;
        //    decimal lienBal = _dataContext.deposit_accountsetup.Where(x => x.DepositAccountId == 3).FirstOrDefault().InitialDeposit;

        //    credit_casa casaAccount = new credit_casa();
        //    casaAccount = _dataContext.credit_casa.Where(x => x.AccountNumber == entity.CASAAccountNumber).FirstOrDefault();
        //    if (casaAccount == null)
        //    {
        //        var customerAccount = new credit_casa
        //        {
        //            AccountName = entity.FirstName + " " + entity.LastName,
        //            AccountNumber = entity.CASAAccountNumber,
        //            AccountStatusId = (int)CASAAccountStatusEnum.Inactive,
        //            //ActionBy = entity.actionBy,
        //            ActionDate = DateTime.Now,
        //            AprovalStatusId = (int)ApprovalStatus.Pending,
        //            AvailableBalance = bal,
        //            BranchId = 1,
        //            CompanyId = 7,
        //            CurrencyId = 1,
        //            CustomerId = customerId,
        //            //CustomerSensitivityLevelId = entity.customerSensitivityLevelId,
        //            EffectiveDate = DateTime.Now,
        //            HasLien = true,
        //            HasOverdraft = true,
        //            InterestRate = irate,
        //            IsCurrentAccount = true,
        //            LedgerBalance = Ledgerbal,
        //            LienAmount = lienBal,
        //            //MISCode = "",
        //            OperationId = (int)OperationsEnum.CasaAccountApproval,
        //            //OverdraftAmount = 0,
        //            //OverdraftExpiryDate = entity.overdraftExpiryDate,
        //            //OverdraftInterestRate = 0,
        //            //PostNoStatusId = entity.postNoStatusId,
        //            ProductId = 1,
        //            RelationshipManagerId = 0,
        //            RelationshipOfficerId = 0,
        //            //TEAMMISCode = "",
        //            FromDeposit = false,
        //            //Tenor = entity.tenor,
        //            //TerminalDate = entity.terminalDate,
        //            Active = true,
        //            Deleted = false,
        //            CreatedBy = entity.CreatedBy,
        //            CreatedOn = DateTime.Now,
        //        };
        //        _dataContext.credit_casa.Add(customerAccount);
        //        _dataContext.SaveChanges();
        //    }
        //}

        //private void updateDepositcustomer(credit_loancustomer entity)
        //{
        //    deposit_accountopening casaAccount = new deposit_accountopening();
        //    casaAccount = _dataContext.deposit_accountopening.Where(x => x.AccountNumber == entity.CASAAccountNumber).FirstOrDefault();
        //    if (casaAccount == null)
        //    {
        //        var CustomerObj = new deposit_accountopening
        //        {
        //            CustomerTypeId = entity.CustomerTypeId,
        //            AccountTypeId = 3,
        //            AccountNumber = entity.CASAAccountNumber,
        //            Title = entity.TitleId,
        //            Surname = entity.LastName,
        //            Firstname = entity.FirstName,
        //            Othername = entity.MiddleName,
        //            MaritalStatusId = entity.MaritalStatusId,
        //            //RelationshipOfficerId = entity.RelationshipOfficerId,
        //            GenderId = entity.GenderId,
        //            BirthCountryId = entity.CountryId,
        //            //DOB = entity.Dob,
        //            //Address1 = entity.Address,
        //            //City = entity.Address,
        //            //CountryId = entity.CountryId,
        //            Email = entity.Email,
        //            MobileNumber = entity.PhoneNo,
        //            Active = true,
        //            Deleted = false,
        //            CreatedBy = entity.CreatedBy,
        //            CreatedOn = DateTime.Now,
        //        };
        //        _dataContext.deposit_accountopening.Add(CustomerObj);
        //        _dataContext.SaveChanges();
        //    }

        //}
    }
}

