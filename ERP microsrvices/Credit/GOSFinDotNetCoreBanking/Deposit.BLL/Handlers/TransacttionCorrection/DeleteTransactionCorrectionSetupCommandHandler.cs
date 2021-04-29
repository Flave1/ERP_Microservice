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
    public class DeleteTransactionCorrectionSetupCommand : IRequest<DeleteRespObj>
	{
		public List<int> ItemIds { get; set; }
		public class DeleteTransactionCorrectionSetupCommandHandler : IRequestHandler<DeleteTransactionCorrectionSetupCommand, DeleteRespObj>
		{
			private readonly DataContext _dataContext;
			public DeleteTransactionCorrectionSetupCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<DeleteRespObj> Handle(DeleteTransactionCorrectionSetupCommand request, CancellationToken cancellationToken)
			{
				var resp = new DeleteRespObj { Deleted = true, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
				try
				{
					if (request.ItemIds.Count() > 0)
					{
						foreach (var id in request.ItemIds)
						{
							var item = await _dataContext.deposit_transactioncorrectionsetup.FindAsync(id);
							if (item != null)
							{
								_dataContext.deposit_transactioncorrectionsetup.Remove(item);
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
