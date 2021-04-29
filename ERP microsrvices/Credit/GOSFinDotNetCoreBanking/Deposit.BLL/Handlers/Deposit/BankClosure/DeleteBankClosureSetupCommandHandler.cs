using Deposit.Contracts.Command;
using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.AccountSetup
{
	public class DeleteBankClosureSetupCommandHandler : IRequestHandler<DeleteBankClosureSetupCommand, DeleteRespObj>
    {
		private readonly DataContext _dataContext;
		public DeleteBankClosureSetupCommandHandler(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
        public async Task<DeleteRespObj> Handle(DeleteBankClosureSetupCommand request, CancellationToken cancellationToken)
        {
			var resp = new Contracts.Response.Deposit.DeleteRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
			try
			{
				if(request.BankClosureSetupIds.Count() > 0)
				{
					foreach(var id in request.BankClosureSetupIds)
					{
						var item = await _dataContext.deposit_bankclosuresetup.FindAsync(id);
						if(item != null)
						{
							_dataContext.deposit_bankclosuresetup.Remove(item);
							_dataContext.SaveChanges();
						}
					}
					resp.Status.Message.FriendlyMessage = "Successful";
					return resp;
				}
				else
				{
					resp.Deleted = false;
					resp.Status.Message.FriendlyMessage = "Please Select Item to delete" ;
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
