using AutoMapper;
using Deposit.AuthHandler.Interface;
using Deposit.Contracts.Response.Deposit;
using Deposit.Contracts.V1;
using Deposit.Handlers.Auths;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
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
using Deposit.Contracts.GeneralExtension;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class CashierTellerController : Controller
    {
        private readonly ICashierTellerService _repo;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _serverRequest;
        private readonly DataContext _dataContext;

        public CashierTellerController(
            ICashierTellerService repo, 
            IIdentityService identityService, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor, ILoggerService logger,
            IIdentityServerRequest serverRequest,
            DataContext dataContext)
        {
            _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _serverRequest = serverRequest;
            _dataContext = dataContext;
        }
        #region CashierTellerSetup
        [HttpGet(ApiRoutes.CashierTeller.GET_ALL_CASHIERTELLERSETUP)]

        public async Task<ActionResult<CashierTellerSetupRespObj>> GetAllCashierTellerSetupAsync()
        {
            var response = await _repo.GetAllCashierTellerSetupAsync();
           return Ok(new CashierTellerSetupRespObj
            {
                DepositCashierTellerSetups = response.ToList(),
            });
        }

        [HttpGet(ApiRoutes.CashierTeller.GET_CASHIERTELLERSETUP_BY_ID)]
        public ActionResult<CashierTellerSetupRespObj> GetCashierTellerSetupById([FromQuery] SearchObj search)
        {
            var response = new CashierTellerSetupRespObj { DepositCashierTellerSetups = new List<CashierTellerSetupObj>(), Status = new APIResponseStatus() };
            if (search.SearchId < 1)
            {
                response.Status.Message.FriendlyMessage = "CashierTeller Id is required";
                return response;
            }
            var result = _repo.GetCashierTellerSetupById(search.SearchId);
            response.Status.IsSuccessful = true;
            response.DepositCashierTellerSetups.Add(result);
            return response; 
        }

        [HttpGet(ApiRoutes.CashierTeller.DOWNLOAD_CASHIERTELLERSETUP)]
        public ActionResult<CashierTellerSetupRespObj> GenerateExportCashierTellerSetup()
        {
            var response = _repo.GenerateExportCashierTellerSetup();

            return new CashierTellerSetupRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.CashierTeller.ADD_UPDATE_CASHIERTELLERSETUP)]
        public async Task<ActionResult<CashierTellerSetupRegRespObj>> AddUpDateCashierTellerSetup([FromBody] AddUpdateCashierTellerSetupObj model)
        {
            try
            {  
                var domainObj = _dataContext.deposit_cashiertellersetup.Find(model.DepositCashierTellerSetupId);
                if(domainObj == null)
                    domainObj = new deposit_cashiertellersetup();

                domainObj.DepositCashierTellerSetupId = model.DepositCashierTellerSetupId; ;
                domainObj.Structure = model.Structure; 
                domainObj.ProductId = model.ProductId;
                domainObj.PresetChart = model.PresetChart;
                domainObj.Employee_ID = model.Employee_ID;
                domainObj.Sub_strructure = model.Sub_strructure;


                await _repo.AddUpdateCashierTellerSetupAsync(domainObj);
                return new CashierTellerSetupRegRespObj
                {
                    DepositCashierTellerSetupId = domainObj.DepositCashierTellerSetupId,
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
            }
        }

        [HttpPost(ApiRoutes.CashierTeller.UPLOAD_CASHIERTELLERSETUP)]
        public async Task<ActionResult<CashierTellerSetupRegRespObj>> UploadCashierTellerSetupAsync()
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
            await _repo.UploadCashierTellerSetupAsync(byteList);
            return new CashierTellerSetupRegRespObj
            {
                Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful" } }
            };
        }

        [HttpPost(ApiRoutes.CashierTeller.DELETE_CASHIERTELLERSETUP)]
        public async Task<IActionResult> DeleteCashierTellerSetup([FromBody] DeleteRequest item)
        {
            var response = new Contracts.GeneralExtension.Delete_response();
            foreach (var id in item.ItemIds)
            {
                await _repo.DeleteCashierTellerSetupAsync(id);
            }
            response.Deleted = true;
            response.Status.IsSuccessful = true;
            response.Status.Message.FriendlyMessage = "successful";
            return Ok(response); 
        }
        #endregion
    }
}
