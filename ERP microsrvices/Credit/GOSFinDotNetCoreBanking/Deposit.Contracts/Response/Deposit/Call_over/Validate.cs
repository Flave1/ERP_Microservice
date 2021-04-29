using Deposit.Contracts.Response.Deposit.Deposit_form;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Deposit.Call_over
{
  
    public class Validate_transaction : IRequest<Account_response>
    {
        public int Structure { get; set; }
        public string Sub_structure { get; set; }
        public List<long> Currencies { get; set; }
    }
}
