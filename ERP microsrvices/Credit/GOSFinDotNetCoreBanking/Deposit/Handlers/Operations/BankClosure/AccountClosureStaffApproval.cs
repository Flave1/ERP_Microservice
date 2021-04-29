using Deposit.Contracts.Response.Approvals;
using Deposit.DomainObjects;
using Deposit.Handlers.Details;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deposit.DomainObjects.Approval;

namespace Deposit.Handlers.Deposit.BankClosure
{
	public class AccountClosureStaffApprovalCommand : IRequest<StaffApprovalRegRespObj>
	{
		public int ApprovalStatusId { get; set; }
		public string ApprovalComment { get; set; }
		public int ReferredStaffId { get; set; }
		public int TargetId { get; set; }

		public class AccountClosureStaffApprovalCommandHandler : IRequestHandler<AccountClosureStaffApprovalCommand, StaffApprovalRegRespObj>
		{

			private readonly IHttpContextAccessor _accessor;
			private readonly IIdentityServerRequest _serverRequest;
			private readonly IWorkflowDetailService _detailService;
			private readonly DataContext _dataContext;
			private readonly IDepositBankClosure _service; 
			public AccountClosureStaffApprovalCommandHandler(
				IHttpContextAccessor httpContextAccessor,
				IIdentityServerRequest serverRequest,
				IWorkflowDetailService detailService,
				IDepositBankClosure depositBankClosure,
				DataContext dataContext)
			{
				_accessor = httpContextAccessor;
				_serverRequest = serverRequest;
				_detailService = detailService;
				_dataContext = dataContext;
				_service = depositBankClosure;
			}

			public async Task<StaffApprovalRegRespObj> Handle(AccountClosureStaffApprovalCommand request, CancellationToken cancellationToken)
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

					var currentItem = _dataContext.deposit_bankclosure.FirstOrDefault(e => e.Deleted == false && request.TargetId == e.BankClosureId);
				   
					if (currentItem.ApprovalStatusId == (int)ApprovalStatus.Approved)
					{
						response.Status.Message.FriendlyMessage = "Request already processed";
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
								currentItem.ApprovalStatusId = (int)ApprovalStatus.Processing; 
								 
								await _service.AddUpdateDepositBankClosure(currentItem);
								await _trans.CommitAsync();

								response.Status.Message.FriendlyMessage = "Approval process started";
								response.ResponseId = (int)ApprovalStatus.Processing;
								return response; 
							}
							if (response.ResponseId == (int)ApprovalStatus.Revert)
							{
								await _detailService.AddUpdateApprovalDetailsAsync(detail);
								currentItem.ApprovalStatusId = (int)ApprovalStatus.Revert;



								await _service.AddUpdateDepositBankClosure(currentItem);
								await _trans.CommitAsync();

								response.Status.Message.FriendlyMessage = "Process revert successful";
								response.ResponseId = (int)ApprovalStatus.Revert;
								return response;
 
							}
							if (response.ResponseId == (int)ApprovalStatus.Approved)
							{
								var customer_account = _dataContext.deposit_customer_account_information.FirstOrDefault(r => r.AccountNumber.ToLower() == currentItem.AccountNumber.ToLower());
								if (customer_account != null)
								{
									customer_account.Deleted = true;
								}
								await _detailService.AddUpdateApprovalDetailsAsync(detail);
								currentItem.ApprovalStatusId = (int)ApprovalStatus.Approved;
								 
								await _service.AddUpdateDepositBankClosure(currentItem);
								await _trans.CommitAsync();

								response.Status.Message.FriendlyMessage = "Final approval successful";
								response.ResponseId = (int)ApprovalStatus.Revert;
								return response;
							}
							if (response.ResponseId == (int)ApprovalStatus.Disapproved)
							{
								await _detailService.AddUpdateApprovalDetailsAsync(detail);
								currentItem.ApprovalStatusId = (int)ApprovalStatus.Disapproved;
								await _service.AddUpdateDepositBankClosure(currentItem);
								await _trans.CommitAsync();

								response.Status.Message.FriendlyMessage = "Final approval successful";
								response.ResponseId = (int)ApprovalStatus.Revert;
								return response;
							}
						}
						catch (Exception ex)
						{
							await _trans.RollbackAsync();
							throw new Exception("Error Occurerd", new Exception($"{ex.Message}"));
						}
						finally { await _trans.DisposeAsync(); }

					}

					return new StaffApprovalRegRespObj
					{
						ResponseId = Convert.ToInt32(currentItem.BankClosureId),
						Status = response.Status
					};
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}

			 
			private cor_approvaldetail BuildApprovalDetailObject(AccountClosureStaffApprovalCommand request, deposit_bankclosure currentItem, int staffId)
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
