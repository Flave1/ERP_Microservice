using Deposit.Data;
using Deposit.Managers.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Managers.InterfaceManagers
{
    public class AccountInformationService : IAccountInformationService
    {
        private readonly DataContext _context;
        public AccountInformationService(DataContext context) =>  _context = context; 
        string IAccountInformationService.Create_account_number(string prefix)
        {
            var lastHighest = _context.deposit_customer_account_information.Select(e => Convert.ToInt64(e.AccountNumber)).Max();
            if (lastHighest == 0) lastHighest = 1;
            var value = Convert.ToInt32(lastHighest.ToString().Length - lastHighest);
            var account_number = value.ToString().PadLeft(value, '0');
            return account_number;
        }
    }
}
