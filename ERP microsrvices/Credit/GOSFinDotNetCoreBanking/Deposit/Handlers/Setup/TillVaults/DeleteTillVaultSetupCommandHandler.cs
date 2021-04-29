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
	public class DeleteTillVaultSetupCommand : IRequest<Delete_response>
	{
		public List<int> targetids { get; set; }
		public class DeleteTillVaultSetupCommandHandler : IRequestHandler<DeleteTillVaultSetupCommand, Delete_response>
		{
			private readonly DataContext _dataContext;
			public DeleteTillVaultSetupCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<Delete_response> Handle(DeleteTillVaultSetupCommand request, CancellationToken cancellationToken)
			{
				var resp = new Delete_response();
				try
				{
					if (request.targetids.Count() > 0)
					{
						foreach (var id in request.targetids)
						{
							var item = await _dataContext.deposit_tillvaultsetup.FindAsync(id);
							if (item != null)
							{
								_dataContext.deposit_tillvaultsetup.Remove(item);
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
