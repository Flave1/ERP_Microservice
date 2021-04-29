using Deposit.Contracts.Response.Deposit.Deposit_form;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Deposit.Operation
{
    public class Withdrwal_from_customer_accountCommand : IRequest<Account_response>
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public int Product { get; set; }
        public long Currency { get; set; }
        public string Account_number { get; set; }
        public string Customer_number { get; set; }
        public decimal Amount { get; set; }
        public decimal Decimal { get; set; }
        public string Description { get; set; }
        public int Withdrawal_type { get; set; }
        public string Withdrawal_instrument_number { get; set; }
        public DateTime Withdrawal_instrument_date { get; set; }
        public string Exchnage_right { get; set; } 

    }

    public class Withdrawal_from_customer_account
    {
        public long Id { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public string Transaction_reference { get; set; }

        public string Account_number { get; set; }

        public int? Account_type { get; set; }

        public int? Currency { get; set; }

        public decimal? Amount { get; set; }

        public string Description { get; set; }

        public DateTime? Transaction_date { get; set; }

        public DateTime? Value_date { get; set; }

        public string Withdrawal_type { get; set; }

        public string Instrument_number { get; set; }

        public DateTime? Instrument_date { get; set; }

        public decimal? Exchange_rate { get; set; }

        public decimal? Total_charge { get; set; }
    }

    public class Withdrawal_from_customer_account_reponse
    {
        public List<Withdrawal_from_customer_account> Withdrawals { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
