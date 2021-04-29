using AutoMapper;
using Deposit.AuthHandler.Interface;
using Deposit.Contracts.Response.Deposit;
using Deposit.Contracts.V1;
using Deposit.Repository.Interface.Deposit;
using GODP.Entities.Models;
using GOSLibraries;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Deposit.Requests;
using GOSLibraries.GOS_Error_logger.Service;
using Deposit.Handlers.Auths;
using Deposit.Data;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class BusinessCategoryController : Controller
    {
        private readonly IBusinessCategoryService _repo;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityServerRequest _identityServer;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;

        public BusinessCategoryController(IBusinessCategoryService repo, DataContext dataContext, IIdentityService identityService, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerService logger, IIdentityServerRequest identityServer)
        {
            _repo = repo;
            _identityService = identityService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _identityServer = identityServer;
            _logger = logger;
            _dataContext = dataContext;
        }

        [HttpGet(ApiRoutes.BusinessCategory.GET_ALL_BUSINESSCATEGORY)]

        public async Task<ActionResult<BusinessCategoryRespObj>> GetAllBusinessCategoryAsync()
        {
            try
            {
                var response = await _repo.GetAllBusinessCategoryAsync();
                return new BusinessCategoryRespObj
                {
                    BusinessCategories = _mapper.Map<List<BusinessCategoryObj>>(response),
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new BusinessCategoryRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpGet(ApiRoutes.BusinessCategory.GET_BUSINESSCATEGORY_BY_ID)]
        public async Task<ActionResult<BusinessCategoryRespObj>> GetBusinessCategoryByIdAsync([FromQuery] SearchObj search)
        {
            if (search.SearchId < 1)
            {
                return new BusinessCategoryRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "BusinessCategory Id is required" } }
                };
            }

            var response = await _repo.GetBusinessCategoryByIdAsync(search.SearchId);
            var resplist = new List<deposit_businesscategory> { response };
            return new BusinessCategoryRespObj
            {
                BusinessCategories = _mapper.Map<List<BusinessCategoryObj>>(resplist),
            };

        }

        [HttpGet(ApiRoutes.BusinessCategory.DOWNLOAD_BUSINESSCATEGORY)]
        public async Task<ActionResult<BusinessCategoryRespObj>> GenerateExportBusinessCategory()
        {
            var response = _repo.GenerateExportBusinessCategory();

            return new BusinessCategoryRespObj
            {
                export = response,
            };
        }

        [HttpPost(ApiRoutes.BusinessCategory.ADD_UPDATE_BUSINESSCATEGORY)]
        public async Task<ActionResult<BusinessCategoryRegRespObj>> AddUpDateBusinessCategory([FromBody] AddUpdateBusinessCategoryObj model)
        {
            try
            {
                var domainObj = _dataContext.deposit_businesscategory.Find(model.BusinessCategoryId);
                if(domainObj == null) 
                    domainObj = new deposit_businesscategory();

                domainObj.BusinessCategoryId = model.BusinessCategoryId;
                domainObj.Name = model.Name;
                domainObj.Description = model.Description;  
                

            var isDone = await _repo.AddUpdateBusinessCategoryAsync(domainObj);
                return new BusinessCategoryRegRespObj
                {
                    BusinessCategoryId = domainObj.BusinessCategoryId,
                    Status = new APIResponseStatus { IsSuccessful = isDone ? true : false, Message = new APIResponseMessage { FriendlyMessage = isDone ? "successful" : "Unsuccessful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                return new BusinessCategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.BusinessCategory.UPLOAD_BUSINESSCATEGORY)]
        public async Task<ActionResult<BusinessCategoryRegRespObj>> UploadBusinessCategoryAsync()
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
 
                var res = await _repo.UploadBusinessCategoryAsync(byteList);
                if(res != "uploaded")
                {
                    return new BusinessCategoryRegRespObj
                    {
                        Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = res } }
                    };
                }
                return new BusinessCategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = true , Message = new APIResponseMessage { FriendlyMessage = "successful" } }
                };
            }
            catch (Exception ex)
            {
                var errorCode = ErrorID.Generate(5);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new BusinessCategoryRegRespObj
                {
                    Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "Error Occurred", TechnicalMessage = ex?.Message, MessageId = errorCode } }
                };
            }
        }

        [HttpPost(ApiRoutes.BusinessCategory.DELETE_BUSINESSCATEGORY)]
        public async Task<IActionResult> DeleteBusinessCategory([FromBody] DeleteRequest item)
        {  
            try
            {
                foreach (var id in item.ItemIds)
                {
                    await _repo.DeleteBusinessCategoryAsync(id);
                }
                return Ok(
                        new DeleteRespObjt
                        {
                            Deleted = true,
                            Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Successful" } }
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

    }
}

