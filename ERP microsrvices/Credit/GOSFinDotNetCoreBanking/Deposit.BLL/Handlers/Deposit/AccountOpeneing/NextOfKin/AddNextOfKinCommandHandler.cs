using Deposit.Contracts.Response.Deposit.AccountOpening;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.NextOfKins
{
    public class AddUpdateNextOfKinCommandHandler : IRequestHandler<AddUpdateNextOfKinCommand, AccountOpeningRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        public AddUpdateNextOfKinCommandHandler(ILoggerService logger, DataContext dataContext, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<AccountOpeningRegRespObj> Handle(AddUpdateNextOfKinCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountOpeningRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var domain = _dataContext.deposit_nextofkin.Find(request.NextOfKinId);
                if (domain == null)
                    domain = new deposit_nextofkin();



                //Next of kin
                domain.CustomerId = request.CustomerId;
                domain.NextOfKinId = request.NextOfKinId;
                domain.NextOfKinTitle = request.NextOfKinTitle;
                domain.NextOfKinSurname = request.NextOfKinSurname;
                domain.NextOfKinFirstName = request.NextOfKinFirstName;
                domain.NextOfKinOtherNames = request.NextOfKinOtherNames;
                domain.NextOfKinDateOfBirth = request.NextOfKinDateOfBirth;
                domain.NextOfKinGender = request.NextOfKinGender;
                domain.NextOfKinRelationship = request.NextOfKinRelationship;
                domain.NextOfKinMobileNumber = request.NextOfKinMobileNumber;
                domain.NextOfKinEmailAddress = request.NextOfKinEmailAddress;
                domain.NextOfKinAddress = request.NextOfKinAddress;
                domain.NextOfKinCity = request.NextOfKinCity;
                domain.NextOfKinState = request.NextOfKinState;

 
                if (domain.NextOfKinId > 0)
                    _dataContext.Entry(domain).CurrentValues.SetValues(domain);
                else
                    _dataContext.deposit_nextofkin.Add(domain);
                await _dataContext.SaveChangesAsync();


                response.CustomerId = domain.CustomerId;
                response.Status.Message.FriendlyMessage = "successful";
                return response;
            }
            catch (Exception e)
            {
                response.Status.IsSuccessful = false;
                response.Status.Message.FriendlyMessage = e?.Message ?? e.InnerException?.Message;
                response.Status.Message.TechnicalMessage = e.ToString();
                return response;
            }
        }
    }
}
