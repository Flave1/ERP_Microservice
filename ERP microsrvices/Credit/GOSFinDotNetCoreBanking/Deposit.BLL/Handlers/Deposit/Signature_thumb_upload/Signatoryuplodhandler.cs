using Deposit.Contracts.Response.Common;
using Deposit.Repository.Implement.Deposit;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.Signature_thumb_upload
{
    public class Uploads
    {
        public string Name { get; set; }
    }
    public class Signatoryupload : IRequest<UploadResponse>
    {
        public int CustomerId { get; set; }
        public List<Uploads> Uploads { get; set; }
       
        public class SignatoryuploadHandler : IRequestHandler<Signatoryupload, UploadResponse>
        {
            private readonly IFilesHandlerService _service;
            private readonly DataContext _dataContext;
            public SignatoryuploadHandler(DataContext dataContext, IFilesHandlerService filesHandlerService)
            {
                _dataContext = dataContext;
                _service = filesHandlerService;
            }
            public async Task<UploadResponse> Handle(Signatoryupload request, CancellationToken cancellationToken)
            {
                var response = new UploadResponse { File = new List<byte[]>(), Status = new APIResponseStatus { Message = new APIResponseMessage() } };
                try
                {
                    var result = await _service.HandleForSignatures_thumbs_Async(request.CustomerId, request.Uploads);
                    if (result != "success")
                    {
                        response.Status.Message.FriendlyMessage = result;
                        return response;
                    }
                    response.Status.Message.FriendlyMessage = "Successful";
                    response.Status.IsSuccessful = true;
                    return response; 
                }
                catch (Exception e)
                {
                    response.Status.Message.TechnicalMessage = e.ToString();
                    return response;
                }
            }
        }
    }
}
