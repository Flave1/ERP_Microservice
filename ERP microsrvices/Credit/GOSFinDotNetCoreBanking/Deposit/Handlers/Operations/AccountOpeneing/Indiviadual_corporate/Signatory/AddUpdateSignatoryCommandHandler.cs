
using Deposit.Contracts.Response;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using Deposit.Managers.Interface.temp;
using Deposit.Requests;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Signatorys
{
    public class AddUpdateSignatoryCommandHandler : IRequestHandler<AddUpdateSignatoryCommand, AccountResponse<Signatory>>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _env;
        private readonly IFilesHandlerService _fileService;
        private readonly IIdentityServerRequest _serverRequest;
        public AddUpdateSignatoryCommandHandler(ILoggerService logger, DataContext dataContext, IHttpContextAccessor accessor, IWebHostEnvironment webHostEnvironment, IFilesHandlerService files, IIdentityServerRequest serverRequest)
        {
            _env = webHostEnvironment;
            _accessor = accessor;
            _dataContext = dataContext;
            _logger = logger;
            _fileService = files;
            _serverRequest = serverRequest;
        }
        public async Task<AccountResponse<Signatory>> Handle(AddUpdateSignatoryCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountResponse<Signatory>();
            try
            {
                var customer = _dataContext.deposit_customer_lite_information.Include(d => d.deposit_customer_signatories).Where(r => r.CustomerId == request.CustomerId && r.Deleted == false).FirstOrDefault();
                if (customer == null)
                {
                    response.Status.Message.FriendlyMessage = "unable to identify customer";
                    return response;
                }
                 
                var domain = customer.deposit_customer_signatories.FirstOrDefault(d => d.SignatoriesId == request.SignatoriesId);
                if (domain == null)
                    domain = new deposit_customer_signatories();
                else
                {
                    if (!string.IsNullOrEmpty(domain.SignatureFile))
                    {
                        // Path.Combine("Resources", "Images/" +)
                        var filePath = domain.SignatureFile;
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                var identificatins = await _serverRequest.GetIdentiticationTypeAsync();
                var file = _accessor.HttpContext.Request.Form.Files[0];

                var result = _fileService.SaveSingleFile(file);
                if (!result.Status.IsSuccessful)
                {
                    response.Status.Message.FriendlyMessage = result.Status.Message.FriendlyMessage;
                    return response;
                }
                domain.AccountName = request.AccountName;
                domain.SignatoriesId = request.SignatoriesId;
                domain.Surname = request.Surname;
                domain.FirstName = request.FirstName;
                domain.ClassOfSignatory = request.ClassOfSignatory;
                domain.OtherNames = request.OtherNames;
                domain.IdentificationType = request.IdentificationType;
                domain.IdentificationNumber = request.IdentificationNumber;
                domain.Telephone = request.Telephone;
                domain.SignatureFile = result.Status.Message.SearchResultMessage;
                domain.Date = request.Date;
                domain.CustomerId = request.CustomerId;
                if (domain.SignatoriesId == 0)
                    _dataContext.deposit_customer_signatories.Add(domain);
                await _dataContext.SaveChangesAsync();
  
                response.List = _dataContext.deposit_customer_signatories.Where(d => d.CustomerId == request.CustomerId && d.Deleted == false).Select(d => new Signatory(d, identificatins)).ToList();
                response.Status.Message.FriendlyMessage = "successful";
                response.Status.IsSuccessful = true;
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
