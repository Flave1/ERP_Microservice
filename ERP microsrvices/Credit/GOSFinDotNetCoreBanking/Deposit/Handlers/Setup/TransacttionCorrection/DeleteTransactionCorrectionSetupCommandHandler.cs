using Deposit.Contracts.GeneralExtension;
using Deposit.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.AccountSetup
{
	public class DeleteTransactionCorrectionSetupCommand : IRequest<Delete_response>
	{
		public List<int> ItemIds { get; set; }
		public class DeleteTransactionCorrectionSetupCommandHandler : IRequestHandler<DeleteTransactionCorrectionSetupCommand, Delete_response>
		{
			private readonly DataContext _dataContext;
			public DeleteTransactionCorrectionSetupCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<Delete_response> Handle(DeleteTransactionCorrectionSetupCommand request, CancellationToken cancellationToken)
			{
				var resp = new Delete_response();
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
