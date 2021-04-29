using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.GeneralExtension
{
    public class Delete_response
    {
        public Delete_response()
        {
            Status = new APIResponseStatus
            {
                Message = new APIResponseMessage()
            };
        }
        public bool Deleted { get; set; } = true;
        public APIResponseStatus Status { get; set; } = new APIResponseStatus { IsSuccessful = true };
    }  
}
