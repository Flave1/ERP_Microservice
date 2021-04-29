
using Deposit.Contracts.Response.Deposit.AccountOpening;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.KYCs
{
    public class AddUpdateKYCCommandHandler : IRequestHandler<AddUpdateKYCCommand, AccountOpeningRegRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        public AddUpdateKYCCommandHandler(ILoggerService logger, DataContext dataContext, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<AccountOpeningRegRespObj> Handle(AddUpdateKYCCommand request, CancellationToken cancellationToken)
        {
            var response = new AccountOpeningRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var domain = _dataContext.deposit_kyc.Find(request.kycId);
                if (domain == null)
                    domain = new deposit_kyc();

                domain.CustomerId = request.CustomerId;
                domain.kycId = request.kycId;
                domain.AddressVisited = request.AddressVisited;
                domain.CommentOnLocation = request.CommentOnLocation;
                domain.Confirmaiotnname = request.Confirmaiotnname;
                domain.ConfirmationDate = request.ConfirmationDate;
                domain.Confirmed = request.Confirmed;
                domain.DateOfVisitation = request.DateOfVisitation;
                domain.DoesTheCustomerEnjoyTieredKYC = request.DoesTheCustomerEnjoyTieredKYC;
                domain.DulyCompletedAccountOpenningForm = request.DulyCompletedAccountOpenningForm;
                domain.FullNameOfVisitingStaff = request.FullNameOfVisitingStaff;
                domain.IsCustomerPoliticalyExposed = request.IsCustomerPoliticalyExposed;
                domain.isUtilityBillSubmitted = request.isUtilityBillSubmitted;
                domain.Location_ColorOfbuilding = request.Location_ColorOfbuilding;
                domain.Location_DescriptionOfBuilding = request.Location_DescriptionOfBuilding;
                domain.OtherDocumentsObtained = request.OtherDocumentsObtained;
                domain.PoliticalyExposedDetails = request.PoliticalyExposedDetails;
                domain.RecentPassportPhotograph = request.RecentPassportPhotograph;
                domain.RiskCategory = request.RiskCategory;
                domain.SociallyOrFinanciallyDisadvantaged = request.Financiallydisadvantaged;
                domain.DeferralFullName = request.DeferralFullName;
                domain.DeferralDate = request.ConfirmationDate;

                if (domain.kycId > 0)
                    _dataContext.Entry(domain).CurrentValues.SetValues(domain);
                else
                    _dataContext.deposit_kyc.Add(domain);
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
