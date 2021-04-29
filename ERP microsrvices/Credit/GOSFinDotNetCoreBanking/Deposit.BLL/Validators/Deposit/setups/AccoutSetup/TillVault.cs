using Deposit.Handlers.TillVaults;
using Deposit.Data;
using FluentValidation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.AccoutSetup
{
    public class TillVault : AbstractValidator<AddUpdateTillVaultCommand>
    {
        private readonly DataContext _dataContext;
        public TillVault(DataContext dataContext)
        {
            _dataContext = dataContext;
            RuleFor(e => e.Structure).NotEmpty();
            RuleFor(e => e).MustAsync(NoDuplicateAsync).WithMessage("Duplicate setup deteted");
        }
        private async Task<bool> NoDuplicateAsync(AddUpdateTillVaultCommand request, CancellationToken cancellationToken)
        {
            if (request.TillVaultSetupId > 0)
            {
                var item = _dataContext.deposit_tillvaultsetup.FirstOrDefault(e => e.Structure == request.Structure && e.TillVaultSetupId != request.TillVaultSetupId && e.Deleted == false);
                if (item != null)
                {
                    return await Task.Run(() => false);
                }
                return await Task.Run(() => true);
            }
            if (_dataContext.deposit_tillvaultsetup.Count(e => e.Structure == request.Structure && e.Deleted == false) >= 1)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
    }
}
