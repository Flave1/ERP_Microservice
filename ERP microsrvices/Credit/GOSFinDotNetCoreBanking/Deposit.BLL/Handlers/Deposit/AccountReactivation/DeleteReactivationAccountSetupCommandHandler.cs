using Deposit.Contracts.Command;
using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.AccountSetup
{
    public class DeleteReactivationAccountSetupCommand : IRequest<DeleteRespObj>
	{
		public List<int> targetids { get; set; }
		public class DeleteReactivationAccountSetupCommandHandler : IRequestHandler<DeleteReactivationAccountSetupCommand, DeleteRespObj>
		{
			private readonly DataContext _dataContext;
			public DeleteReactivationAccountSetupCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<DeleteRespObj> Handle(DeleteReactivationAccountSetupCommand request, CancellationToken cancellationToken)
			{
				var resp = new Contracts.Response.Deposit.DeleteRespObj { Deleted = true, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
				try
				{
					if (request.targetids.Count() > 0)
					{
						foreach (var id in request.targetids)
						{
							var item = await _dataContext.deposit_accountreactivationsetup.FindAsync(id);
							if (item != null)
							{
								_dataContext.deposit_accountreactivationsetup.Remove(item);
								_dataContext.SaveChanges();
							}
						}
						resp.Status.Message.FriendlyMessage = "Successful";
						return resp;
					}
					else
					{
						resp.Deleted = false;
						resp.Status.Message.FriendlyMessage = "Please Select Item to delete";
						return resp;
					}
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}
	}
	 
}
