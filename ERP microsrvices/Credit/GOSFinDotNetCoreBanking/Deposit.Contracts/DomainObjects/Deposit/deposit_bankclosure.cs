using Deposit.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.DomainObjects
{
    public partial class deposit_bankclosure : GeneralEntity
    {
        [Key]
        public long BankClosureId { get; set; }

        public int? Structure { get; set; }

        public int? SubStructure { get; set; }

        [StringLength(50)]
        public string AccountName { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public string Status { get; set; }

        [StringLength(50)]
        public string AccountBalance { get; set; }

        public int? Currency { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ClosingDate { get; set; }

        [StringLength(50)]
        public string Reason { get; set; }

        public decimal? Charges { get; set; }

        [StringLength(50)]
        public string FinalSettlement { get; set; }

        [StringLength(50)]
        public string Beneficiary { get; set; }

        public int ModeOfSettlement { get; set; }
        public string SettlmentAccountNumber { get; set; }

        [StringLength(50)]
        public string TransferAccount { get; set; }  
        public int AccountId { get; set; }
        public int ApprovalStatusId { get; set; }
        public string WorkflowToken { get; set; }
    }

}
