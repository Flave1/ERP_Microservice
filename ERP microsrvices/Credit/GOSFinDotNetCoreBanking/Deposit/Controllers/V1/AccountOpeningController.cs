using Deposit.Contracts.V1;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Deposit.AuthHandler.Interface;
using Microsoft.AspNetCore.Http;
using GOSLibraries.GOS_Error_logger.Service;
using Deposit.Requests;
using Deposit.Handlers.Auths;
using MediatR;

using Deposit.Handlers.Deposit.AccountSetup; 
using Deposit.Handlers.Deposit.DirectorsDetails; 
using Deposit.Data;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Microsoft.AspNetCore.Authorization;
using Deposit.Handlers.Operations.AccountOpeneing.Indiviadual_corporate.Queries;

namespace Deposit.Controllers.V1.Deposit
{
    [ERPAuthorize]
    public class AccountOpeningController : Controller
    {
       // private readonly IAccountOpeningService _repo;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _identityServer;
        private readonly IMediator _mediator;


        public AccountOpeningController(
            IMapper mapper, IIdentityService identityService, 
            IHttpContextAccessor httpContextAccessor, 
            ILoggerService logger, DataContext dataContext, 
            IIdentityServerRequest identityServer,
            IMediator mediator)
        {
           // _repo = repo;
            _mapper = mapper;
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _dataContext = dataContext;
            _identityServer = identityServer;
            _mediator = mediator;
        }





        #region INDIVIDUAL CUSTOMER
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Customer.ADD_UPDATE_INDIVIDUAL_CUSTOMER)]
        public async Task<IActionResult> ADD_UPDATE_INDIVIDUAL_CUSTOMER([FromBody] Create_update_individual_personal_information command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Customer.ADD_UPDATE_INDIVIDUAL_CONTACT_DETAIL)]
        public async Task<IActionResult> ADD_UPDATE_INDIVIDUAL_CONTACT_DETAIL([FromBody] Create_update_individual_contact_dteails command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Customer.ADD_UPDATE_INDIVIDUAL_EMPLOYMENT_DETAIL)]
        public async Task<IActionResult> ADD_UPDATE_INDIVIDUAL_EMPLOYMENT_DETAIL([FromBody] Create_emplyment_details command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Customer.ADD_UPDATE_INDIVIDUAL_IDENTIFICATION_DETAIL)]
        public async Task<IActionResult> ADD_UPDATE_INDIVIDUAL_IDENTIFICATION_DETAIL([FromBody] AddUpdateIdentificationCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Customer.ADD_UPDATE_INDIVIDUAL_NEXTOFKINS_DETAIL)]
        public async Task<IActionResult> ADD_UPDATE_INDIVIDUAL_NEXTOFKINS_DETAIL([FromBody] AddUpdateNextOfKinCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        #endregion


        #region INDIVIDUAL COPORATE

        [HttpGet(ApiRoutes.Customer.GET_INDIIDUAL_CORPORATE_LITE)]
        public async Task<IActionResult> GET_INDIIDUAL_CORPORATE_LITE()
        {
            var query = new CustomerLiteQuery();
            return Ok(await _mediator.Send(query));
        }



        [AllowAnonymous]
        [HttpPost(ApiRoutes.Customer.ADD_UPDATE_INDIIDUAL_CORPORATE_THUMPRINT)]
        public async Task<IActionResult> ADD_UPDATE_INDIIDUAL_CORPORATE_THUMPRINT([FromForm] Add_update_thump_printCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Customer.ADD_INDIVIDUAL_CORPORATE_SIGNATORY)]
        public async Task<IActionResult> ADD_INDIVIDUAL_CORPORATE_SIGNATORY([FromForm] AddUpdateSignatoryCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Customer.DELETE_INDIVIDUAL_CORPORATE_SIGNATORY)]
        public async Task<IActionResult> DELETE_INDIVIDUAL_CORPORATE_SIGNATORY([FromBody] DeleteSignatoryCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Customer.ADD_INDIVIDUAL_CORPORATE_KYC)]
        public async Task<IActionResult> ADD_INDIVIDUAL_CORPORATE_KYC([FromForm] AddUpdateKYCCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        #endregion

         



        #region CustomerDetails



        //[HttpGet(ApiRoutes.Customer.GET_CUSTOMER_BY_ID)]
        //public async Task<IActionResult> GET_CUSTOMER_BY_ID([FromQuery] GetSingleCustomerDetailsQuery query)
        //{ 
        //    return Ok(await _mediator.Send(query));
        //}


        //[HttpPost(ApiRoutes.Customer.ADD_UPDATE_CORPORATE_CUSTOMER)]
        //public async Task<IActionResult> ADD_UPDATE_CORPORATE_CUSTOMER([FromBody] Create_update_corporate_information command)
        //{
        //    var response = await _mediator.Send(command);
        //    if (response.Status.IsSuccessful)
        //        return Ok(response);
        //    return BadRequest(response);
        //}

        #endregion


        #region DIRECTORS
        //[HttpPost(ApiRoutes.Customer.ADD_DIRECTORS)]
        //public async Task<IActionResult> ADD_DIRECTORS([FromBody] AddUpdateDirectorsCommand command)
        //{
        //    var response = await _mediator.Send(command);
        //    if (response.Status.IsSuccessful)
        //        return Ok(response);
        //    return BadRequest(response);
        //}

        //[HttpPost(ApiRoutes.Customer.DELETE_DIRECTORS)]
        //public async Task<IActionResult> DELETE_DIRECTORS([FromBody] DeleteDirectorsCommand command)
        //{
        //    var response = await _mediator.Send(command);
        //    if (response.Status.IsSuccessful)
        //        return Ok(response);
        //    return BadRequest(response);
        //}

        //[HttpGet(ApiRoutes.Directors.GET_ALL_DIRECTORS)]
        //public async Task<IActionResult> GET_SIGNATORY([FromQuery] GetAllDirectorsDetailsQuery query)
        //{
        //    return Ok(await _mediator.Send(query));
        //}

        

        #endregion


        #region KEYCONTACTS
        [HttpPost(ApiRoutes.Customer.ADD_KEYCONTACTS)]
        public async Task<IActionResult> ADD_KEYCONTACTS([FromBody] AddUpdateKeyContactPersonCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        //[HttpPost(ApiRoutes.Customer.DELETE_KEYCONTACTS)]
        //public async Task<IActionResult> DELETE_KEYCONTACTS([FromBody] DeleteKeyContactPersonCommand command)
        //{
        //    var response = await _mediator.Send(command);
        //    if (response.Status.IsSuccessful)
        //        return Ok(response);
        //    return BadRequest(response);
        //}


        //[HttpGet(ApiRoutes.Customer.GET_KEY_CONTACT_PERSONS)]
        //public async Task<IActionResult> GET_KEY_CONTACT_PERSONS([FromQuery] GetAllContactPersonsQuery query)
        //{
        //    return Ok(await _mediator.Send(query));
        //}


        
        #endregion
 
        #region COMPLETDEDETAILS
        //[HttpGet(ApiRoutes.Customer.GET_SINGLE_COMPLETEDETAILS)]
        //public async Task<IActionResult> GET_SINGLE_COMPLETEDETAILS([FromQuery] GetSingleCustomerDetailsQuery query)
        //{
        //    var response = await _mediator.Send(query);
        //    if (response.Status.IsSuccessful)
        //        return Ok(response);
        //    return BadRequest(response);
        //}
        #endregion

         
        #region ACCOUNT INFORM
        //[HttpPost(ApiRoutes.Customer.ADD_ACCOUNT_DETAIL)]
        //public async Task<IActionResult> ADD_ACCOUNT_DETAIL([FromBody] Create_update_account_informationCommand command)
        //{
        //    var response = await _mediator.Send(command);
        //    if (response.Status.IsSuccessful)
        //        return Ok(response);
        //    return BadRequest(response);
        //}
          
        #endregion
    }
}
