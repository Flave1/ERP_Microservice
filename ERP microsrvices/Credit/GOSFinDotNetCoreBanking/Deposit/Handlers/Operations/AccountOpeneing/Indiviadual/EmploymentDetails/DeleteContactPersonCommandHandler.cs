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

namespace Deposit.Handlers.Operations.AccountOpeneing.Indiviadual.EmploymentDetails
{
	public class DeleteKeyContactPersonCommand : IRequest<DeleteRespObj>
	{
		public int KeyContactPersonId { get; set; }
		public class DeleteContactPersonCommandHandler : IRequestHandler<DeleteKeyContactPersonCommand, DeleteRespObj>
		{
			private readonly DataContext _dataContext;
			public DeleteContactPersonCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<DeleteRespObj> Handle(DeleteKeyContactPersonCommand request, CancellationToken cancellationToken)
			{
				var resp = new DeleteRespObj { Deleted = true, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
				try
				{
					//var item = await _dataContext.deposit_keycontactpersons.FindAsync(request.KeyContactPersonId);
					//if (item != null)
					//{
					//	item.Deleted = true;
					//	_dataContext.SaveChanges();
					//}
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
