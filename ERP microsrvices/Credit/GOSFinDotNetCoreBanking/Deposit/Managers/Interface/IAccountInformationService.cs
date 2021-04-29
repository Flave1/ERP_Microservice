using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Managers.Interface
{
    public interface IAccountInformationService
    {
        string Create_account_number(string prefix);
    }
}
