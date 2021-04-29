
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Signatorys
{
    public class AddUpdateSignatoryCommandHandler : IRequestHandler<AddUpdateSignatoryCommand, AccountOpeningRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _env;
        public AddUpdateSignatoryCommandHandler(ILoggerService logger, DataContext dataContext, IHttpContextAccessor accessor, IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
            _accessor = accessor;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<AccountOpeningRegRespObj> Handle(AddUpdateSignatoryCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountOpeningRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var domain = _dataContext.deposit_signatories.Find(request.SignatoriesId);
                if (domain == null)
                    domain = new deposit_signatories();


                var files = _accessor.HttpContext.Request.Form.Files;
                if (files[0].Length > 0)
                {
                    if (files[0].FileName.Split('.').Length > 2)
                    {
                        response.Status.Message.FriendlyMessage = "Invalid Character detected in file Name";
                        return response;
                    }

                    //using (var ms = new MemoryStream())
                    //{
                    //    files[0].CopyTo(ms);
                    //    domain.SignatureFile = ms.ToArray();
                    //}


                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var guid = Guid.NewGuid();
                    var fileName = $"{guid}-{DateTime.Now.ToString().Split(" ")[1].Replace(':', '-')}";
                    var type = files[0].ContentType;



                    var fullPath = _env.WebRootPath + "/Resources/" + fileName;
                    var dbPath = _env.WebRootPath + "/Resources/" + fileName;

                    using (FileStream filestrem = System.IO.File.Create(fullPath))
                    {
                        await files[0].CopyToAsync(filestrem);
                        await filestrem.FlushAsync();
                    }

                    domain.Signature_Full_Path = fullPath;
                    domain.SignatureName = "/Resources/" + fileName;
                    domain.SignatureUploadPath = dbPath;
                    domain.SignatureUploadType = type;
                    domain.Extention = files[0].FileName.Split('.')[1];

                }

                if (request.IsDeclaration)
                {
                    var current_customer_signatories = _dataContext.deposit_signatories.Where(r => r.CustomerId == request.CustomerId && r.Deleted == false).ToList();
                    if(current_customer_signatories.Count() > 0)
                    {
                        foreach(var item in current_customer_signatories)
                        {
                            item.SoleSignatory = request.SoleSignatory;
                            item.Number_of_to_sign = request.Number_of_to_sign;
                            item.DeclarationName = request.DeclarationName; 
                            domain.DeclarationDate = DateTime.UtcNow;
                        }

                        await _dataContext.SaveChangesAsync();
                        response.CustomerId = domain.CustomerId;
                        response.Status.Message.FriendlyMessage = "successful";
                        return response;
                    }
                }

               
                domain.SignatureType = request.SignatureType;
                domain.AccountName = request.AccountName;
                domain.ClassOfSignatory = request.ClassOfSignatory;
                domain.CustomerId = request.CustomerId;
                domain.Date = DateTime.UtcNow;
                domain.DeclarationName = request.DeclarationName;
                domain.DOB = request.DOB;
                domain.EmailAddress = request.EmailAddress;
                domain.FirstName = request.FirstName;
                domain.Gender = request.Gender;
                domain.IdentificationNumber = request.IdentificationNumber;
                domain.IdentificationType = request.IdentificationType;
                domain.IDIssuedate = request.IDIssuedate;
                domain.LGA = request.LGA;
                domain.MailingAddressSameWithResidentialAddress = request.MailingAddressSameWithResidentialAddress;
                domain.MailingCity = request.MailingCity;
                domain.MailingLGA = request.MailingLGA;
                domain.MailingState = request.MailingState;
                domain.MaritalStatus = request.MaritalStatus;
                domain.MeansOfIdentification = request.MeansOfIdentification;
                domain.MobileNumber = request.MobileNumber;
                domain.MotherMaidienName = request.MotherMaidienName;
                domain.NameOfNextOfKin = request.NameOfNextOfKin;
                domain.Nationality = request.Nationality;
                domain.Occupation = request.Occupation;
                domain.OtherNames = request.OtherNames;
                domain.PermitExpiryDate = request.PermitExpiryDate;
                domain.PermitIssueDate = request.PermitIssueDate;
                domain.POB = request.POB;
                domain.Position = request.Position;
                domain.ResidentialCity = request.ResidentialCity;
                domain.ResidentialLGA = request.ResidentialLGA;
                domain.ResidentialState = request.ResidentialState;
                domain.ResidentPermitNumber = request.ResidentPermitNumber;
                domain.SignatoriesId = request.SignatoriesId;
                domain.SocialSecurityNumber = request.SocialSecurityNumber;
                domain.State = request.State;
                domain.Surname = request.Surname;
                domain.TaxIdentitfication = request.TaxIdentitfication;
                domain.Telephone = request.Telephone;
                

                if (domain.SignatoriesId > 0)
                    _dataContext.Entry(domain).CurrentValues.SetValues(domain);
                else
                    _dataContext.deposit_signatories.Add(domain);
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
