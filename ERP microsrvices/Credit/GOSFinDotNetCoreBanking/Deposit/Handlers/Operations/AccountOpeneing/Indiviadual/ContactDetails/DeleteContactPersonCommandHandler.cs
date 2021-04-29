using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.AccountSetup
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
				var resp = new DeleteRespObj();
				try
				{
					var item = await _dataContext.deposit_customer_contact_detail.FindAsync(request.KeyContactPersonId);
					if (item != null)
					{
						item.Deleted = true;
						_dataContext.SaveChanges();
					}
					resp.Deleted = true;
					resp.Status.Message.FriendlyMessage = "Successful";
					resp.Status.IsSuccessful = true;
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
