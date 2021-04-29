using Deposit.Contracts.Response.Deposit;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
    public interface ICashierTellerService
    {
        #region CashierTellerSetup
        Task<bool> AddUpdateCashierTellerSetupAsync(deposit_cashiertellersetup model);
        Task<IEnumerable<CashierTellerSetupObj>> GetAllCashierTellerSetupAsync();
        CashierTellerSetupObj GetCashierTellerSetupById(int id);
        Task<bool> DeleteCashierTellerSetupAsync(int id);
        Task<string> UploadCashierTellerSetupAsync(List<byte[]> record);
        byte[] GenerateExportCashierTellerSetup();
        #endregion

        #region CashierTellerForm 
        Task<bool> AddUpdateCashierTellerFormAsync(deposit_cashierteller_form model);
        Task<IEnumerable<deposit_cashierteller_form>> GetAllCashierTellerFormAsync();
        Task<deposit_cashierteller_form> GetCashierTellerFormByIdAsync(int id);
        Task<bool> DeleteCashierTellerFormAsync(int id);
        #endregion
    }
}
