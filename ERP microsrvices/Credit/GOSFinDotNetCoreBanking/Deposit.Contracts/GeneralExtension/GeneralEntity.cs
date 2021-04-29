using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.GeneralExtension
{
    public class GeneralEntity
    {
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int CompanyId { get; set; }
    }

    public enum EmplumentTypes
    {
        Employed = 1,
        Selfemployed = 2,
        Unemployed = 3,
        Retired = 4,
        Student = 5,
        Others = 6,
    }

    public class DownloadFIleResp
    {
        public string FileName { get; set; }
        public byte[] FIle { get; set; }
        public string Extension { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class ViewFileResponse
    {
        public string FilePath { get; set; } = string.Empty;
        public APIResponseStatus Status { get; set; } = new APIResponseStatus { Message = new APIResponseMessage() };
    }
}
