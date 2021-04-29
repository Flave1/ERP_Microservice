using Deposit.Contracts.Response.IdentityServer;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Deposit.Contracts.Response.Deposit
{
    public class ChangeOfRateSetupObj
    {
        public ChangeOfRateSetupObj() { }
        public int ChangeOfRateSetupId { get; set; }

        public int? Structure { get; set; }

        public int? ProductId { get; set; }

        public bool? CanApply { get; set; } 
         
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public int Excel_line_number { get; set; }
        public ChangeOfRateSetupObj(deposit_changeofratesetup db, CompanyStructureRespObj comps)
        {
            ChangeOfRateSetupId = db.ChangeOfRateSetupId;
            Structure = db.Structure;
            ProductId = db.ProductId;
            CanApply = db.CanApply;
            ProductName = db?.deposit_accountsetup?.AccountName;
            CompanyName = comps.companyStructures.FirstOrDefault(e => e.companyStructureId == db.Structure)?.name;
        }
    }

    public class AddUpdateChangeOfRateSetupObj
    {
        public int ChangeOfRateSetupId { get; set; }

        public int? Structure { get; set; }

        public int ProductId { get; set; }

        public bool? CanApply { get; set; }
    }

    public class ChangeOfRateSetupRegRespObj
    {
        public int ChangeOfRateSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class ChangeOfRateSetupRespObj
    {
        public ChangeOfRateSetupRespObj()
        {
            ChangeOfRateSetups = new List<ChangeOfRateSetupObj>();
            Status = new APIResponseStatus { Message = new APIResponseMessage() };
        }
        public List<ChangeOfRateSetupObj> ChangeOfRateSetups { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

