﻿using Deposit.DomainObjects;
using GODP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
    public interface IDepositBankClosure
    {
        #region DepositBankClosureSetup
        bool AddUpdateBankClosureSetup(deposit_bankclosuresetup entity);
        IEnumerable<deposit_bankclosuresetup> GetAllBankClosureSetup();
        deposit_bankclosuresetup GetBankClosureSetup(int id);
        byte[] GenerateExportBankClosure();
        bool UploadBankClosure(byte[] record, string createdBy);
        bool DeleteBankClosureSetup(int CustomerId);
        #endregion

        #region DepositBankClosure
        Task<bool> AddUpdateDepositBankClosure(deposit_bankclosure model);
        Task<IEnumerable<deposit_bankclosure>> GetAllDepositBankClosureAsync();
        deposit_bankclosure GetDepositBankClosure(int id); 
        bool DeleteDepositBankClosure(int CustomerId);
        Task<IEnumerable<deposit_bankclosure>> GetBankAccountClosureAwaitingApprovalAsync(List<long> prnIds, List<string> tokens);
        #endregion



    }
}
