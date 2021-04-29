using Deposit.Contracts.GeneralExtension;
using Deposit.DomainObjects.Deposit;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GODP.Entities.Models
{
    public partial class deposit_withdrawalsetup : GeneralEntity
    {
        [Key]
        public int WithdrawalSetupId { get; set; }

        public int? Structure { get; set; }

        public int? Product { get; set; }

        public bool? PresetChart { get; set; }

        public int? AccountType { get; set; }

        public decimal? DailyWithdrawalLimit { get; set; }

        public bool? WithdrawalCharges { get; set; }

        [StringLength(50)]
        public string Charge { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(50)]
        public string ChargeType { get; set; }
        [ForeignKey("Product")]
        public deposit_accountsetup deposit_accountsetup { get; set; }
        [ForeignKey("AccountType")]
        public deposit_accountype deposit_accountype { get; set; }
    }
}
