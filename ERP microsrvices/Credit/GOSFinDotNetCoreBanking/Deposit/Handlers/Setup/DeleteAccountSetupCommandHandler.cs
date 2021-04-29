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
    public class DeleteAccountSetupCommand : IRequest<Delete_response>
    {
        public int Item_ids { get; set; }
        public class DeleteAccountSetupCommandHandler : IRequestHandler<DeleteAccountSetupCommand, Delete_response>
        {
            private readonly DataContext _dataContext;
            public DeleteAccountSetupCommandHandler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task<Delete_response> Handle(DeleteAccountSetupCommand request, CancellationToken cancellationToken)
            {
                var response = new Delete_response();
                try {
                    //{
                    //    foreach(var id in Item_ids)
                    //    var item = await _dataContext.deposit_accountsetup.FindAsync(id);
                    //    if (itemToDelete != null)
                    //    {
                    //        itemToDelete.Deleted = true;
                    //        _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
                    //    }
                    //    return await _dataContext.SaveChangesAsync() > 0;
                    return null;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
    }

}
