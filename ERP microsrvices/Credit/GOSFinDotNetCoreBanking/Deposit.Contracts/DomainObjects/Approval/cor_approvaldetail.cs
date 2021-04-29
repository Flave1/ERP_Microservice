using Deposit.Contracts.GeneralExtension;
using System;
using System.ComponentModel.DataAnnotations;

namespace Deposit.DomainObjects.Approval
{
    public class cor_approvaldetail : GeneralEntity
    {
        [Key]
        public long ApprovalDetailId { get; set; }
        public int StatusId { get; set; }
        public int StaffId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public int TargetId { get; set; }
        public string WorkflowToken { get; set; }
        public int ReferredStaffId { get; set; }
    }
}
