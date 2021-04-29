using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.General
{
    public class UploadResponse
    {
        public bool Uploaded { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
