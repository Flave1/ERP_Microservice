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
    public class WithdrawalSetupObj
    {
        public WithdrawalSetupObj()
        {

        }
        public int WithdrawalSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? PresetChart { get; set; }

        public int? AccountType { get; set; }

        public decimal? DailyWithdrawalLimit { get; set; }

        public bool? WithdrawalCharges { get; set; }  
        public string ProductName { get; set; }
        public string AccountTypeName { get; set; } 
        public string CompanyName { get; set; }     
        public string Charge { get; set; }
        public decimal? Amount { get; set; }
        public string ChargeType { get; set; }
        public int ExcelLine { get; set; }
        public WithdrawalSetupObj(deposit_withdrawalsetup db, CompanyStructureRespObj comp)
        {
            WithdrawalSetupId = db.WithdrawalSetupId;
            Structure = db.Structure;
            Product = db.Product;
            ProductName = db.deposit_accountsetup?.AccountName;
            PresetChart = db.PresetChart;
            AccountType = db.AccountType;
            AccountTypeName = db.deposit_accountype?.Name;
            DailyWithdrawalLimit = db.DailyWithdrawalLimit;
            Charge = db.Charge;
            WithdrawalCharges = db.WithdrawalCharges;
            ChargeType = db.ChargeType;
            CompanyName = comp.companyStructures.FirstOrDefault(e => e.companyStructureId == db.Structure)?.name;
        } 
    }

    public class AddUpdateWithdrawalSetupObj
    {
        public int WithdrawalSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? PresetChart { get; set; }

        public int? AccountType { get; set; }

        public decimal DailyWithdrawalLimit { get; set; }

        public bool WithdrawalCharges { get; set; }

        [StringLength(50)]
        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string ChargeType { get; set; }
    }

    public class WithdrawalSetupRegRespObj
    {
        public int WithdrawalSetupId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WithdrawalSetupRespObj
    {
        public List<WithdrawalSetupObj> WithdrawalSetups { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

