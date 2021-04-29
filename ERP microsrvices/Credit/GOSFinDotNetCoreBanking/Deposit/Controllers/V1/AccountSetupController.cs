using Deposit.Contracts.Response.Deposit;
using Deposit.Contracts.V1;
using Deposit.Repository.Interface.Deposit;
using AutoMapper;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GOSLibraries;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Deposit.AuthHandler.Interface;
using Deposit.Requests;
using Microsoft.AspNetCore.Http;
using GOSLibraries.GOS_Error_logger.Service;
using Deposit.Handlers.Auths;
using Deposit.Data;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class AccountSetupController : Controller
    {
        private readonly IAccountSetupService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;
        private readonly DataContext _dataContext;

        public AccountSetupController(IAccountSetupService repo, DataContext dataContext, IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
            _dataContext = dataContext;
        }

        #region DEPOSIT_ACCOUNT_SETUP
        [HttpGet(ApiRoutes.AccountSetup.GET_ALL_ACCOUNTSETUP)]
        public async Task<ActionResult<AccountSetupRespObj>> GetAllAccountSetupAsync()
        {
            try
            {
                var response = await _repo.GetAllAccountSetupAsync();
                return new AccountSetupRespObj
                {
                    DepositAccounts = response,
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new AccountSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.AccountSetup.GET_ACCOUNTSETUP_BY_ID)]
        public async Task<ActionResult<AccountSetupRespObj>> GetAccountSetupByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new AccountSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "AccountSetup Id is required" } }
                };
            }

            var response = await _repo.GetAccountSetupByIdAsync(search.SearchId);
            var resplist = new List<DepositAccountObj> { response };
            return new AccountSetupRespObj
            {
                DepositAccounts = resplist,
                Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage()}
            };
            
        }

        [HttpPost(ApiRoutes.AccountSetup.ADD_UPDATE_ACCOUNTSETUP)]
        public async Task<ActionResult<AccountSetupRegRespObj>> AddUpDateAccountSetup([FromBody] AddUpdateAccountSetupObj model)
        {
            var resp = new AccountSetupRegRespObj();
            try
            { 
                await _repo.AddUpdateAccountSetupAsync(model);
                resp.DepositAccountId = 1;
                resp.Status.IsSuccessful = true;
                resp.Status.Message.FriendlyMessage = "Successful";
                return resp;
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
                 
            }
        }

        [HttpPost(ApiRoutes.AccountSetup.DELETE_ACCOUNTSETUP)]

        public async Task<IActionResult> DeleteAccountSetup([FromBody] DeleteRequest item)
        {
            try
            {
                foreach (var id in item.ItemIds)
                {
                    await _repo.DeleteAccountSetupAsync(id);
                }
                return Ok(
                new DeleteRespObjt
                {
                    Deleted = true,
                    Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new DeleteRespObjt
                {
                    Deleted = false,
                    Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful", TechnicalMessage = ex.ToString() } }
                });
            }
        }

        [HttpGet(ApiRoutes.AccountSetup.DOWNLOAD_ACCOUNTSETUP)]
        public async Task<ActionResult<AccountSetupRespObj>> GenerateExportAccountSetup()
        {
            var response = _repo.GenerateExportAccountSetup();

            return new AccountSetupRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.AccountSetup.UPLOAD_ACCOUNTSETUP)]
        public async Task<ActionResult<AccountSetupRegRespObj>> UploadAccountSetupAsync()
        {
            var response = new AccountSetupRegRespObj();
            var files = _httpContextAccessor.HttpContext.Request.Form.Files;

            var byteList = new List<byte[]>();
            foreach (var fileBit in files) 
                if (fileBit.Length > 0) 
                    using (var ms = new MemoryStream())
                    {
                        await fileBit.CopyToAsync(ms);
                        byteList.Add(ms.ToArray());
                        ms.Flush();
                    } 

            var res = await _repo.UploadAccountSetupAsync(byteList);
            if (res != "uploaded")
            {
                response.Status.Message.FriendlyMessage = "res";
                return response;
            }
            response.Status.IsSuccessful = true;
            response.Status.Message.FriendlyMessage = "successful";
            return response; 
        }
        #endregion
         
    }
}
