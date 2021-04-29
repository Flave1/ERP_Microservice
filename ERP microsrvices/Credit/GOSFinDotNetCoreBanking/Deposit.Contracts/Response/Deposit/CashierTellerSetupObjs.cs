using Deposit.Contracts.Response.IdentityServer;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deposit.Contracts.Response.Deposit
{
    public class CashierTellerSetupObj
    {
        public int DepositCashierTellerSetupId { get; set; }

        public int? Structure { get; set; }

        public int? ProductId { get; set; }

        public bool? PresetChart { get; set; } 
        public string CompanyName { get; set; }
        public string ProductName { get; set; }
        public long Employee_ID { get; set; }
        public string Staff_name { get; set; }
        public string Staff_code { get; set; }
        public string Cashier_numer { get; set; }
        public int ExcelLine { get; set; }
        public CashierTellerSetupObj() { }
        public CashierTellerSetupObj(deposit_cashiertellersetup db, StaffRespObj stf, CompanyStructureRespObj comp)
        {
            DepositCashierTellerSetupId = db.DepositCashierTellerSetupId;
            Structure = db.Structure;
            ProductId = db.ProductId;
            PresetChart = db.PresetChart;
            CompanyName = comp.companyStructures.FirstOrDefault(e => e.companyStructureId == db.Structure)?.name;
            Employee_ID = db.Employee_ID;
            Staff_code = $"{stf.staff.FirstOrDefault(r => r.staffId == db.Employee_ID)?.staffCode}";
            Staff_name = $"{stf.staff.FirstOrDefault(r => r.staffId == db.Employee_ID)?.firstName} {stf.staff.FirstOrDefault(r => r.staffId == db.Employee_ID)?.lastName}";
            Cashier_numer = db.Sub_strructure;
            ProductName = db.deposit_accountsetup?.AccountName;
        }
        public CashierTellerSetupObj(deposit_cashiertellersetup db)
        {
            DepositCashierTellerSetupId = db.DepositCashierTellerSetupId;
            Structure = db.Structure;
            ProductId = db.ProductId;
            PresetChart = db.PresetChart; 
            Employee_ID = db.Employee_ID; 
            Cashier_numer = db.Sub_strructure;
        }
    }

    public class AddUpdateCashierTellerSetupObj
    {
        public int DepositCashierTellerSetupId { get; set; }

        public int Structure { get; set; }

        public int ProductId { get; set; }

        public bool? PresetChart { get; set; }
        public long Employee_ID { get; set; }
        public string Sub_strructure { get; set; } 
    }

    public class CashierTellerSetupRegRespObj
    {
        public int DepositCashierTellerSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class CashierTellerSetupRespObj
    {
        public List<CashierTellerSetupObj> DepositCashierTellerSetups { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

