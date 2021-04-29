using Deposit.Contracts.Response.Deposit.Deposit_form;
using Deposit.Repository.Interface.Deposit;
using Deposit.Data;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.Deposit_form
{
    public class GetAllCustomerDepositTransactionQuery : IRequest<deposit_transaction_response>
    {
        public class GetAllCustomerDepositTransactionQueryHandler : IRequestHandler<GetAllCustomerDepositTransactionQuery, deposit_transaction_response>
        {
            private readonly DataContext _context;
            private readonly ICustomerService _customer;
            public GetAllCustomerDepositTransactionQueryHandler(DataContext dataContext, ICustomerService customer)
            {
                _context = dataContext;
                _customer = customer;
            }
            public async Task<deposit_transaction_response> Handle(GetAllCustomerDepositTransactionQuery request, CancellationToken cancellationToken)
            {
                var response = new deposit_transaction_response();

                response .Customer_deposits= (from a in _context.deposit_form where a.Deleted == false select
                          new Customer_deposits
                          {
                              Account_number = a.Account_number,
                              CustomerId = a.CustomerId,
                              Customer_name = _customer.Return_customer_name(a.CustomerId),
                              Deposit_amount = a.Deposit_amount,
                              Id = a.Id,
                              Instrument_date = a.Instrument_date,
                              Instrument_number = a.Instrument_number,
                              Remark = a.Remark,
                              Structure = a.Structure,
                              TransactionId = a.TransactionId,
                              Transaction_mode = a.Transaction_mode,
                              Transaction_particulars = a.Transaction_particulars,
                              Value_date = a.Value_date, 
                          }).ToList(); 
                return response;
            }
        }
    }

}
