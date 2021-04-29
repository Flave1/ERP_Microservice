using Deposit.Contracts.GeneralExtension;
using Deposit.Contracts.Response.Deposit;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Command
{
    public class AddUpdateBankClosureCommand : IRequest<Deposit_bankClosureRegRespObj>
    {
        public int BankClosureId { get; set; }
        public int? Structure { get; set; }

        public int? SubStructure { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }

        public string Status { get; set; }
        public string AccountBalance { get; set; }

        public int? Currency { get; set; }

        public DateTime? ClosingDate { get; set; }

        public string Reason { get; set; }

        public decimal? Charges { get; set; }

        public decimal FinalSettlement { get; set; }

        public string Beneficiary { get; set; }

        public int ModeOfSettlement { get; set; }

        public string TransferAccount { get; set; } 
        public string ApproverName { get; set; } 
        public string ApproverComment { get; set; }
        public int AccountId { get; set; }
        public string SettlmentAccountNumber { get; set; }
    }
    public class DeleteBankClosureCommand : IRequest<Delete_response>
    {
        public List<int> BankClosureIds { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
