﻿using Deposit.Contracts.Response.Deposit;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
    public interface IWithdrawalService
    {
        #region Withdrawaletup
        Task<bool> AddUpdateWithdrawalSetupAsync(deposit_withdrawalsetup model);

        Task<bool> DeleteWithdrawalSetupAsync(int id);

        Task<List<WithdrawalSetupObj>> GetAllWithdrawalSetupAsync();

        Task<deposit_withdrawalsetup> GetWithdrawalSetupByIdAsync(int id);

        Task<string> UploadWithdrawalSetupAsync(List<byte[]> record);
        byte[] GenerateExportWithdrawalSetup();
        #endregion

        #region WithdrawalForm
        Task<bool> AddUpdateWithdrawalAsync(deposit_withdrawalform model);

        Task<bool> DeleteWithdrawalAsync(int id);

        Task<IEnumerable<deposit_withdrawalform>> GetAllWithdrawalAsync();

        Task<deposit_withdrawalform> GetWithdrawalByIdAsync(int id);

        /*Task<bool> UploadWithdrawalAsync(List<byte[]> record, string createdBy);
        byte[] GenerateExportWithdrawal();*/
        #endregion
    }
}
