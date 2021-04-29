using MediatR;
using static Deposit.Contracts.Response.Approvals.ApprovalObjs;

namespace Deposit.Handlers.Details
{
    public class GetCurrentTargetApprovalDetailQuery : IRequest<ApprovalDetailsRespObj>
    {
        public GetCurrentTargetApprovalDetailQuery() { }
        public int TargetId { get; set; }
        public string WorkflowToken { get; set; }
        public GetCurrentTargetApprovalDetailQuery(int targetId, string workflwToken)
        {
            TargetId = targetId;
            WorkflowToken = workflwToken;
        }
    }
}
