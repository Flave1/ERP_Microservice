using Deposit.Contracts.GeneralExtension;
using Deposit.Contracts.Response;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using Deposit.Managers.Interface;
using Deposit.Managers.Interface.temp;
using Deposit.Repository.Implement.Deposit;
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

namespace Deposit.Handlers.AccountInformations
{
    public class Add_update_thump_printCommandHandler : IRequestHandler<Add_update_thump_printCommand, AccountResponse<CustomerThumbs>>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IFilesHandlerService _fileService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _env;
        public Add_update_thump_printCommandHandler(ILoggerService logger, DataContext dataContext, IFilesHandlerService handlerService, IHttpContextAccessor accessor, IWebHostEnvironment web)
        {
            _accessor = accessor;
            _fileService = handlerService;
            _dataContext = dataContext;
            _logger = logger;
            _env = web;
        }
        public async Task<AccountResponse<CustomerThumbs>> Handle(Add_update_thump_printCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountResponse<CustomerThumbs>();
            try
            {
                var customer = _dataContext.deposit_customer_lite_information.Include(d => d.Deposit_customer_thumbs).Where(r => r.CustomerId == request.CustomerId && r.Deleted == false).FirstOrDefault();
                if (customer == null)
                {
                    response.Status.Message.FriendlyMessage = "unable to identify customer";
                    return response;
                }

                var file = _accessor.HttpContext.Request.Form.Files[0];

                var result =  _fileService.SaveSingleFile(file);
                if(!result.Status.IsSuccessful)
                {
                    response.Status.Message.FriendlyMessage = result.Status.Message.FriendlyMessage;
                    return response;
                }
                var customer_thumbs = customer.Deposit_customer_thumbs.FirstOrDefault(r => r.CustomerId == customer.CustomerId && r.FileName.Contains(request.Name));
                if (customer_thumbs == null) 
                    customer_thumbs = new Deposit_customer_thumbs();
                else
                { 
                    if (!string.IsNullOrEmpty(customer_thumbs.FilePath))
                    {
                        // Path.Combine("Resources", "Images/" +)
                        var filePath = customer_thumbs.FilePath;
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }  
                }

                customer_thumbs.CustomerId = request.CustomerId;
                customer_thumbs.FilePath = result.Status.Message.SearchResultMessage;
                customer_thumbs.FileName = request.Name;
                if (customer_thumbs.ThumbId == 0)
                    _dataContext.Deposit_customer_thumbs.Add(customer_thumbs);
                await _dataContext.SaveChangesAsync();

                response.List = _dataContext.Deposit_customer_thumbs.Where(e => e.CustomerId == request.CustomerId).Select(d => new CustomerThumbs(d)).ToList(); ;
                response.Status.IsSuccessful = true;
                response.Status.Message.FriendlyMessage = "successful";
                return response;
            }
            catch (Exception e)
            {
                response.Status.IsSuccessful = false;
                response.Status.Message.FriendlyMessage = e?.Message ?? e.InnerException?.Message;
                _logger.Error(e.ToString());
                response.Status.Message.TechnicalMessage = e.ToString();
                return response;
            }
        }
    }
}
