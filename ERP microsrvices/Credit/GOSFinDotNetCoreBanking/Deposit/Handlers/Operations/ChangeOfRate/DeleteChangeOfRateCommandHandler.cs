using Deposit.Contracts.Command;
using Deposit.Contracts.GeneralExtension;
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
    public class DeleteChangeOfRateCommand : IRequest<Delete_response>
	{
		public List<int> ItemIds { get; set; }
		public class DeleteChangeOfRateCommandHandler : IRequestHandler<DeleteChangeOfRateCommand, Delete_response>
		{
			private readonly DataContext _dataContext;
			public DeleteChangeOfRateCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<Delete_response> Handle(DeleteChangeOfRateCommand request, CancellationToken cancellationToken)
			{
				var resp = new Delete_response();
				try
				{
					if (request.ItemIds.Count() > 0)
					{
						foreach (var id in request.ItemIds)
						{
							var item = await _dataContext.deposit_changeofrates.FindAsync(id);
							if (item != null)
							{
								item.Deleted = true; 
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
