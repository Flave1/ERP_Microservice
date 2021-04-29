using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Deposit.Contracts.Response.Deposit
{
    public class TillVaultSetupObj
    {
        public int TillVaultSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        public string StructureTillIdPrefix { get; set; }

        public string TellerTillIdPrefix { get; set; } 
        public string CompanyName { get; set; }
        public int ExcelLine { get; set; }
    }

    public class AddUpdateTillVaultSetupObj
    {
        public int TillVaultSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; }

        [StringLength(50)]
        public string StructureTillIdPrefix { get; set; }

        [StringLength(50)]
        public string TellerTillIdPrefix { get; set; }
    }

    public class TillVaultSetupRegRespObj
    {
        public int TillVaultSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TillVaultSetupRespObj
    {
        public List<TillVaultSetupObj> TillVaultSetups { get; set; }

        public APIResponseStatus Status { get; set; }
    }
}

