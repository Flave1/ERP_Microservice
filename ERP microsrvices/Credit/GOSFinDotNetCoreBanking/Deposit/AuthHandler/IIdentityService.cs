using Deposit.Contracts.Response;
using Deposit.Contracts.Response.IdentityServer;
using Deposit.DomainObjects.Auth;
using GOSLibraries.GOS_Financial_Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Deposit.AuthHandler.Interface
{
    public interface IIdentityService
    {

        Task<bool> OTPDateExpiredAsync(string otp);
        Task<OTPTracker> GetSingleOtpTrackAsync(string otp);
        Task<AuthenticationResult> CustomerRegisterAsync(CustomUserRegistrationReqObj userRegistration);
        Task<AuthenticationResult> CustomerChangePasswsord(ChangePassword pass);
        Task<AuthenticationResult> CustomerLoginAsync(ApplicationUser user);
        Task<AuthenticationResult> CustomerRefreshTokenAsync(string refreshToken, string token);
        Task<bool> CustomerCheckUserAsync(string email);
        Task<bool> CustomerCheckUserAsyncByUserId(string userId);
        Task<UserDataResponseObj> CustomerFetchLoggedInUserDetailsAsync(string userId);
        Task<ConfirnmationResponse> CustomerConfirmEmailAsync(ConfirnmationRequest request); 
        Task<bool> verifyCustomerEmailAccount(string userId);
        Task<bool> SendOTPToEmailAsync(ApplicationUser user);
        Task<bool> RemoveOtpAsync(string otp);
        Task<bool> PerformLockFunction(string user, DateTime unlockat, bool isQuestionTime);
        Task<bool> UnlockUserAsync(string userid);
        Task<bool> ReturnStatusAsync(string userid);


    }
}
