using Deposit.Contracts.Response.Deposit;
using Deposit.Contracts.V1;
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
using Microsoft.AspNetCore.Http;
using GOSLibraries.GOS_Error_logger.Service;
using Deposit.Requests;
using Deposit.Handlers.Auths;
using Deposit.Data;
using Deposit.Managers.Interface;
using Deposit.Repository.Interface;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class TransactionChargeController : Controller
    {
        private readonly ITransactionChargeService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly DataContext _dataContext;
        public TransactionChargeController(
            ITransactionChargeService transactionChargeervice, 
            IMapper mapper, 
            IIdentityService identityService, 
            IHttpContextAccessor httpContextAccessor, 
            ILoggerService logger,
            DataContext dataContext,
            IIdentityServerRequest serverRequest)
        {
            _mapper = mapper;
            _serverRequest = serverRequest;
            _repo = transactionChargeervice;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _dataContext = dataContext;
        }

        [HttpGet(ApiRoutes.TransactionCharge.GET_ALL_TRANSACTIONCHARGE)]

        public async Task<ActionResult<TransactionChargeRespObj>> GetAllTransactionChargeAsync()
        {
            try
            {
                var response = await _repo.GetAllTransactionChargeAsync();
                return new TransactionChargeRespObj
                {
                    TransactionCharges = _mapper.Map<List<TransactionChargeObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new TransactionChargeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.TransactionCharge.GET_TRANSACTIONCHARGE_BY_ID)]
        public async Task<ActionResult<TransactionChargeRespObj>> GetTransactionChargeByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new TransactionChargeRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "TransactionCharge Id is required" } }
                };
            }

            var response = await _repo.GetTransactionChargeByIdAsync(search.SearchId);
            var resplist = new List<deposit_transactioncharge> { response };
            return new TransactionChargeRespObj
            {
                TransactionCharges = _mapper.Map<List<TransactionChargeObj>>(resplist),
            };
        }

        [HttpPost(ApiRoutes.TransactionCharge.ADD_UPDATE_TRANSACTIONCHARGE)]
        public ActionResult<TransactionChargeRegRespObj> AddUpDateTransactionCharge([FromBody] AddUpdateTransactionChargeObj model)
        {
            var response = new TransactionChargeRegRespObj();
            try
            {
                var domainObj = _dataContext.deposit_transactioncharge.Find(model.TransactionChargeId);
                if(domainObj == null)
                    domainObj = new deposit_transactioncharge();

                domainObj.TransactionChargeId = model.TransactionChargeId;
                domainObj.Description = model.Description;
                domainObj.FixedOrPercentage = model.FixedOrPercentage;
                domainObj.Amount_Percentage = model.Amount_Percentage;
                domainObj.Name = model.Name;  

                _repo.AddUpdateTransactionCharge(domainObj);
                response.Status.IsSuccessful = true;
                response.Status.Message.FriendlyMessage = "successful";
                return response;
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
            }
        }

        [HttpPost(ApiRoutes.TransactionCharge.DELETE_TRANSACTIONCHARGE)]

        public async Task<IActionResult> DeleteTransactionCharge([FromBody] DeleteRequest item)
        {
            var response = new TransactionChargeRegRespObj();
            try
            {
                foreach (var id in item.ItemIds)
                {
                    await _repo.DeleteTransactionChargeAsync(id);
                }
                response.Status.IsSuccessful = true;
                response.Status.Message.FriendlyMessage = "successful";
                return Ok(response);
             
            }
            catch (Exception e)
            {
                throw e;
            } 
        }

        [HttpGet(ApiRoutes.TransactionCharge.DOWNLOAD_TRANSACTIONCHARGE)]
        public ActionResult<TransactionChargeRespObj> GenerateExportTransactionCharge()
        {
            var resp = new TransactionChargeRespObj();
            var response = _repo.GenerateExportTransactionCharge();
            resp.export = response;
            return resp;
        }

        [HttpPost(ApiRoutes.TransactionCharge.UPLOAD_TRANSACTIONCHARGE)]
        public async Task<ActionResult<TransactionChargeRegRespObj>> UploadTransactionChargeAsync()
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
                await _repo.UploadTransactionChargeAsync(byteList);
                return new TransactionChargeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new TransactionChargeRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

    }
}
