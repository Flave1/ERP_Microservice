using Deposit.Contracts.Response.Approvals;

using Deposit.Handlers.Details;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deposit.DomainObjects.Approval;

namespace Deposit.Handlers.Deposit.Call_over
{
    public class Transaction_validations_staff_approvalCommand : IRequest<StaffApprovalRegRespObj>
	{
		public int ApprovalStatusId { get; set; }
		public string ApprovalComment { get; set; }
		public int ReferredStaffId { get; set; }
		public int TargetId { get; set; }

		public class Transaction_validations_staff_approvalCommandHandler : IRequestHandler<Transaction_validations_staff_approvalCommand, StaffApprovalRegRespObj>
		{

			private readonly IHttpContextAccessor _accessor;
			private readonly IIdentityServerRequest _serverRequest;
			private readonly IWorkflowDetailService _detailService;
			private readonly DataContext _dataContext;
			private readonly ICustomerService _service;
			public Transaction_validations_staff_approvalCommandHandler(
				IHttpContextAccessor httpContextAccessor,
				IIdentityServerRequest serverRequest,
				IWorkflowDetailService detailService,
				ICustomerService service,
				DataContext dataContext)
			{
				_accessor = httpContextAccessor;
				_serverRequest = serverRequest;
				_detailService = detailService;
				_dataContext = dataContext;
				_service = service;
			}

			public async Task<StaffApprovalRegRespObj> Handle(Transaction_validations_staff_approvalCommand request, CancellationToken cancellationToken)
			{
				var response = new StaffApprovalRegRespObj();
				try
				{
					if (request.ApprovalStatusId == (int)ApprovalStatus.Revert && request.ReferredStaffId < 1)
					{
						response.Status.Message.FriendlyMessage = "Please select staff to revert to";
						return response;
					}

					var user = await _serverRequest.UserDataAsync();

					var currentItem = _dataContext.deposit_cashierteller_form.FirstOrDefault(e => e.Deleted == false && request.TargetId == e.Id);

					if (currentItem.Approval_status == (int)ApprovalStatus.Approved)
					{
						response.Status.Message.FriendlyMessage = "Request already Validated";
						return response;
					}

					var detail = BuildApprovalDetailObject(request, currentItem, user.StaffId);

					var req = new IndentityServerApprovalCommand
					{
						ApprovalComment = request.ApprovalComment,
						ApprovalStatus = request.ApprovalStatusId,
						TargetId = request.TargetId,
						WorkflowToken = currentItem.WorkflowToken,
						ReferredStaffId = request.ReferredStaffId
					};

					using (var _trans = await _dataContext.Database.BeginTransactionAsync())
					{
						try
						{
							var result = await _serverRequest.StaffApprovalRequestAsync(req);

							if (!result.IsSuccessStatusCode)
							{
								response.Status.Message.FriendlyMessage = result.ReasonPhrase;
								response.Status.Message.TechnicalMessage = result.ToString();
								return response;
							}

							var stringData = await result.Content.ReadAsStringAsync();
							response = JsonConvert.DeserializeObject<StaffApprovalRegRespObj>(stringData);

							if (!response.Status.IsSuccessful)
							{
								response.Status = response.Status;
								return response;
							}

							if (response.ResponseId == (int)ApprovalStatus.Processing)
							{
								await _detailService.AddUpdateApprovalDetailsAsync(detail);
								currentItem.Approval_status = (int)ApprovalStatus.Processing;

								_dataContext.SaveChanges();
								await _trans.CommitAsync();

								response.Status.Message.FriendlyMessage = "Approval process started";
								response.ResponseId = (int)ApprovalStatus.Processing;
								return response;
							}
							if (response.ResponseId == (int)ApprovalStatus.Revert)
							{
								await _detailService.AddUpdateApprovalDetailsAsync(detail);
								currentItem.Approval_status = (int)ApprovalStatus.Revert;
								_dataContext.SaveChanges();
								await _trans.CommitAsync();

								response.Status.Message.FriendlyMessage = "Process revert successful";
								response.ResponseId = (int)ApprovalStatus.Revert;
								return response;
							}
							if (response.ResponseId == (int)ApprovalStatus.Approved)
							{
								await _detailService.AddUpdateApprovalDetailsAsync(detail);
								currentItem.Approval_status = (int)ApprovalStatus.Approved;
								await _service.Perform_transaction_teller_and_balancing(currentItem);
								_dataContext.SaveChanges();
								await _trans.CommitAsync();

								response.Status.Message.FriendlyMessage = "Final approval successful";
								response.ResponseId = (int)ApprovalStatus.Revert;
								return response;
							}
							if (response.ResponseId == (int)ApprovalStatus.Disapproved)
							{
								await _detailService.AddUpdateApprovalDetailsAsync(detail);
								currentItem.Approval_status = (int)ApprovalStatus.Disapproved;
								_dataContext.SaveChanges();
								await _trans.CommitAsync();

								response.Status.Message.FriendlyMessage = "Final approval successful";
								response.ResponseId = (int)ApprovalStatus.Revert;
								return response;
							}
						}
						catch (Exception ex)
						{
							await _trans.RollbackAsync();
							throw ex;
						}
						finally { await _trans.DisposeAsync(); }

					}

					return new StaffApprovalRegRespObj
					{
						ResponseId = Convert.ToInt32(detail.ApprovalDetailId),
						Status = response.Status
					};
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}


			private cor_approvaldetail BuildApprovalDetailObject(Transaction_validations_staff_approvalCommand request, deposit_cashierteller_form currentItem, int staffId)
			{
				var approvalDeatil = new cor_approvaldetail();
				var previousDetail = _detailService.GetApprovalDetailsAsync(request.TargetId, currentItem.WorkflowToken).Result;
				approvalDeatil.ArrivalDate = currentItem.CreatedOn;

				if (previousDetail.Count() > 0)
					approvalDeatil.ArrivalDate = previousDetail.OrderByDescending(s => s.ApprovalDetailId).FirstOrDefault().Date;

				approvalDeatil.Comment = request.ApprovalComment;
				approvalDeatil.Date = DateTime.Today;
				approvalDeatil.StatusId = request.ApprovalStatusId;
				approvalDeatil.TargetId = request.TargetId;
				approvalDeatil.ReferredStaffId = request.ReferredStaffId;
				approvalDeatil.StaffId = staffId;
				approvalDeatil.WorkflowToken = currentItem.WorkflowToken;
				return approvalDeatil;
			}
		}
	}

}
