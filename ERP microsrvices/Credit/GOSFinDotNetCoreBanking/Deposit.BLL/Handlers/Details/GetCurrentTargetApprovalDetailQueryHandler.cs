using Deposit.Handlers.Details;
using Deposit.Requests;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Deposit.Contracts.Response.Approvals.ApprovalObjs;

namespace Puchase_and_payables.Handlers.Supplier.Approvals
{

    public class GetCurrentTargetApprovalDetailQueryHandler : IRequestHandler<GetCurrentTargetApprovalDetailQuery, ApprovalDetailsRespObj>
    {
        private readonly IWorkflowDetailService _detail;
        private readonly IIdentityServerRequest _serverRequest;

        public GetCurrentTargetApprovalDetailQueryHandler(
            IWorkflowDetailService workflowDetailService, 
            IIdentityServerRequest identityServerRequest)
        { 
            _detail = workflowDetailService;
            _serverRequest = identityServerRequest;

        }
        public async Task<ApprovalDetailsRespObj> Handle(GetCurrentTargetApprovalDetailQuery request, CancellationToken cancellationToken)
        {
            var list = await _detail.GetApprovalDetailsAsync(request.TargetId, request.WorkflowToken);

            var staff = await _serverRequest.GetAllStaffAsync();
            var response = new ApprovalDetailsRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
              
            var temp = list;
            var previousStaff = temp.GroupBy(d => d.StaffId).Select(d => d.First()).Where(d => d.StatusId == (int)ApprovalStatus.Approved && d.TargetId == request.TargetId && d.WorkflowToken == request.WorkflowToken).ToArray();

            response.Details = list.Select(x => new ApprovalDetailsObj()
            {
                ApprovalDetailId = Convert.ToInt16(x.ApprovalDetailId),
                Comment = x.Comment,
                Date = x.Date,
                StaffId = x.StaffId,
                FirstName = staff.staff.FirstOrDefault(d => d.staffId == x.StaffId)?.firstName,
                LastName = staff.staff.FirstOrDefault(d => d.staffId == x.StaffId)?.lastName,
                StatusId = x.StatusId,
                StatusName = Convert.ToString((ApprovalStatus)x.StatusId),
                TargetId = x.TargetId,
                ArrivalDate = x.ArrivalDate,
                WorkflowToken = x.WorkflowToken
            }).ToList();
            response.PreviousStaff = previousStaff.Select(p => new PreviousStaff
            {
                StaffId = p.StaffId,
                Name = $"{staff.staff.FirstOrDefault(d => d.staffId == p.StaffId)?.firstName} {staff.staff.FirstOrDefault(d => d.staffId == p.StaffId)?.lastName}",
            }).ToList();
            return response;
        }
    }
}
