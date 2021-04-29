﻿using Deposit.Contracts.GeneralExtension;
using Deposit.Contracts.Response.IdentityServer;
using Deposit.Contracts.Response.Mail;
using Deposit.DomainObjects.Auth;
using Deposit.Requests;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.URI;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid.Recovery
{
    public class RecoverAccountByEmailCommand : IRequest<RecoveryResp>
    {
        public string Email { get; set; }
        public class RecoverAccountByEmailCommandHandler : IRequestHandler<RecoverAccountByEmailCommand, RecoveryResp>
        {
            private async Task RecoveryMail(string email,string token)
            { 
                var path = $"{_uRIs.SelfClient}#/auth/change/password?email={email}&token={token}";
                var sm = new MailObj();
                sm.subject = $"Account Recovery";
                sm.content = $"Please click <a href='{path}'> here </a> to change password";
                sm.sendIt = true;
                sm.saveIt = true;
                sm.template = (int)EmailTemplate.LoginDetails;
                sm.toAddresses = new List<ToAddress>();
                sm.fromAddresses = new List<FromAddress>();
                sm.toAddresses.Add(new ToAddress { address = email, name = email });
                await _identityServer.SendMail(sm);
            }

            private readonly IBaseURIs _uRIs;
            private readonly IIdentityServerRequest _identityServer;
            private readonly UserManager<ApplicationUser> _userManager;
            public RecoverAccountByEmailCommandHandler(
                IBaseURIs uRIs,
                UserManager<ApplicationUser> userManager, 
                IIdentityServerRequest  identityServer)
            {
                _uRIs = uRIs;
                _identityServer = identityServer;
                _userManager = userManager;
            }
            public async Task<RecoveryResp> Handle(RecoverAccountByEmailCommand request, CancellationToken cancellationToken)
            {
                var response = new RecoveryResp { Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage() } };
                try
                {
                    var user = await _userManager.FindByEmailAsync(request.Email);

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var encodedToken = CustomEncoder.Base64Encode(token);
                    await RecoveryMail(request.Email, encodedToken);
                    response.Status.IsSuccessful = true;
                    response.Status.Message.FriendlyMessage = "Link to reset password has been sent to your email";
                    return response;
                }
                catch (Exception ex)
                {
                    response.Status.Message.FriendlyMessage = "Unable to process request";
                    response.Status.Message.TechnicalMessage = ex.ToString();
                    return response;
                }
            }
        }
    }
   
}
