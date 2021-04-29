﻿using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
    public interface ITransactionChargeService
    {
        Task<bool> AddUpdateTransactionChargeAsync(deposit_transactioncharge model);
        Task<bool> DeleteTransactionChargeAsync(int id);
        Task<IEnumerable<deposit_transactioncharge>> GetAllTransactionChargeAsync();
        Task<deposit_transactioncharge> GetTransactionChargeByIdAsync(int id);
        Task<bool> UploadTransactionChargeAsync(List<byte[]> record);
        byte[] GenerateExportTransactionCharge();
    }
}
