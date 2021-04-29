using Deposit.DomainObjects;
using Deposit.Repository.Interface.Deposit;
using Deposit.Data;
using GODP.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Implement
{
    public class DepositBankClosure : IDepositBankClosure
    {
        private readonly DataContext _dataContext; public DepositBankClosure(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool AddUpdateBankClosureSetup(deposit_bankclosuresetup entity)
        {
            if (entity.BankClosureSetupId > 0)
            {
                var item = _dataContext.deposit_bankclosuresetup.Find(entity.BankClosureSetupId);
                _dataContext.Entry(item).CurrentValues.SetValues(entity);
            }
            else
                _dataContext.deposit_bankclosuresetup.Add(entity);
            return _dataContext.SaveChanges() > 0;
        }

        public async Task<bool> AddUpdateDepositBankClosure(deposit_bankclosure model)
        {
           
            if (model.BankClosureId > 0)
            {
                _dataContext.Entry(model).CurrentValues.SetValues(model);
            }
            else
                _dataContext.deposit_bankclosure.Add(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public bool DeleteBankClosureSetup(int CustomerId)
        {
            var item = _dataContext.deposit_bankclosuresetup.Find(CustomerId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(item);
            return _dataContext.SaveChanges() > 0;
        }

        public bool DeleteDepositBankClosure(int CustomerId)
        {
            var item = _dataContext.deposit_bankclosure.Find(CustomerId);
            item.Deleted = true;
            _dataContext.Entry(item).CurrentValues.SetValues(item);
            return _dataContext.SaveChanges() > 0;
        }

        public bool DeleteMultipleDepositBankClosure(List<int> deposit_bankclosureIds)
        {
            throw new NotImplementedException();
        }

       

        public deposit_bankclosure GetDepositBankClosure(int id)
        {
            throw new NotImplementedException();
        }

        public bool UploadBankClosure(byte[] record, string createdBy)
        {
            throw new NotImplementedException();
        }


        public byte[] GenerateExportBankClosure()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<deposit_bankclosuresetup> GetAllBankClosureSetup()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<deposit_bankclosure> GetAllDepositBankClosure()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<deposit_bankclosure>> GetAllDepositBankClosureAsync()
        {
            throw new NotImplementedException();
        }

        public deposit_bankclosuresetup GetBankClosureSetup(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<deposit_bankclosure>> GetBankAccountClosureAwaitingApprovalAsync(List<long> targetIds, List<string> tokens)
        {
            var item = await _dataContext.deposit_bankclosure
                .Where(s => targetIds.Contains(s.BankClosureId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }
    }
}
