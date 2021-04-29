using Deposit.Contracts.Command;
using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.AccountSetup
{
    public class DeleteSignatoryCommand : IRequest<Contracts.Response.Deposit.DeleteRespObj>
	{
		public int SignatoriesId { get; set; }
		public class DeleteSignatoryCommandHandler : IRequestHandler<DeleteSignatoryCommand, DeleteRespObj>
		{
			private readonly DataContext _dataContext;
			public DeleteSignatoryCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<DeleteRespObj> Handle(DeleteSignatoryCommand request, CancellationToken cancellationToken)
			{
				var resp = new Contracts.Response.Deposit.DeleteRespObj { Deleted = true, Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
				try
				{ 
					var item = await _dataContext.deposit_signatories.FindAsync(request.SignatoriesId);
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
