using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
    public interface IDepositCategoryService
    {
        Task<bool> AddUpdateCategoryAsync(deposit_category model);
        Task<IEnumerable<deposit_category>> GetAllCategoryAsync();
        Task<deposit_category> GetCategoryByIdAsync(int id);
        Task<bool> DeleteCategoryAsync(int id);
        Task<string> UploadCategoryAsync(List<byte[]> record);
        byte[] GenerateExportCategory();
    }
}
