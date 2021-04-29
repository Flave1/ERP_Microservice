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
	public class DeleteIdentificationCommand : IRequest<Contracts.GeneralExtension.Delete_response>
	{
		public List<int> ItemIds { get; set; }
		public class DeleteIdentificationCommandHandler : IRequestHandler<DeleteIdentificationCommand, Contracts.GeneralExtension.Delete_response>
		{
			private readonly DataContext _dataContext;
			public DeleteIdentificationCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<Contracts.GeneralExtension.Delete_response> Handle(DeleteIdentificationCommand request, CancellationToken cancellationToken)
			{
				var resp = new Contracts.GeneralExtension.Delete_response();
				try
				{
					if (request.ItemIds.Count() > 0)
					{
						foreach (var id in request.ItemIds)
						{
							var item = await _dataContext.deposit_customerIdentification.FindAsync(id);
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
