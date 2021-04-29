using Deposit.Contracts.Response.Deposit;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
    public interface IAccountSetupService
    {
        Task<bool> AddUpdateAccountSetupAsync(deposit_accountsetup model);
        Task<bool> DeleteAccountSetupAsync(int id);
        Task<List<DepositAccountObj>> GetAllAccountSetupAsync();
        Task<DepositAccountObj> GetAccountSetupByIdAsync(int id);
        Task<string> UploadAccountSetupAsync(List<byte[]> record);
        byte[] GenerateExportAccountSetup();

        // GetAllDepositForm GetDepositForm DeleteDepositionForm
        bool AddUpdateDepositForm(DepositformObj entity);
        //IEnumerable<DepositformObj> GetAllDepositForm();
        //DepositformObj GetDepositForm(int id);
        //bool DeleteDepositionForm(int id);
    }
}
