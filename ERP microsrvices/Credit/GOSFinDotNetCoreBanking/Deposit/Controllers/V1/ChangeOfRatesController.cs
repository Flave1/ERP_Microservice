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
using MediatR;
using Deposit.Handlers.Deposit.AccountSetup;
using Deposit.Handlers.Deposit.ChangeOfRate;
using Deposit.Handlers.Deposit.BankClosure;
using Deposit.Data;
using Deposit.Contracts.GeneralExtension;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class ChangeOfRatesController : Controller
    {
        private readonly IChangeOfRatesService _repo;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;
        private readonly DataContext _dataContext;
        private readonly IMediator _mediator;


        public ChangeOfRatesController(IChangeOfRatesService repo, IIdentityService identityService, IMediator mediator, DataContext dataContext,
            IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _identityServer = identityServer;
            _dataContext = dataContext;
            _mediator = mediator;
        }

        #region ChangeOfRatesSetup
        [HttpGet(ApiRoutes.ChangeOfRates.GET_ALL_CHANGEOFRATES_SETUP)]
        public async Task<ActionResult<ChangeOfRateSetupRespObj>> GetAllChangeOfRatesSetupAsync()
        {
            var response = new ChangeOfRateSetupRespObj();

            var result = await _repo.GetAllChangeOfRatesSetupAsync();
            response.ChangeOfRateSetups = result.ToList();
            response.Status.IsSuccessful = true;
            return response;
        }

        [HttpGet(ApiRoutes.ChangeOfRates.GET_CHANGEOFRATES_SETUP_BY_ID)]
        public async Task<ActionResult<ChangeOfRateSetupRespObj>> GetChangeOfRatesSetupByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new ChangeOfRateSetupRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "ChangeOfRateSetup Id is required" } }
                };
            }

            var response = await _repo.GetChangeOfRatesSetupByIdAsync(search.SearchId);
            var resplist = new List<deposit_changeofratesetup> { response };
            return new ChangeOfRateSetupRespObj
            {
                ChangeOfRateSetups = _mapper.Map<List<ChangeOfRateSetupObj>>(resplist),
            };

        }

        [HttpGet(ApiRoutes.ChangeOfRates.DOWNLOAD_CHANGEOFRATES_SETUP)]
        public async Task<ActionResult<ChangeOfRateSetupRespObj>> GenerateExportChangeOfRatesSetup()
        {
            var response = _repo.GenerateExportChangeOfRatesSetup();

            return new ChangeOfRateSetupRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.ChangeOfRates.ADD_UPDATE_CHANGEOFRATES_SETUP)]
        public async Task<ActionResult<ChangeOfRateSetupRegRespObj>> AddUpdateChangeOfRatesSetupAsync([FromBody] AddUpdateChangeOfRateSetupObj model)
        {
            try
            {  
                var domainObj = _dataContext.deposit_changeofratesetup.Find(model.ChangeOfRateSetupId);

                if (domainObj == null) 
                    domainObj = new deposit_changeofratesetup();  

                domainObj.ChangeOfRateSetupId = model.ChangeOfRateSetupId; 
                domainObj.ProductId = model.ProductId;
                domainObj.CanApply = model.CanApply;
                domainObj.Structure = model.Structure;

                 await _repo.AddUpdateChangeOfRatesSetupAsync(domainObj);
                return new ChangeOfRateSetupRegRespObj
                {
                    ChangeOfRateSetupId = domainObj.ChangeOfRateSetupId,
                    Status = new APIResponseStatus { IsSuccessful = true , Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
            }
        }

        [HttpPost(ApiRoutes.ChangeOfRates.UPLOAD_CHANGEOFRATES_SETUP)]
        public async Task<ActionResult<ChangeOfRateSetupRegRespObj>> UploadChangeOfRatesSetupAsync()
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

                var user = await _identityServer.UserDataAsync();
                var createdBy = user.UserName;

                var result = await _repo.UploadChangeOfRatesSetupAsync(byteList);
                if(result != "success")
                {
                    return new ChangeOfRateSetupRegRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = result } }
                    };
                }
                return new ChangeOfRateSetupRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "successful"  } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                throw ex;
            }
        }

        [HttpPost(ApiRoutes.ChangeOfRates.DELETE_CHANGEOFRATES_SETUP)]
        public async Task<IActionResult> DeleteChangeOfRatesSetupAsync([FromBody] DeleteRequest item)
        {
            var response = new Contracts.GeneralExtension.Delete_response(); 
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                await _repo.DeleteChangeOfRatesSetupAsync(id);
            }
            response.Status.IsSuccessful = true;
            response.Status.Message.FriendlyMessage = "successful";
            return Ok(response);
        }
        #endregion

        #region TransferForm
        [HttpGet(ApiRoutes.ChangeOfRates.GET_ALL_CHANGEOFRATES)]
        public async Task<ActionResult<ChangeOfRatesRespObj>> GetAllChangeOfRatesAsync()
        {
            var query = new GetAllChangeOfRateQuery();
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpGet(ApiRoutes.ChangeOfRates.GET_CHANGEOFRATES_BY_ID)]
        public async Task<ActionResult<ChangeOfRatesRespObj>> GetChangeOfRatesByIdAsync([FromQuery] GetSingleChangeOfRateQuery query)
        { 
            var res = await _mediator.Send(query);
            return Ok(res); 
        }

        [HttpPost(ApiRoutes.ChangeOfRates.ADD_UPDATE_CHANGEOFRATES)]
        public async Task<ActionResult<ChangeOfRatesRegRespObj>> AddUpDateChangeOfRates([FromBody] AddChangeOfRateCommand command)
        {
            var res = await _mediator.Send(command);
            if (res.Status.IsSuccessful)
                return Ok(res);
            return BadRequest(res);
        }

        [HttpPost(ApiRoutes.ChangeOfRates.DELETE_CHANGEOFRATES)]
        public async Task<IActionResult> DeleteChangeOfRatesAsync([FromBody] DeleteChangeOfRateCommand command)
        {
            var res = await _mediator.Send(command);
            if (res.Status.IsSuccessful)
                return Ok(res);
            return BadRequest(res);
        }

        
        
        [HttpPost(ApiRoutes.ChangeOfRates.STAFF_CHANGEOFRATES)]
        public async Task<IActionResult> STAFF_CHANGEOFRATES([FromBody] ChangeOfRatesStaffApprovalCommand command)
        {
            var res = await _mediator.Send(command);
            if (res.Status.IsSuccessful)
                return Ok(res);
            return BadRequest(res);
        }

        [HttpGet(ApiRoutes.ChangeOfRates.AWAITING_CHANGEOFRATES)]
        public async Task<ActionResult<ChangeOfRatesRespObj>> AWAITING_CHANGEOFRATES([FromQuery] GetChangeOfRateAwaitingApprovalQuery query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }
        #endregion
    }


}
