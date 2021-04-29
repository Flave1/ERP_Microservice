using Deposit.Contracts.GeneralExtension;
using Deposit.Contracts.Response.Deposit.Deposit_form;
using Deposit.DomainObjects.Deposit;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.Deposit_form
{
    public class Deposit_to_customerCommandHandler : IRequestHandler<Deposit_to_customer, Account_response>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        private readonly ICustomerService _customer;
        private readonly IIdentityServerRequest _serverRequest;
        
        public Deposit_to_customerCommandHandler(ILoggerService logger, IIdentityServerRequest serverRequest, DataContext dataContext, ICustomerService customer)
        {
            _dataContext = dataContext;
            _customer = customer;
            _logger = logger;
            _serverRequest = serverRequest;
        }

        private Account_response Validate_transaction(deposit_form request)
        {
            var validation_response = new Account_response();
            var customer_account_details = _dataContext.deposit_customer_account_information.FirstOrDefault(e => e.AccountNumber.ToLower() == request.Account_number.ToLower());

            if (customer_account_details == null)
            {
                validation_response.Status.Message.FriendlyMessage = "Invalid customer account number";
                return validation_response;
            }

            if( DateTime.UtcNow.Date > customer_account_details.Date_to_go_dormant)
            {
                validation_response.Status.Message.FriendlyMessage = "Unable to process transaction on a dormant account";
                return validation_response;
            }

            var this_account_operating_currencies = customer_account_details.Currencies.Split(',').ToList().Select(int.Parse).ToList();
            if (!this_account_operating_currencies.Any(r => r == request.Currency))
            {
                validation_response.Status.Message.FriendlyMessage = "this account does not operate on selected currency";
                return validation_response;
            }
            validation_response.Status.IsSuccessful = true;
            return validation_response;

        }

        public deposit_form Build_DB_object(Deposit_to_customer request, deposit_form db_item)
        {
            var user = _serverRequest.UserDataAsync().Result;

            db_item.CompanyId = user.CompanyId;
            db_item.Id = request.Id;
            db_item.Transaction_mode = request.Transaction_mode;
            db_item.Remark = request.Remark;
            db_item.Account_number = request.Account_number;
            db_item.CustomerId = request.CustomerId;
            db_item.Deposit_amount = request.Deposit_amount;
            db_item.Instrument_date = request.Instrument_date;
            db_item.Structure = user.CompanyId;
            db_item.Instrument_number = request.Instrument_number;
            db_item.TransactionId = Transaction_ID.Generate();
            db_item.Value_date = DateTime.UtcNow;
            db_item.Currency = request.Currency; 
            return db_item; 
        }

        public void Update_customer_from_deposit(deposit_form deposit)
        { 
            var customer = _dataContext.deposit_customer_account_information.FirstOrDefault(e => e.AccountNumber.ToLower() == deposit.Account_number.ToLower());
            if(customer != null)
            {
                var days = _customer.Return_dormancy_days(customer.CustomerId);
                customer.Date_to_go_dormant = DateTime.UtcNow.AddDays(days);
                customer.AvailableBalance = customer.AvailableBalance + deposit.Deposit_amount; 
            }
        }

        public async Task<Account_response> Handle(Deposit_to_customer request, CancellationToken cancellationToken)
        {
            var response = new Account_response();
            try
            { 
                var item = _dataContext.deposit_form.Find(request.Id); 
                if (item == null) item = new deposit_form(); 
                item = Build_DB_object(request, item);

                var validation_reponse = Validate_transaction(item);
                if (!validation_reponse.Status.IsSuccessful)
                    return validation_reponse;

                if (item.Id < 1) _dataContext.deposit_form.Add(item);
                using(var transaction = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        Update_customer_from_deposit(item);
                        await _customer.Remove_from_staff_opening_balance(item.Deposit_amount, item.Currency);
                        await _dataContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        response.Status.IsSuccessful = true;
                        response.Status.Message.FriendlyMessage = "successful";
                        return response;
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        response.Status.Message.FriendlyMessage = e?.Message ?? e?.InnerException?.Message;
                        response.Status.Message.TechnicalMessage = e.ToString();
                        return response;
                    }
                    finally { await transaction.DisposeAsync(); }
                } 
            }
            catch (Exception e)
            {
                response.Status.Message.FriendlyMessage = e?.Message ?? e.InnerException?.Message;
                response.Status.Message.TechnicalMessage = e.ToString();
                _logger.Error(e.ToString());
                return response;
            }
        }
    }
}
