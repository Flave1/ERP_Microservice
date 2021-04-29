using Deposit.Contracts.Response.Deposit;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
    public interface IChangeOfRatesService
    {
        #region ChangeOfRateSetup
        Task<bool> AddUpdateChangeOfRatesSetupAsync(deposit_changeofratesetup model);

        Task<bool> DeleteChangeOfRatesSetupAsync(int id);

        Task<IEnumerable<ChangeOfRateSetupObj>> GetAllChangeOfRatesSetupAsync();

        Task<deposit_changeofratesetup> GetChangeOfRatesSetupByIdAsync(int id);

        Task<string> UploadChangeOfRatesSetupAsync(List<byte[]> record);
        public byte[] GenerateExportChangeOfRatesSetup();
        #endregion

        #region ChangeOfRateForm
        Task<bool> AddUpdateChangeOfRatesAsync(deposit_changeofrates model);

        Task<bool> DeleteChangeOfRatesAsync(int id);

        Task<IEnumerable<deposit_changeofrates>> GetAllChangeOfRatesAsync();

        Task<deposit_changeofrates> GetChangeOfRatesByIdAsync(int id);

        /*Task<bool> UploadChangeOfRatesAsync(List<byte[]> record, string createdBy);
        public byte[] GenerateExportChangeOfRates();*/
        #endregion
    }
}
