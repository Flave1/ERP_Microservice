using Deposit.Contracts.Response.Deposit.Deposit_form;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Deposit.Operation
{
    public class Reactivate_Customer_command : IRequest<Account_response>
    {
        public long Id { get; set; }
        public int Product { get; set; }  
        public int? Structure { get; set; } 
        public int? Substructure { get; set; }
        public int[] Currency { get; set; }
        public string Account_number { get; set; }      
        public string Reactivation_reason { get; set; }
        public long CustomerId { get; set; } 

    }

    public class Reactivated_customers
    {
        public long Id { get; set; }

        public int? Structure { get; set; }

        public int? Substructure { get; set; }

        public string Account_name { get; set; }

        public string Account_number { get; set; }

        public decimal AccountBalance { get; set; }

        public int[] Currency { get; set; }
         

        public string Reactivation_reason { get; set; }

        public decimal Charges { get; set; }
        public int ApprovalStatus { get; set; }
        public string Approval_status_name { get; set; }
    }

    public class Reactivated_customers_response
    {
        public Reactivated_customers_response()
        {
            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage()};
            Reactivated_customers = new List<Reactivated_customers>(); 
        }
        public APIResponseStatus Status { get; set; }
        public List<Reactivated_customers> Reactivated_customers { get; set; }
    }
}
