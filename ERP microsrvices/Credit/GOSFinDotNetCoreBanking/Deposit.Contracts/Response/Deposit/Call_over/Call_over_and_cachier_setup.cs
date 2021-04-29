using Deposit.Contracts.Response.Deposit.Deposit_form;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Deposit.Call_over
{
    public class cashierteller_call_over_response
    {
        public cashierteller_call_over_response()
        {
            Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() };
            Transaction_validations = new List<cashierteller_call_over>();
        }
        public List<cashierteller_call_over> Transaction_validations { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public  class cashierteller_call_over
    {
        public cashierteller_call_over()
        {
            Currencie_and_amount = new List<call_over_currecies_and_amount>(); 
        }
        public long Id { get; set; }

        public int Structure { get; set; }

        public string SubStructure { get; set; }
        public string Structure_name  { get; set; }

        public long Employee_ID { get; set; }
        public string Staff_name { get; set; }

        public DateTime? Date { get; set; } 

        public string Transaction_IDs { get; set; }
        public int Approval_status { get; set; }
        public string Approval_status_name { get; set; } 
        public List<call_over_currecies_and_amount> Currencie_and_amount { get; set; } 
    }

    public class call_over_currecies_and_amount 
    {
        public call_over_currecies_and_amount()
        {
            Transactions = new List<Transactions>();
        }
        public long Id { get; set; }
        public long Currency { get; set; }
        public string Currency_name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Call_over_date { get; set; }
        public decimal Opening_bal{ get; set; } 
        public decimal Closing_bal { get; set; }
        public decimal Cr_amount { get; set; }
        public decimal Dr_amount { get; set; }
        public List<Transactions> Transactions { get; set; }
    }

    public class add_call_over_currecies_and_amount : IRequest<Account_response>
    {  
        public long Currency { get; set; }
        public decimal Amount { get; set; }
        public DateTime Call_over_date { get; set; } 
    }

    public class Transaction_response_by_currencies
    {
        public Transaction_response_by_currencies()
        {
            Transactions = new List<Transactions>();
            Status = new APIResponseStatus();
        }
        public List<Transactions> Transactions { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class Transactions
    {
        public long Currency { get; set; }
        public decimal DB_amount { get; set; }
        public decimal CR_amount { get; set; }
        public string Transaction_operation { get; set; }
        public int Product { get; set; }
        public string Transaction_Id { get; set; }
        public string Instrument_type { get; set; }
        public string Instruent_number { get; set; }
        public string Account_number { get; set; }
        public decimal Amount { get; set; }
    }

   
}
