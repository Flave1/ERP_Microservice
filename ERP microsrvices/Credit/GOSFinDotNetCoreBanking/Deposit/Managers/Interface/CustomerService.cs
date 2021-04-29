using Deposit.DomainObjects.Deposit;
using GODP.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Deposit.Repository.Interface.Deposit
{
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
        List<long> Return_this_account_operating_currencies(string Account_number);
        Task<bool> Perform_transaction_teller_and_balancing(deposit_cashierteller_form request);

        Task<bool> Check_against_opening_balance_async(long currency, decimal amount_request);

    }
}
