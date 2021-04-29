using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Deposit.Deposit_form
{
    public class Deposit_to_customer : IRequest<Account_response>
    {
        public long Id { get; set; } 
        public string Account_number { get; set; }
        public long CustomerId { get; set; }
        public long Currency { get; set; }
        public decimal Deposit_amount { get; set; } 
        public string Transaction_particulars { get; set; }
        public string Remark { get; set; }
        public int Transaction_mode { get; set; }
        public string Instrument_number { get; set; }
        public DateTime Instrument_date { get; set; }
    }

    public class Customer_deposits  
    {
        public long Id { get; set; }
        public int Structure { get; set; }
        public string TransactionId { get; set; }
        public string Account_number { get; set; }
        public string Customer_name { get; set; }
        public long CustomerId { get; set; }
        public decimal Deposit_amount { get; set; }
        public string Transaction_particulars { get; set; }
        public string Remark { get; set; }
        public int Transaction_mode { get; set; }
        public string Instrument_number { get; set; }
        public DateTime Instrument_date { get; set; }
        public DateTime Value_date { get; set; }
        public decimal Total_charge { get; set; }
    }

    public class deposit_transaction_response
    {
        public deposit_transaction_response()
        {
            Customer_deposits = new List<Customer_deposits>();
            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() };
        }
        public List<Customer_deposits> Customer_deposits { get; set; }
        public APIResponseStatus Status{ get; set; }
    }

    public class Account_response
    {
        public Account_response()
        {
            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() };
            
        }
        public int Deposit_form_Id { get; set; }
        public APIResponseStatus Status { get; set; }
    }

   
}
