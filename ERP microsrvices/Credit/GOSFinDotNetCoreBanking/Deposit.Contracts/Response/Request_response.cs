using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response
{
    public class Deposit_req_response
    {
        public Deposit_req_response()
        {
            Status = new APIResponseStatus { Message = new APIResponseMessage() };
        }
        public long Process_Id { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class AccountResponse<T>
    {
        public List<T> List { get; set; }
        public APIResponseStatus Status { get; set; } = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() };
    }
}
