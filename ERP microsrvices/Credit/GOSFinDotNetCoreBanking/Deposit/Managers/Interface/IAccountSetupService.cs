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
        Task<bool> AddUpdateAccountSetupAsync(AddUpdateAccountSetupObj setup);
        Task<bool> DeleteAccountSetupAsync(int id);
        Task<List<DepositAccountObj>> GetAllAccountSetupAsync();
        Task<DepositAccountObj> GetAccountSetupByIdAsync(int id);
        Task<string> UploadAccountSetupAsync(List<byte[]> record);
        byte[] GenerateExportAccountSetup(); 
    }
}
