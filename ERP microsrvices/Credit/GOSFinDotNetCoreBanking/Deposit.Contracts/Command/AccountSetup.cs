using Deposit.Contracts.Response.Deposit;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Deposit.Contracts.Command
{
    public class AddUpdateAccountSetupCommand : IRequest<AccountSetupRegRespObj>
    {
        public int DepositAccountId { get; set; }

        [Required]
        [StringLength(500)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public int AccountTypeId { get; set; }

        public int? CurrencyId { get; set; }
         
        public int DormancyDays { get; set; }

        public decimal InitialDeposit { get; set; }

        public int CategoryId { get; set; }

        public int? BusinessCategoryId { get; set; }

        public int? GLMapping { get; set; }

        public int? BankGl { get; set; }

        public decimal? InterestRate { get; set; }

        [Required]
        [StringLength(50)]
        public string InterestType { get; set; }

        public bool? CheckCollecting { get; set; }

        [Required]
        [StringLength(50)]
        public string MaturityType { get; set; }

        public int? ApplicableTaxId { get; set; }

        public int? ApplicableChargesId { get; set; }

        public bool? PreTerminationLiquidationCharge { get; set; }

        public int? InterestAccrual { get; set; }

        public bool? Status { get; set; }

        public bool? OperatedByAnother { get; set; }

        public bool? CanNominateBenefactor { get; set; }

        public bool? UsePresetChartofAccount { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionPrefix { get; set; }

        [Required]
        [StringLength(50)]
        public string CancelPrefix { get; set; }

        [Required]
        [StringLength(50)]
        public string RefundPrefix { get; set; }

        public bool? Useworkflow { get; set; }

        public bool? CanPlaceOnLien { get; set; }

        public bool? InUse { get; set; }
    }
}
