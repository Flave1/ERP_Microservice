using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
    public interface IBusinessCategoryService
    {
        Task<bool> AddUpdateBusinessCategoryAsync(deposit_businesscategory model);
        Task<IEnumerable<deposit_businesscategory>> GetAllBusinessCategoryAsync();
        Task<deposit_businesscategory> GetBusinessCategoryByIdAsync(int id);
        Task<bool> DeleteBusinessCategoryAsync(int id);
        Task<string> UploadBusinessCategoryAsync(List<byte[]> record);
        byte[] GenerateExportBusinessCategory();
    }
}
