using Deposit.Contracts.GeneralExtension;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Deposit.DomainObjects.Deposit
{
     
    public partial class deposit_accountsetup : GeneralEntity
    {
        public deposit_accountsetup()
        {
            deposit_changeofratesetup = new HashSet<deposit_changeofratesetup>();
            Account_setup_transaction_charges = new HashSet<Account_setup_transaction_charges>();
            deposit_changeofratesetup = new HashSet<deposit_changeofratesetup>();
            deposit_withdrawalsetup = new HashSet<deposit_withdrawalsetup>();
            deposit_cashiertellersetup = new HashSet<deposit_cashiertellersetup>();
        }
        [Key]
        public int DepositAccountId { get; set; }

        [StringLength(500)]
        public string AccountName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int AccountTypeId { get; set; }
        public decimal InitialDeposit { get; set; }
        public int CategoryId { get; set; }
        public decimal? InterestRate { get; set; }
        [StringLength(50)]
        public string InterestType { get; set; }
        public bool? CheckCollecting { get; set; }
        [StringLength(50)]
        public string MaturityType { get; set; }
        public bool? PreTerminationLiquidationCharge { get; set; }
        public int? InterestAccrual { get; set; }
        public bool? Status { get; set; }
        public bool? OperatedByAnother { get; set; }
        public bool? CanNominateBenefactor { get; set; }
        public bool? UsePresetChartofAccount { get; set; }

        [StringLength(50)]
        public string TransactionPrefix { get; set; }
        [StringLength(50)]
        public string CancelPrefix { get; set; }
        [StringLength(50)]
        public string RefundPrefix { get; set; }
        public bool? Useworkflow { get; set; }
        public bool? CanPlaceOnLien { get; set; }
        public long? CurrencyId { get; set; }
        public int DormancyDays { get; set; }
        public bool? InUse { get; set; }
        public DateTime DomancyDateCount { get; set; }
        [ForeignKey("AccountTypeId")]

        public virtual deposit_accountype deposit_accountype { get; set; } 
        [ForeignKey("CategoryId")]

        public virtual deposit_category deposit_category { get; set; }

        public virtual ICollection<Account_setup_transaction_charges> Account_setup_transaction_charges { get; set; }

        public virtual ICollection<Account_setup_transaction_tax> Account_Setup_Transaction_Taxes { get; set; } 
        
        public virtual ICollection<deposit_changeofratesetup> deposit_changeofratesetup { get; set; }
        public virtual ICollection<deposit_withdrawalsetup> deposit_withdrawalsetup { get; set; }
        public virtual ICollection<deposit_cashiertellersetup> deposit_cashiertellersetup { get; set; }
    }

    public class Account_setup_transaction_tax
    {
        [Key]
        public int Account_setup_transaction_taxID { get; set; }
        public int DepositAccountId { get; set; }
        public int TransactionTaxId { get; set; }
        [ForeignKey("DepositAccountId")]
        public virtual deposit_accountsetup Deposit_Accountsetup { get; set; }
        [ForeignKey("TransactionTaxId")]
        public virtual deposit_transactiontax Deposit_Transactiontax { get; set; }
    }

    public class Account_setup_transaction_charges
    {
        [Key]
        public int Account_setup_transaction_chargeID { get; set; }
        public int DepositAccountId { get; set; }
        public int TransactionChargeId { get; set; }
        [ForeignKey("DepositAccountId")]
        public virtual deposit_accountsetup Deposit_Accountsetup { get; set; } 
        [ForeignKey("TransactionChargeId")]
        public virtual deposit_transactioncharge Deposit_Transactioncharge { get; set; } 
    }
}
