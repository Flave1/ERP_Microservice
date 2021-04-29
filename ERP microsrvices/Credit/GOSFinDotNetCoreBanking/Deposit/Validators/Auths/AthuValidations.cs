using Deposit.Contracts.Response.Auth;
using Deposit.DomainObjects.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Identity; 
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Validators.Auths
{
    public class RegistrationCommandValidator : AbstractValidator<RegistrationCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public RegistrationCommandValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            RuleFor(a => a.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password)
                .NotNull()
                .MinimumLength(8)
                .MaximumLength(16)
                .MustAsync(IsPasswordCharactersValid).WithMessage("Invalid Password");

            RuleFor(c => c).MustAsync(UserExist).WithMessage("User With This Email Already Exist");
            // .MustAsync(IsValidPassword).WithMessage("User/Password Combination is wrong");
        }


        private async Task<bool> UserExist(RegistrationCommand request, CancellationToken cancellation)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return await Task.Run(() => true);
            }
            return await Task.Run(() => false);
        }
        private async Task<bool> IsPasswordCharactersValid(string password, CancellationToken cancellationToken)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return await Task.Run(() => hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password));
        }
    }
}
