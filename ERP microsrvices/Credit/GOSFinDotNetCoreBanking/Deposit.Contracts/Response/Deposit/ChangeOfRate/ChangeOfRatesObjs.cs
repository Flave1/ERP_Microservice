using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Deposit.Contracts.Response.Deposit
{
    public class ChangeOfRatesObj
    {
        public int ChangeOfRateId { get; set; }

        public int? Structure { get; set; }
        public string StructureName { get; set; }

        public int? Product { get; set; }
        public string ProductName { get; set; }

        public decimal? CurrentRate { get; set; }

        public decimal? ProposedRate { get; set; }

        public string Reasons { get; set; }

        public string ApproverName { get; set; }

        public string ApproverComment { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string WorkflowToken { get; set; }
        public int ApprovalStatusId { get; set; }
        public string StatusName { get; set; }
    }




    

    public class AddChangeOfRateCommand : IRequest<ChangeOfRatesRegRespObj>
    {
        public int ChangeOfRateId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public decimal? CurrentRate { get; set; }

        public decimal? ProposedRate { get; set; }

        [StringLength(500)]
        public string Reasons { get; set; }

        [StringLength(50)]
        public string ApproverName { get; set; }

        [StringLength(50)]
        public string ApproverComment { get; set; }
    }

    public class ChangeOfRatesRegRespObj
    {
        public int ChangeOfRateId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class ChangeOfRatesRespObj
    {
        public List<ChangeOfRatesObj> ChangeOfRates { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

