using Deposit.Data;
using GODP.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
    public interface IChangeOfRate
    {
        Task<bool> AddUpdateChangeOfRate(deposit_changeofrates model);
        Task<IEnumerable<deposit_changeofrates>> GetChangeOfRateAwaitingApprovalAsync(List<long> Ids, List<string> tokens);
    }
    public class ChangeOfRate : IChangeOfRate
    {
        private readonly DataContext _dataContext;
        public ChangeOfRate(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> AddUpdateChangeOfRate(deposit_changeofrates model)
        {
            if (model.ChangeOfRateId > 0)
                _dataContext.Entry(model).CurrentValues.SetValues(model);
            else
                _dataContext.deposit_changeofrates.Add(model);
           return  await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_changeofrates>> GetChangeOfRateAwaitingApprovalAsync(List<long> Ids, List<string> tokens)
        {
            var item = await _dataContext.deposit_changeofrates
                .Where(s => Ids.Contains(s.ChangeOfRateId)
                && s.Deleted == false && tokens.Contains(s.WorkflowToken)).ToListAsync();
            return item;
        }
    }
}
