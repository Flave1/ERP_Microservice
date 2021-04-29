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
    public class DeleteKYCCommand : IRequest<DeleteRespObj>
	{
		public List<int> ItemIds { get; set; }
		public class DeleteKYCCommandHandler : IRequestHandler<DeleteKYCCommand, DeleteRespObj>
		{
			private readonly DataContext _dataContext;
			public DeleteKYCCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<DeleteRespObj> Handle(DeleteKYCCommand request, CancellationToken cancellationToken)
			{
				var resp = new Contracts.Response.Deposit.DeleteRespObj { Deleted = true, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
				try
				{
					if (request.ItemIds.Count() > 0)
					{
						foreach (var id in request.ItemIds)
						{
							var item = await _dataContext.deposit_kyc.FindAsync(id);
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
