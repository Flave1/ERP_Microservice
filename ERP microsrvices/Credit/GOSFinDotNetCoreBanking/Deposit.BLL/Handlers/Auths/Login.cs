using Deposit.AuthHandler.Interface;
using Deposit.Contracts.Response.IdentityServer;
using Deposit.DomainObjects.Auth;
using Deposit.Requests;
using Deposit.Data;
using GOSLibraries;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger;
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_Financial_Identity;
using GOSLibraries.Options;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Deposit.Handlers.Auths
{

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IIdentityServerRequest _service; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDetectionService _detectionService;
        private readonly DataContext _securityContext;
        private readonly ILoggerService _logger;
        private readonly IIdentityService _identityService;
        public LoginCommandHandler(
            IIdentityServerRequest identityRepoService,
            UserManager<ApplicationUser> userManager, 
            DataContext dataContext,
            IIdentityService identityService,
            IDetectionService detectionService,
            ILoggerService loggerService)
        {
            _userManager = userManager; 
            _service = identityRepoService; 
            _securityContext = dataContext;
            _logger = loggerService;
            _detectionService = detectionService;
            _identityService = identityService;
        }

        public async Task<AuthResponse> OTPOptionsAsync(string userid)
        {
            try
            { 
                var response = new AuthResponse { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                var settings = await _service.GetSettingsAsync()??new SecurityResp { authSettups = new List<Security>()};
                if(settings.authSettups.Count() > 0)
                {
                    var multiplefFA = settings.authSettups.Where(a => a.Module == (int)Modules.CREDIT).ToList();
                    var user = await _userManager.FindByIdAsync(userid);
                    if (multiplefFA.Count() > 0)
                    {
                        if (_detectionService.Device.Type.ToString().ToLower() == Device.Desktop.ToString().ToLower())
                        {
                            if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.EMAIL).ActiveOnWebApp)
                            {
                                await _identityService.SendOTPToEmailAsync(user);
                                response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your email";
                                return response;
                            }
                            if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.SMS) != null && multiplefFA.FirstOrDefault(a => a.Media == (int)Media.SMS).ActiveOnWebApp)
                            {
                                response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your number";
                                return response;
                            }
                        }
                        if (_detectionService.Device.Type.ToString().ToLower() == Device.Mobile.ToString().ToLower())
                        {
                            if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.EMAIL).ActiveOnMobileApp)
                            {
                                await _identityService.SendOTPToEmailAsync(user);
                                response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your email";
                                return response;
                            }
                            if (multiplefFA.FirstOrDefault(a => a.Media == (int)Media.SMS).ActiveOnMobileApp)
                            {
                                response.Status.Message.FriendlyMessage = "OTP Verification Code sent to your number";
                                return response;
                            }
                        }
                    }
                }
                
                response.Status.IsSuccessful = false;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new AuthResponse { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
            try
            {
                if (!await _identityService.ReturnStatusAsync(request.UserName))
                { 
                    return response;
                }
                if (!await IsPasswordCharactersValid(request.Password))
                { 
                    response.Status.Message.FriendlyMessage = "Invalid Password";
                    return response;
                }
                if (!await UserExist(request))
                { 
                    response.Status.Message.FriendlyMessage = "User does not exist";
                    return response;
                }
                if (!await IsValidPassword(request))
                { 
                    response.Status.Message.FriendlyMessage = "User/Password Combination is wrong";
                    return response;
                }

                var user = await _userManager.FindByNameAsync(request.UserName);


                var otp = await OTPOptionsAsync(user.Id);
                if (otp.Status.IsSuccessful)
                {
                    response.Status.IsSuccessful = true;
                    otp.Status.Message.MessageId = user.Email;
                    return otp;
                }

                var result = await _identityService.CustomerLoginAsync(user);

                response.Status.IsSuccessful = true;
                response.Token = result.Token;
                response.RefreshToken = result.RefreshToken;
                return response;
            }
            catch (Exception ex)
            {
                response.Status.Message.FriendlyMessage = ex?.Message ?? ex?.InnerException?.Message;
                response.Status.Message.TechnicalMessage = ex.ToString();
                return response;
            }
        }

        private async Task<bool> IsValidPassword(LoginCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var isValidPass = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValidPass)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> UserExist(LoginCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return await Task.Run(() => false);
            }
            return await Task.Run(() => true);
        }
        private async Task<bool> IsPasswordCharactersValid(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return await Task.Run(() => hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password));
        }

    }

}
