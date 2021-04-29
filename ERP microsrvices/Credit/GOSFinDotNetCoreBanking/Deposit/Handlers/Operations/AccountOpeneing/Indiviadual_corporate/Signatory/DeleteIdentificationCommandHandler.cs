using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.GeneralExtension;
using Microsoft.EntityFrameworkCore;
using Deposit.Contracts.Response;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using System.Linq;
using Deposit.Requests;

namespace Deposit.Handlers.Deposit.AccountSetup
{
	public class DeleteSignatoryCommand : IRequest<AccountResponse<Signatory>>
	{
		public int SignatoriesId { get; set; }
		public class DeleteSignatoryCommandHandler : IRequestHandler<DeleteSignatoryCommand, AccountResponse<Signatory>>
		{
			private readonly DataContext _dataContext;
			private readonly IIdentityServerRequest _serverRequest;
			public DeleteSignatoryCommandHandler(DataContext dataContext, IIdentityServerRequest serverRequest)
			{
				_serverRequest = serverRequest;
				_dataContext = dataContext;
			}
			public async Task<AccountResponse<Signatory>> Handle(DeleteSignatoryCommand request, CancellationToken cancellationToken)
			{
				var response = new AccountResponse<Signatory>();
				try
				{
					var identificatins = await _serverRequest.GetIdentiticationTypeAsync();
					var item = await _dataContext.deposit_customer_signatories.FirstOrDefaultAsync(d => d.SignatoriesId == request.SignatoriesId && d.Deleted == false);
					if (item != null)
					{
						item.Deleted = true;
						_dataContext.SaveChanges();
					}
					response.Status.Message.FriendlyMessage = "Successful";
					response.Status.IsSuccessful = true;
					response.List = _dataContext.deposit_customer_signatories.Where(d => d.CustomerId == item.CustomerId && d.Deleted == false).Select(d => new Signatory(d, identificatins)).ToList();
					return response;
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}
	}
}