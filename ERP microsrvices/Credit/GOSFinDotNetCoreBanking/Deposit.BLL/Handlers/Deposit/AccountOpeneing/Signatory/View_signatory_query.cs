using Deposit.Contracts.GeneralExtension;
using Deposit.Data;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.AccountOpeneing.Signatory
{
    public class View_signatory_query : IRequest<DownloadFIleResp>
    {
        public View_signatory_query() { }
        public int SignatoryId { get; set; }
        public View_signatory_query(int signatoryId)
        {
            SignatoryId = signatoryId;
        }

        public class View_signatory_queryHandler : IRequestHandler<View_signatory_query, DownloadFIleResp>
        {
            private readonly ILoggerService _logger;
            private readonly DataContext _dataContext;
            public View_signatory_queryHandler(
                ILoggerService loggerService,
                DataContext dataContext)
            {
                _logger = loggerService;
                _dataContext = dataContext;
            }
            public async Task<DownloadFIleResp> Handle(View_signatory_query request, CancellationToken cancellationToken)
            {
                var response = new DownloadFIleResp { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                try
                {
                    if (request.SignatoryId < 1)
                    {
                        response.Status.Message.FriendlyMessage = "Please select an item to download";
                        return response;
                    }
                    var item = await _dataContext.deposit_signatories.FindAsync(request.SignatoryId);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + item.SignatureName);
                    _logger.Information(path);

                    if (string.IsNullOrEmpty(item.SignatureName) || path.Length < 1)
                    {
                        response.Status.Message.FriendlyMessage = "File Not Found";
                        return response;
                    }
                    response.FileName = item?.SignatureName;
                    response.FIle = File.ReadAllBytes(path);
                    response.Extension = item?.Extention;
                    response.Status.IsSuccessful = true;
                    return response;
                }
                catch (Exception ex)
                {
                    #region Log error to file 
                    var errorCode = ErrorID.Generate(4);
                    _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = ex?.Message;
                    response.Status.Message.TechnicalMessage = ex.ToString();
                    return response;
                    #endregion
                }
            }

        }
    }
}
