using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Common
{
    public class DownloadResponse
    {
        public byte[] ExcelFile { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class UploadResponse
    { 
        public List<byte[]> File { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
