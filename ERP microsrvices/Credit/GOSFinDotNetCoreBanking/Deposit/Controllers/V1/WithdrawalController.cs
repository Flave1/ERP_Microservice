using AutoMapper;
using Deposit.AuthHandler.Interface;
using Deposit.Contracts.Response.Deposit;
using Deposit.Contracts.V1;
using Deposit.Repository.Interface.Deposit;
using GODP.Entities.Models;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Deposit.Requests;
using Deposit.Handlers.Auths;
using Deposit.Contracts.Response.Common;
using Deposit.Data;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class WithdrawalController : Controller
    {
        private readonly IWithdrawalService _repo;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;
        private readonly DataContext _dataContext;


        public WithdrawalController(IWithdrawalService repo, DataContext data, IIdentityService identityService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
            _dataContext = data;
        }

        #region WithdrawalSetup
        [HttpGet(ApiRoutes.Withdrawal.GET_ALL_WITHDRAWAL_SETUP)]
        public async Task<ActionResult<WithdrawalSetupRespObj>> GetAllWithdrawalSetupAsync()
        {
            var response = await _repo.GetAllWithdrawalSetupAsync(); 
            return Ok(new WithdrawalSetupRespObj
            {
                WithdrawalSetups = response,
                Status = new APIResponseStatus { IsSuccessful = true}
            });
        }

        [HttpGet(ApiRoutes.Withdrawal.GET_WITHDRAWAL_SETUP_BY_ID)]
        public async Task<ActionResult<WithdrawalSetupRespObj>> GetWithdrawalSetupByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new WithdrawalSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "WithdrawalSetup Id is required" } }
                };
            }

            var response = await _repo.GetWithdrawalSetupByIdAsync(search.SearchId);
            var resplist = new List<deposit_withdrawalsetup> { response };
            return new WithdrawalSetupRespObj
            {
                WithdrawalSetups = _mapper.Map<List<WithdrawalSetupObj>>(resplist),
            };

        }

        [HttpGet(ApiRoutes.Withdrawal.DOWNLOAD_WITHDRAWAL_SETUP)]
        public async Task<ActionResult<DownloadResponse>> GenerateExportWithdrawalSetup()
        {
            var response = new DownloadResponse { ExcelFile = new byte[0], Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };

            response.ExcelFile = _repo.GenerateExportWithdrawalSetup();

            return response;
        }

        [HttpPost(ApiRoutes.Withdrawal.ADD_UPDATE_WITHDRAWAL_SETUP)]
        public async Task<ActionResult<WithdrawalSetupRegRespObj>> AddUpdateWithdrawalSetupAsync([FromBody] AddUpdateWithdrawalSetupObj model)
        {
            try
            {
                var domainObj = _dataContext.deposit_withdrawalsetup.Find(model.WithdrawalSetupId);
                if(domainObj == null)
                    domainObj = new deposit_withdrawalsetup();

                domainObj.WithdrawalSetupId = model.WithdrawalSetupId;
                domainObj.Structure = model.Structure;
                domainObj.Product = model.Product;
                domainObj.PresetChart = model.PresetChart;
                domainObj.AccountType = model.AccountType;
                domainObj.DailyWithdrawalLimit = model.DailyWithdrawalLimit;
                domainObj.WithdrawalCharges = model.WithdrawalCharges;
                domainObj.Charge = model.Charge;
                domainObj.Amount = model.Amount;
                domainObj.ChargeType = model.ChargeType;  

                await _repo.AddUpdateWithdrawalSetupAsync(domainObj);
                return new WithdrawalSetupRegRespObj
                {
                    WithdrawalSetupId = domainObj.WithdrawalSetupId,
                    Status = new APIResponseStatus { IsSuccessful = true , Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new WithdrawalSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.Withdrawal.UPLOAD_WITHDRAWAL_SETUP)]
        public async Task<ActionResult<WithdrawalSetupRegRespObj>> UploadWithdrawalSetupAsync()
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

            var res = await _repo.UploadWithdrawalSetupAsync(byteList);
            if (res != "success")
            {
                return new WithdrawalSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = res } }
                };
            }
            return new WithdrawalSetupRegRespObj
            {
                Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
            };
        }

        [HttpPost(ApiRoutes.Withdrawal.DELETE_WITHDRAWAL_SETUP)]
        public async Task<IActionResult> DeleteWithdrawalSetupAsync([FromBody] DeleteRequest item)
        { 
           
            try
            {
                foreach (var id in item.ItemIds)
                {
                    await _repo.DeleteWithdrawalSetupAsync(id);
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
                return BadRequest(
                  new DeleteRespObjt
                  {
                      Deleted = false,
                      Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = $"Unsuccessful : {ex.Message}", TechnicalMessage = ex.ToString() } }
                  });
            }
               
        }
        #endregion

        //#region WithdrawalForm
        //[HttpGet(ApiRoutes.Withdrawal.GET_ALL_WITHDRAWAL)]
        //public async Task<ActionResult<WithdrawalFormRespObj>> GetAllWithdrawalAsync()
        //{
        //    try
        //    {
        //        var response = await _repo.GetAllWithdrawalAsync();
        //        return new WithdrawalFormRespObj
        //        {
        //            WithdrawalForms = _mapper.Map<List<WithdrawalFormObj>>(response),
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        var errorCode = ErrorID.Generate(5);
        //        return new WithdrawalFormRespObj
        //        {
        //            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
        //        };
        //    }
        //}

        //[HttpGet(ApiRoutes.Withdrawal.GET_WITHDRAWAL_BY_ID)]
        //public async Task<ActionResult<WithdrawalFormRespObj>> GetWithdrawalByIdAsync([FromQuery] SearchObj search)
        //{
        //    if (search.SearchId < 1)
        //    {
        //        return new WithdrawalFormRespObj
        //        {
        //            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "ChangeOfRates Id is required" } }
        //        };
        //    }

        //    var response = await _repo.GetWithdrawalByIdAsync(search.SearchId);
        //    var resplist = new List<deposit_withdrawalform> { response };
        //    return new WithdrawalFormRespObj
        //    {
        //        WithdrawalForms = _mapper.Map<List<WithdrawalFormObj>>(resplist),
        //    };

        //}

        //[HttpPost(ApiRoutes.Withdrawal.ADD_UPDATE_WITHDRAWAL)]
        //public async Task<ActionResult<WithdrawalFormRegRespObj>> AddUpDateWithdrawal([FromBody] AddUpdateWithdrawalFormObj model)
        //{
        //    try
        //    {
        //        var user = await _identityServer.UserDataAsync();
        //        deposit_withdrawalform item = null;
        //        if (model.WithdrawalFormId > 0)
        //        {
        //            item = await _repo.GetWithdrawalByIdAsync(model.WithdrawalFormId);
        //            if (item == null)
        //                return new WithdrawalFormRegRespObj
        //                {
        //                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Item does not Exist" } }
        //                };
        //        }

        //        var domainObj = new deposit_withdrawalform();

        //        domainObj.WithdrawalFormId = model.WithdrawalFormId > 0 ? model.WithdrawalFormId : 0;
        //        domainObj.Structure = model.Structure;
        //        domainObj.Product = model.Product;
        //        domainObj.TransactionReference = model.TransactionReference;
        //        domainObj.AccountNumber = model.AccountNumber;
        //        domainObj.AccountType = model.AccountType;
        //        domainObj.Currency = model.Currency;
        //        domainObj.Amount = model.Amount;
        //        domainObj.TransactionDescription = model.TransactionDescription;
        //        domainObj.TransactionDate = model.TransactionDate;
        //        domainObj.ValueDate = model.ValueDate;
        //        domainObj.WithdrawalType = model.WithdrawalType;
        //        domainObj.InstrumentNumber = model.InstrumentNumber;
        //        domainObj.InstrumentDate = model.InstrumentDate;
        //        domainObj.ExchangeRate = model.ExchangeRate;
        //        domainObj.TotalCharge = model.TotalCharge;
        //        domainObj.Active = true;
        //        domainObj.CreatedOn = DateTime.Today;
        //        domainObj.CreatedBy = user.UserName;
        //        domainObj.Deleted = false;
        //        domainObj.UpdatedOn = model.WithdrawalFormId > 0 ? DateTime.Today : DateTime.Today;
        //        domainObj.UpdatedBy = user.UserName;


        //        var isDone = await _repo.AddUpdateWithdrawalAsync(domainObj);
        //        return new WithdrawalFormRegRespObj
        //        {
        //            WithdrawalFormId = domainObj.WithdrawalFormId,
        //            Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        var errorCode = ErrorID.Generate(5);
        //        _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
        //        return new WithdrawalFormRegRespObj
        //        {
        //            Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
        //        };
        //    }
        //}

        //[HttpPost(ApiRoutes.Withdrawal.DELETE_WITHDRAWAL)]
        //public async Task<IActionResult> DeleteWithdrawalAsync([FromBody] DeleteRequest item)
        //{
        //    var response = false;
        //    var Ids = item.ItemIds;
        //    foreach (var id in Ids)
        //    {
        //        response = await _repo.DeleteWithdrawalAsync(id);
        //    }
        //    if (!response)
        //        return BadRequest(
        //            new DeleteRespObjt
        //            {
        //                Deleted = false,
        //                Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
        //            });
        //    return Ok(
        //            new DeleteRespObjt
        //            {
        //                Deleted = true,
        //                Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
        //            });
        //}
        //#endregion
    }


}
