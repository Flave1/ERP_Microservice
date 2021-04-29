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
    public class AccountTypeController : Controller
    {
        private readonly IDepositAccountypeService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;
        private readonly DataContext _dataContext;


        public AccountTypeController(IDepositAccountypeService repo, DataContext dataContext,
            IMapper mapper, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
            _dataContext = dataContext;
        }

        [HttpGet(ApiRoutes.AccountType.GET_ALL_ACCOUNT_TYPE)]
        
        public async Task<ActionResult<AccountTypeRespObj>> GetAllAccountTypeAsync()
        {
            try
            {
                var response = await _repo.GetAllAccountTypeAsync();
                return new AccountTypeRespObj
                {
                    AccountTypes = _mapper.Map<List<AccountTypeObj>>(response),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage()
                    }
                }; 
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new AccountTypeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.AccountType.GET_ACCOUNT_TYPE_BY_ID)]
        public async Task<ActionResult<AccountTypeRespObj>> GetAccountTypeByIdAsync([FromQuery] AccountTypeSearchObj search)
        {
            if(search.AccountTypeId < 1)
            {
                return new AccountTypeRespObj
                {
                    Status = new APIResponseStatus{ IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "AccountType Id is required" } }
                };
            }

            var response = await _repo.GetAccountTypeByIdAsync(search.AccountTypeId);
            var resplist = new List<deposit_accountype> { response };
            return new AccountTypeRespObj
            {
                AccountTypes = _mapper.Map<List<AccountTypeObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.AccountType.ADD_UPDATE_ACCOUNT_TYPE)]
        public async Task<ActionResult<AccountTypeRegRespObj>> AddUpDateAccountType([FromBody] AddUpdateAccountTypeObj model)
        {
            try 
            {

                var domainObj = _dataContext.deposit_accountype.Find(model.AccountTypeId);

                if (domainObj == null)
                    domainObj = new deposit_accountype();

                domainObj.AccountTypeId = model.AccountTypeId; 
                domainObj.Description = model.Description;
                domainObj.Name = model.Name;
                domainObj.AccountNunmberPrefix = model.AccountNunmberPrefix;
                 
                await _repo.AddUpdateAccountTypeAsync(domainObj);
                return new AccountTypeRegRespObj
                {
                    AccountTypeId = domainObj.AccountTypeId,
                    Status = new APIResponseStatus { IsSuccessful = true , Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AccountTypeRegRespObj 
                {
                    Status = new APIResponseStatus { IsSuccessful =  false, Message = new APIResponseMessage { FriendlyMessage =  "Error Occurred", TechnicalMessage =ex?.Message , MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.AccountType.DELETE_ACCOUNT_TYPE)]
        public async Task<IActionResult> DeleteAccountType([FromBody] DeleteRequest item)
        { 
            try
            {
                foreach (var id in item.ItemIds)
                {
                   await _repo.DeleteAccountTypeAsync(id);
                }
                return Ok(
                    new DeleteRespObjt
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });
            }
            catch (Exception e)
            {
                return BadRequest(
                  new DeleteRespObjt
                  {
                      Deleted = false,
                      Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = e.Message, TechnicalMessage = e.ToString() } }
                  });
            }
               

        }

        [HttpGet(ApiRoutes.AccountType.DOWNLOAD_ACCOUNT_TYPE)]
        public async Task<ActionResult<AccountTypeRespObj>> GenerateExportAccountType()
        {
            var response = _repo.GenerateExportAccountType();

            return new AccountTypeRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.AccountType.UPLOAD_ACCOUNT_TYPE)]
        public async Task<ActionResult<AccountTypeRegRespObj>> UploadAccountTypeAsync()
        {
            try
            {
                var files = _httpContextAccessor.HttpContext.Request.Form.Files;

                var byteList = new List<byte[]>();
                foreach (var fileBit in files)
                {
                    if (fileBit.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fileBit.CopyToAsync(ms);
                            byteList.Add(ms.ToArray());
                        }
                    }

                } 

               var res = await _repo.UploadAccountTypeAsync(byteList);
                if(res != "uploaded")
                {
                    return new AccountTypeRegRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = res } }
                    };
                }
                return new AccountTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage =  "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new AccountTypeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

    }
}
