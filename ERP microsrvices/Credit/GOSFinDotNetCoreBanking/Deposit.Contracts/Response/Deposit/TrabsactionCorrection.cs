using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Deposit
{
    
    public partial class TransactionCorrectionSetup 
    { 
        public int TransactionCorrectionSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        public int? JobTitleId { get; set; }
        public string CompanyName { get; set; }
        public string JobTitleName { get; set; }
        public int ExcelLine { get; set; }
    }

    public partial class AddUpdateTransactionCorrectionSetupCommand : IRequest<TransactionCorrectionSetupRegResp>
    {
        public int TransactionCorrectionSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        public int? JobTitleId { get; set; }
        public string CompanyName { get; set; }
        public string JobTitleName { get; set; }
    }

    public partial class TransactionCorrectionSetupResp : IRequest<TransactionCorrectionSetup>
    {
        public List<TransactionCorrectionSetup> TransactionCorrectionSetups { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public partial class TransactionCorrectionSetupRegResp : IRequest<TransactionCorrectionSetup>
    {
        public int TransactionCorrectionSetupId { get; set; }
        public byte[] Export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
