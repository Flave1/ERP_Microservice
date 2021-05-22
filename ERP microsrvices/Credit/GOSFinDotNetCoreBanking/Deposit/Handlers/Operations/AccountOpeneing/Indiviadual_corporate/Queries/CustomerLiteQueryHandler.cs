using Deposit.Contracts.Response;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using GOSLibraries.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Operations.AccountOpeneing.Indiviadual_corporate.Queries
{
    public class CustomerLiteQuery : IRequest<AccountResponse<GetCustomerQueryRepsonse>>
    {
        public class CustomerLiteQueryHandler : IRequestHandler<CustomerLiteQuery, AccountResponse<GetCustomerQueryRepsonse>>
        {
            private readonly DataContext _context;
            public CustomerLiteQueryHandler(DataContext dataContext)
            {
                _context = dataContext;
            }
            public async Task<AccountResponse<GetCustomerQueryRepsonse>> Handle(CustomerLiteQuery request, CancellationToken cancellationToken)
            {
                var response = new AccountResponse<GetCustomerQueryRepsonse>();
                var query = _context.deposit_customer_lite_information
                    .Include(e => e.deposit_individual_customer_information)
                    .Include(e => e.deposit_customer_account_information).Take(100)
                    .OrderByDescending(e => e.UpdatedOn).Where(e => e.Deleted == false && e.CustomerTypeId == (int)CustomerType.Individual); 
                
                response.List = query.Select(e => new GetCustomerQueryRepsonse(e)).ToList();
 
                return response;
            }
        }
    }
  
}
