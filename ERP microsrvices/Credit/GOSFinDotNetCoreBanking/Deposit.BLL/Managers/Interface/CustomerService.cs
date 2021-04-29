using Deposit.DomainObjects.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
    public class CustomerService : ICustomerService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityServerRequest _serverRequest;
        public CustomerService(DataContext dataContext, IIdentityServerRequest serverRequest)
        {
            _serverRequest = serverRequest;
            _dataContext = dataContext;
        }

        public void Reactivate_customer_account(deposit_reactivation_form request)
        {
            var customer = _dataContext.deposit_customer_accountdetails.FirstOrDefault(e => e.CustomerId == request.CustomerId);
            if (customer != null)
            {
                customer.Deleted = false;
                var next_dormant_day = Return_dormancy_days(customer.CustomerId); 
                customer.Date_to_go_dormant = DateTime.UtcNow.AddDays(next_dormant_day);
            }
        }

        public void Reactivate_customer_account(deposit_reactivation_form request, int[] currencies)
        {
            var customer = _dataContext.deposit_customer_accountdetails.FirstOrDefault(e => e.CustomerId == request.CustomerId);
            if (customer != null)
            {
                customer.Deleted = false;
                customer.Currencies = string.Join(",", currencies);
                customer.AvailableBalance = customer.AvailableBalance - request.Charges;
                customer.Date_to_go_dormant = DateTime.UtcNow.AddDays(Return_dormancy_days(customer.CustomerId));
            }
        }
        public string Return_customer_name(long cust_id)
        {
            var name = string.Empty;
            var customer = _dataContext.deposit_accountopening.Find(cust_id);
            if (customer == null) return name;
            if (customer.CustomerTypeId == (int)CustomerType.Corporate)
                name = customer.CompanyName;
            else
                name = $"{customer.Firstname} {customer.Firstname}";
            return name;
        }

        public int Return_dormancy_days(long cust_id)
        {
            var customer_type_id = _dataContext.deposit_customer_accountdetails.FirstOrDefault(e => e.CustomerId == cust_id)?.AccountTypeId ??0;
            return _dataContext.deposit_accountsetup.FirstOrDefault(e => e.AccountTypeId == customer_type_id)?.DormancyDays ?? 0;
        }

        public decimal Return_reactivation_charges_if_applicable(int prod)
        { 
            var reactivation_setup = _dataContext.deposit_accountreactivationsetup.FirstOrDefault(e => e.Product == prod);
            if (reactivation_setup != null)
            {
                if (reactivation_setup.ChargeType.ToLower() == "fixed")
                    return decimal.Parse(reactivation_setup.Charge);
                else
                    return decimal.Parse(reactivation_setup.Charge) / 100 * decimal.Parse(reactivation_setup.Charge);
            }
            return new decimal();
        }

        public decimal Return_withdrawal_charges_if_applicable(int prod)
        {
            var withdrawal_setup = _dataContext.deposit_withdrawalsetup.FirstOrDefault(e => e.Product == prod);
            if (withdrawal_setup != null)
            {
                if ((bool)withdrawal_setup.WithdrawalCharges)
                {
                    if (withdrawal_setup.ChargeType != null && withdrawal_setup.ChargeType.ToLower() == "fixed")
                        return decimal.Parse(withdrawal_setup.Charge);
                    else
                        return decimal.Parse(withdrawal_setup.Charge) / 100 * decimal.Parse(withdrawal_setup.Charge);
                }
                return new decimal();
            }
            return new decimal();
        }

        public decimal Return_bank_closure_charges_if_applicable(decimal amt, int acount_type)
        {
            var this_account_type = _dataContext.deposit_accountsetup.FirstOrDefault(e => e.AccountTypeId == acount_type);
            if(this_account_type != null)
            {
                var bank_closure_setup = _dataContext.deposit_bankclosuresetup.FirstOrDefault(r => r.ProductId == this_account_type.DepositAccountId && r.Deleted == false);
                if (bank_closure_setup != null)
                {
                    if (bank_closure_setup.Charge.ToLower() == "fixed")
                        return Convert.ToDecimal(bank_closure_setup.Percentage);
                    else
                        return amt / 100 * Convert.ToDecimal(bank_closure_setup.Percentage);
                } 
            } 
            return new decimal();
        }
        public List<int> Return_this_account_operating_currencies(string CURRENCIES)
        {
            try { return CURRENCIES.Split(',').ToList().Select(int.Parse).ToList(); }
            catch (Exception e) { throw e; } 
        }

        public async Task<bool> Perform_transaction_teller_and_balancing(deposit_cashierteller_form request)
        {
            var this_validation_transaction_ids = request.Transaction_IDs.Split(",").ToList();
            if(this_validation_transaction_ids.Count() > 0)
            {
                foreach(var tran_id in this_validation_transaction_ids)
                {
                    var withdrawal_transaction = _dataContext.deposit_withdrawal_form.FirstOrDefault(r => r.Deleted == false && r.Transaction_Id == tran_id);
                    if(withdrawal_transaction != null) 
                        withdrawal_transaction.Is_call_over_done = true;

                    var deposit_transaction = _dataContext.deposit_form.FirstOrDefault(r => r.Deleted == false && r.TransactionId == tran_id);
                    if (deposit_transaction != null)
                        deposit_transaction.Is_call_over_done = true;
                }
            }
            return await Task.Run(() => true);
        }
        public async Task<bool> Check_against_opening_balance_async(long currency, decimal amount_request)
        {
            

            var user_detail = await _serverRequest.UserDataAsync();
            var this_staff_opening_balance = await _dataContext.deposit_call_over_currecies_and_amount.
                FirstOrDefaultAsync(e => Convert.ToInt32(e.User_id) == user_detail.StaffId && e.Currency == currency);
            
            if (this_staff_opening_balance == null) return false;

            if (this_staff_opening_balance.Amount < amount_request) return false;

            return true;
        }

        public async Task Remove_from_staff_opening_balance(decimal amount_to_remove, long currency)
        { 
            var user_detail = await _serverRequest.UserDataAsync();
            var this_staff_opening_balance = await _dataContext.deposit_call_over_currecies_and_amount.
                FirstOrDefaultAsync(e => Convert.ToInt32(e.User_id) == user_detail.StaffId && e.Currency == currency);

            if (this_staff_opening_balance != null)
                this_staff_opening_balance.Amount = (this_staff_opening_balance.Amount - amount_to_remove); 
        } 
    }

    public interface ICustomerService
    {
        Task Remove_from_staff_opening_balance(decimal amount_to_remove, long currency);
        decimal Return_reactivation_charges_if_applicable(int prod);
        decimal Return_withdrawal_charges_if_applicable(int prod);
        decimal Return_bank_closure_charges_if_applicable(decimal amt, int acount_type);
        void Reactivate_customer_account(deposit_reactivation_form request, int[] currencies);
        void Reactivate_customer_account(deposit_reactivation_form request);
        string Return_customer_name(long cust_id);
        int Return_dormancy_days(long cust_id);
        List<int> Return_this_account_operating_currencies(string Account_number);
        Task<bool> Perform_transaction_teller_and_balancing(deposit_cashierteller_form request);

        Task<bool> Check_against_opening_balance_async(long currency, decimal amount_request);

    }
}
