using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Deposit.Contracts.Response.Deposit.AccountOpening;
using System.Linq;

namespace Deposit.Handlers.corporateInformations
{
    public class Create_update_corporate_informationHandler : IRequestHandler<Create_update_corporate_information, AccountResponse>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext; 
        public Create_update_corporate_informationHandler(
            ILoggerService logger, 
            DataContext dataContext)
        { 
            _dataContext = dataContext;
            _logger = logger; 
        }

       
        public async Task<AccountResponse> Handle(Create_update_corporate_information request, CancellationToken cancellationToken)
        {
            var response = new AccountResponse();
            try
            {
                var customer_lite = _dataContext.deposit_customer_lite_information.Find(request.CustomerId);
                if (customer_lite == null)
                    customer_lite = new deposit_customer_lite_information();

                customer_lite.CustomerId = request.CustomerId;
                customer_lite.CustomerTypeId = request.CustomerTypeId;

                if (request.CustomerId == 0)
                    _dataContext.deposit_customer_lite_information.Add(customer_lite);
                await _dataContext.SaveChangesAsync();

                var domain = _dataContext.deposit_corporate_customer_information?.FirstOrDefault(e => e.CustomerId == customer_lite.CustomerId)?? null;
                if (domain == null) 
                    domain = new deposit_corporate_customer_information();
                 
                domain.CustomerId = customer_lite.CustomerId; 
                domain.CompanyName = request.CompanyName;
                domain.CertOfIncorporationNumber = request.CertOfIncorporationNumber;
                domain.DateOfIncorporation = request.DateOfIncorporation;
                domain.JurisdictionOfincorporatoin = request.JurisdictionOfincorporatoin;
                domain.NatureOfBusiness = request.NatureOfBusiness;
                domain.SectorOrIndustry = request.SectorOrIndustry;
                domain.OperatingAdress1 = request.OperatingAdress1;
                domain.OperatingAdress2 = request.OperatingAdress2;
                domain.RegisteredAddress = request.RegisteredAddress;
                domain.LGA = request.LGA;
                domain.State = request.State;
                domain.Email = request.Email;
                domain.Website = request.Website;
                domain.Phone = request.Phone;
                domain.MobileNumber = request.MobileNumber;
                domain.TaxIdentificationNumber = request.TaxIdentificationNumber;
                domain.SCUML = request.SCUML; 

                if (domain.CorporateCustomerId == 0) 
                    _dataContext.deposit_corporate_customer_information.Add(domain);
                await _dataContext.SaveChangesAsync();

                 
                response.CustomerId = domain.CustomerId;
                response.Status.IsSuccessful = true;
                response.Status.Message.FriendlyMessage = "successful";
                return response;
            }
            catch (Exception e)
            {
                response.Status.IsSuccessful = false;
                response.Status.Message.FriendlyMessage = e?.Message ?? e.InnerException?.Message;
                response.Status.Message.TechnicalMessage = e.ToString();
                _logger.Error(e.ToString());
                return response;
            }
        }
    }
}
