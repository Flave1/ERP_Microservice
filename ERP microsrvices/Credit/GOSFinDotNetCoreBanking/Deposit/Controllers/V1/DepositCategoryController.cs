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
using Deposit.Data;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class DepositCategoryController : Controller
    {
        private readonly IDepositCategoryService _repo;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly IIdentityServerRequest _identityServer;
        private readonly DataContext _dataContext;


        public DepositCategoryController(IDepositCategoryService repo, DataContext dataContext, IIdentityService identityService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _dataContext = dataContext;
            _identityServer = identityServer;
        }

        [HttpGet(ApiRoutes.DepositCategory.GET_ALL_DEPOSITCATEGORY)]

        public async Task<ActionResult<CategoryRespObj>> GetAllCategoryAsync()
        {
            try
            {
                var response = await _repo.GetAllCategoryAsync();
                return new CategoryRespObj
                {
                    Categories = _mapper.Map<List<CategoryObj>>(response),
                    Status = new APIResponseStatus
                    {
                        Message = new APIResponseMessage(),
                    }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CategoryRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.DepositCategory.GET_DEPOSITCATEGORY_BY_ID)]
        public async Task<ActionResult<CategoryRespObj>> GetCategoryByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new CategoryRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "DepositCategory Id is required" } }
                };
            }

            var response = await _repo.GetCategoryByIdAsync(search.SearchId);
            var resplist = new List<deposit_category> { response };
            return new CategoryRespObj
            {
                Categories = _mapper.Map<List<CategoryObj>>(resplist),
            };

        }

        [HttpGet(ApiRoutes.DepositCategory.DOWNLOAD_DEPOSITCATEGORY)]
        public async Task<ActionResult<CategoryRespObj>> GenerateExportCategory()
        {
            var response = _repo.GenerateExportCategory();

            return new CategoryRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.DepositCategory.ADD_UPDATE_DEPOSITCATEGORY)]
        public async Task<ActionResult<CategoryRegRespObj>> AddUpDateCategory([FromBody] AddUpdateCategoryObj model)
        {
            try
            {

                var domainObj = _dataContext.deposit_category.Find(model.CategoryId);
                if(domainObj == null)
                    domainObj = new deposit_category();

                domainObj.CategoryId = model.CategoryId;
                domainObj.Name = model.Name;
                domainObj.Description = model.Description; 


                var isDone = await _repo.AddUpdateCategoryAsync(domainObj);
                return new CategoryRegRespObj
                {
                    CategoryId = domainObj.CategoryId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new CategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.DepositCategory.UPLOAD_DEPOSITCATEGORY)]
        public async Task<ActionResult<CategoryRegRespObj>> UploadCategoryAsync()
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
                var res = await _repo.UploadCategoryAsync(byteList);
                if(res != "uploaded")
                {
                    return new CategoryRegRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = res } }
                    };
                }
                return new CategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = true , Message = new APIResponseMessage { FriendlyMessage = "successful"  } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new CategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.DepositCategory.DELETE_DEPOSITCATEGORY)]
        public async Task<IActionResult> DeleteCategory([FromBody] DeleteRequest item)
        {
            var response = false;
            var Ids = item.ItemIds;
            foreach (var id in Ids)
            {
                response = await _repo.DeleteCategoryAsync(id);
            }
            if (!response)
                return BadRequest(
                    new DeleteRespObjt
                    {
                        Deleted = false,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Unsuccessful" } }
                    });
            return Ok(
                    new DeleteRespObjt
                    {
                        Deleted = true,
                        Status = new APIResponseStatus { Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
                    });



        }
    }
}
