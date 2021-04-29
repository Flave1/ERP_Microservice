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
    public class DeleteKeyContactPersonCommand : IRequest<DeleteRespObj>
	{
		public int KeyContactPersonId { get; set; }
		public class DeleteKeyContactPersonCommandHandler : IRequestHandler<DeleteKeyContactPersonCommand, DeleteRespObj>
		{
			private readonly DataContext _dataContext;
			public DeleteKeyContactPersonCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<DeleteRespObj> Handle(DeleteKeyContactPersonCommand request, CancellationToken cancellationToken)
			{
				var resp = new Contracts.Response.Deposit.DeleteRespObj { Deleted = true, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
				try
				{
					var item = await _dataContext.deposit_keycontactpersons.FindAsync(request.KeyContactPersonId);
					if (item != null)
					{
						item.Deleted = true;
						_dataContext.SaveChanges();
					}
					resp.Status.Message.FriendlyMessage = "Successful";
					return resp;
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}
	}
	 
}
