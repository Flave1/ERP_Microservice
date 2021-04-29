using System;
using System.Collections.Generic;

namespace Deposit.Contracts.V1
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version + "/deposit";


        public static class AcccountReactivationEndpoints
        {
            public const string GET_REACTIVATE_ACCOUNT_SETUP = Base + "/accountreactivation/get/single/setup";
            public const string GET_ALL_REACTIVATE_ACCOUNT_SETUP = Base + "/accountreactivation/get/all/setup";
            public const string ADD_REACTIVATE_ACCOUNT_SETUP = Base + "/accountreactivation/addupdate/setup";
            public const string DELETE_REACTIVATE_ACCOUNT_SETUP = Base + "/accountreactivation/delete/setup";
            public const string UPLOAD_REACTIVATE_ACCOUNT_SETUP = Base + "/accountreactivation/upload/setup";
            public const string DOWNLOAD_REACTIVATE_ACCOUNT_SETUP = Base + "/accountreactivation/download/setup";
        }

        public static class ReportEndpoints
        {
            public const string LOAN_OFFER_LETTER = Base + "/loanapplication/generate/offerletter";
            public const string LOAN_OFFER_LETTER_LMS = Base + "/loanapplication/generate/offerletter/lms";
            public const string LOAN_OFFER_LETTER_INVESTMENT = Base + "/loanapplication/generate/offerletter/investment";
            public const string LOAN_OFFER_LETTER_SCHEDULE = Base + "/loanapplication/generate/offerletter/schedule";
            public const string LOAN_OFFER_LETTER_FEE = Base + "/loanapplication/generate/offerletter/fee";
            public const string LOAN_INDIVIDUAL_CUSTOMER_REPORT = Base + "/loanapplication/loan/individual/customer/report";
            public const string LOAN_CORPORATE_CUSTOMER_REPORT = Base + "/loanapplication/loan/corporate/customer/report";
            public const string LOAN_REPORT = Base + "/loanapplication/loan/report";
            public const string INVESTMENT_INDIVIDUAL_CUSTOMER_REPORT = Base + "/loanapplication/investment/individual/customer/report";
            public const string INVESTMENT_CORPORATE_CUSTOMER_REPORT = Base + "/loanapplication/investment/corporate/customer/report";
            public const string INVESTMENT_REPORT = Base + "/loanapplication/investment/report";
        }
        public static class Identity
        {
            public const string CUSTOMER_LOGIN = Base + "/customer/identity/login";
            public const string CUSTOMER_OTP_LOGIN = Base + "/customer/otp/identity/login";
            public const string ANSWER = Base + "/customer/an_swer/question";
            public const string GET_ANSWER = Base + "/customer/get/answer/question";
            public const string RECOVER_PASSWORD_BY_EMAIL = Base + "/customer/identity/recoverpassword/byemail";
            public const string NEW_PASS = Base + "/customer/identity/newpassword";
            public const string LOGIN = "/identity/login";
            public const string SECURITY = "/admin/otherservice/auth/guard/get/all";
            public const string FAILED_LOGIN = "/identity/failed/login";
            public const string Session_LOGIN = "/identity/session/login";
            public const string CUSTOMER_REGISTER = Base + "/customer/identity/register";
            public const string CUSTOMER_REFRESHTOKEN = Base + "/customer/identity/refresh";
            public const string CUSTOMER_CHANGE_PASSWORD = Base + "/customer/identity/changePassword";
            public const string CUSTOMER_CONFIRM_EMAIL = Base + "/customer/identity/confirmEmail";
            public const string CUSTOMER_VERIFY_EMAIL = Base + "/customer/identity/verifyEmail";
            public const string FETCH_USERDETAILS =  "/identity/profile";
            public const string FETCH_CUSTOMER_USERDETAILS = Base + "/customer/identity/profile";
            public const string CUSTOMER_CONFIRM_CODE = Base + "/customer/identity/confirmCode";
            public const string GET_PERMIT_DEATAILS = "/identity/permitDetails";
            public const string SEND_MAIL = "/email/send/emails";
            public const string GET_FLUTTERWAVE_KEYS = "/payment/flutterwave/get/keys";
            public const string TRACKED = "/identity/get/trackedlogin";
        }


        public static class IdentitySeverWorkflow
        {
            //CALLED THROUGH CODE
            public const string GO_FOR_APPROVAL = "/workflow/goThroughApprovalFromCode";
            public const string GET_ALL_STAFF_AWAITING_APPROVALS = "/workflow/get/all/staffAwaitingApprovalsFromCode";
            public const string STAFF_APPROVAL_REQUEST = "/workflow/staff/approvaltask";
            public const string GET_ALL_STAFF = "/admin/get/all/staff";
            public const string GET_QUESTION = "/admin/get/all/questions";
            public const string ACTIVITES = "/admin/get/all/activities";
            public const string GET_THIS_ROLES = "/admin/get/this/userroles";
        }

        public static class Finance
        {
            public const string PASS_TO_ENTRY = "/financialtransaction/pass/to/entry";
            public const string GET_ALL_SUBGL = "/subgl/get/all";
        }

        public static class ApprovalDetail
        {
            public const string CREDIT_DEPOSIT_APPROVAL_DETAIL = Base + "/bankclosure/credit/approval/details";
        }
        public static class Currency
        {
            public const string GET_ALL_CURRENCY = "/common/currencies";
            public const string GET_ALL_GENDER = "/common/genders";
            public const string GET_ALL_MARITAL_STATUS = "/common/maritalStatus";
            public const string GET_ALL_EMPLOYMENT_TYPE = "/common/employerTypes";
            public const string GET_ALL_IDENTITICATION_TYPE = "/common/Identifications";
            public const string GET_CURRENCY_BY_ID = "/common/get/single/currencyById";
            public const string GET_ALL_JOBTITLES = "/common/jobTitles";
        }
        public static class CompanyStructure
        {
            public const string GET_ALL_COMPANY_STRUCTURE = "/company/get/all/companystructures";
            public const string GET_COMPANY_STRUCTURE_BY_ID = "/company/get/single/companystructure/id";
        }
        public static class Workflow
        {
            public const string ADD_WORKFLOW = "/workflow/add/update/workflow";
            public const string GET_WORKFLOW_BY_ID = "/workflow/get/all/workflow/workflowId";
            public const string GET_ALL_OPERATION_TYPES = "/workflow/get/all/operationTypes";
            public const string GET_ALL_OPERATIONS = "/workflow/get/all/operations";
            public const string STAFF_APPROVAL_REQUEST = "/workflow/staff/approvaltask";
            public const string STAFF_CAN_EDIT = "/workflow/staff/canedit";
        }
         
        #region Deposit
        public class AccountType
        {
            public const string ADD_UPDATE_ACCOUNT_TYPE = Base + "/acounttype/add/update/accountType";
            public const string GET_ALL_ACCOUNT_TYPE = Base + "/acounttype/get/all/accountType";
            public const string GET_ACCOUNT_TYPE_BY_ID = Base + "/acounttype/get/accountTypeById";
            public const string DELETE_ACCOUNT_TYPE = Base + "/accountType/delete/accountType";
            public const string DOWNLOAD_ACCOUNT_TYPE = Base + "/accountType/download/accountType";
            public const string UPLOAD_ACCOUNT_TYPE = Base + "/accountType/upload/accountType";
        }

        public class AccountSetup
        {
            public const string ADD_UPDATE_ACCOUNTSETUP = Base + "/accountsetup/add/update/accountsetup";
            public const string GET_ALL_ACCOUNTSETUP = Base + "/accountsetup/get/all/accountsetup";
            public const string GET_ACCOUNTSETUP_BY_ID = Base + "/accountsetup/get/accountsetupbyid";
            public const string DELETE_ACCOUNTSETUP = Base + "/accountsetup/delete/accountsetup";
            public const string DOWNLOAD_ACCOUNTSETUP = Base + "/accountsetup/download/accountsetup";
            public const string UPLOAD_ACCOUNTSETUP = Base + "/accountsetup/upload/accountsetup";

            public const string ADD_UPDATE_DEPOSITFORM = Base + "/accountsetup/add/update/depositform";
            public const string GET_ALL_DEPOSITFORM = Base + "/accountsetup/get/all/depositform";
            public const string GET_DEPOSITFORM_BY_ID = Base + "/accountsetup/get/depositformbyid";
            public const string DELETE_DEPOSITFORM = Base + "/accountsetup/delete/depositform";
        }

        public class BankClosure
        {
            public const string ADD_UPDATE_BANK_CLOSURE = Base + "/bankclosure/add/update/bankClosure";
            public const string GET_ALL_BANK_CLOSURE = Base + "/bankclosure/get/all/bankClosures";
            public const string ADD_UPDATE_BANK_CLOSURE_SETUP = Base + "/bankclosure/add/update/bankClosureSetup";
            public const string STAFF_BANK_CLOSURE_APPROVAL = Base + "/bankclosure/staff/approval";
            public const string DELETE_BANK_CLOSURE = Base + "/bankclosure/delete/bankClosure";
            public const string DELETE_BANK_CLOSURE_SETUP = Base + "/bankclosure/delete/bankClosureSetup";
            public const string GET_BANK_CLOSURE_AWAITING_APPRO = Base + "/bankclosure/get/all/awaiting/approvals";
            public const string GET_BANK_CLOSURE_SETUP = Base + "/bankclosure/get/single/bankClosureSetup";
            public const string GET_ALL_BANK_CLOSURE_SETUP = Base + "/bankclosure/get/all/bankClosureSetup";
            public const string DOWNLOAD_BANK_CLOSURE_SETUP = Base + "/bankclosure/download/bankClosureSetup";
            public const string UPLOAD_BANK_CLOSURE_SETUP = Base + "/bankclosure/upload/bankClosureSetup";
        }

        public class BusinessCategory
        {
            public const string ADD_UPDATE_BUSINESSCATEGORY = Base + "/businesscategory/add/update";
            public const string GET_ALL_BUSINESSCATEGORY = Base + "/businesscategory/get/all";
            public const string GET_BUSINESSCATEGORY_BY_ID = Base + "/businesscategory/get/businesscategoryId";
            public const string DELETE_BUSINESSCATEGORY = Base + "/businesscategory/delete";
            public const string DOWNLOAD_BUSINESSCATEGORY = Base + "/businesscategory/download";
            public const string UPLOAD_BUSINESSCATEGORY = Base + "/businesscategory/upload";
        }

        public class CashierTeller
        {
            public const string ADD_UPDATE_CASHIERTELLERSETUP = Base + "/cashierteller/add/update/cashiertellersetup";
            public const string GET_ALL_CASHIERTELLERSETUP = Base + "/cashierteller/get/all/cashiertellersetup";
            public const string GET_CASHIERTELLERSETUP_BY_ID = Base + "/cashierteller/get/cashiertellersetupid";
            public const string DELETE_CASHIERTELLERSETUP = Base + "/cashierteller/delete/cashiertellersetup";
            public const string DOWNLOAD_CASHIERTELLERSETUP = Base + "/cashierteller/download/cashiertellersetup";
            public const string UPLOAD_CASHIERTELLERSETUP = Base + "/cashierteller/upload/cashiertellersetup";

            public const string ADD_UPDATE_CASHIERTELLERFORM = Base + "/cashierteller/add/update/cashiertellerform";
            public const string GET_ALL_CASHIERTELLERFORM = Base + "/cashierteller/get/all/cashiertellerform";
            public const string GET_CASHIERTELLERFORM_BY_ID = Base + "/cashierteller/get/cashiertellerformid";
            public const string DELETE_CASHIERTELLERFORM = Base + "/cashierteller/delete/cashiertellerform";
        }

        public class ChangeOfRates
        {
            public const string ADD_UPDATE_CHANGEOFRATES_SETUP = Base + "/changeofrates/add/update/changeofratessetup";
            public const string GET_ALL_CHANGEOFRATES_SETUP = Base + "/changeofrates/get/all/changeofratessetup";
            public const string GET_CHANGEOFRATES_SETUP_BY_ID = Base + "/changeofrates/get/changeofratessetupid";
            public const string DELETE_CHANGEOFRATES_SETUP = Base + "/changeofrates/delete/changeofratessetup";
            public const string DOWNLOAD_CHANGEOFRATES_SETUP = Base + "/changeofrates/download/changeofratessetup";
            public const string UPLOAD_CHANGEOFRATES_SETUP = Base + "/changeofrates/upload/changeofratessetup";

            public const string ADD_UPDATE_CHANGEOFRATES = Base + "/changeofrates/add/update/changeofrates";
            public const string GET_ALL_CHANGEOFRATES = Base + "/changeofrates/get/all/changeofrates";
            public const string GET_CHANGEOFRATES_BY_ID = Base + "/changeofrates/get/changeofratesid";
            public const string DELETE_CHANGEOFRATES = Base + "/changeofrates/delete/changeofrates";
            public const string AWAITING_CHANGEOFRATES = Base + "/changeofrates/get/awaitig/approval";
            public const string STAFF_CHANGEOFRATES = Base + "/changeofrates/staff/approval";
        }

        public class ContactPersons
        {
            public const string ADD_UPDATE_CONTACTPERSONS = Base + "/accountopening/add/update/contactpersons";
            public const string GET_ALL_CONTACTPERSONS = Base + "/accountopening/get/all/contactpersons";
            public const string GET_CONTACTPERSONS_BY_ID = Base + "/accountopening/get/contactpersons";
            public const string DELETE_CONTACTPERSONS = Base + "/accountopening/delete/contactpersons";
        }

        public class Customer_account_operation
        {
            public const string DEPOSIT_TO_CUSTOMER = Base + "/customer_account_operation/deposit";
            public const string GET_ALL_DEPOSITS = Base + "/customer_account_operation/get/all/deposits";

            public const string WITHDRAW_FROM_CUSTOMER = Base + "/customer_account_operation/withdrawal";
            public const string GET_ALL_WITHDRAWALS = Base + "/customer_account_operation/get/all/withdrawals";

            public const string REACTIVATE_ACCOUNT = Base + "/customer_account_operation/reactivate/account";
            public const string GET_ALL_REACTIVATIONS = Base + "/customer_account_operation/get/all/reativations";
            public const string GET_ALL_AWAITING_REACTIVATIONS = Base + "/customer_account_operation/get/reactivations/awaiting/approval";
            public const string REACTIVATION_STAFF_APPROVAL = Base + "/customer_account_operation/reativation/satff/approval";

            public const string GET_STAFF_CALL_OVERS = Base + "/customer_account_operation/get/satff/call_over_transactions";

            public const string CALL_OVER_CURRENCY_AMOUNT = Base + "/customer_account_operation/add/ob/currency/amount";

            public const string VALIDATE_TRANSACTIONS = Base + "/customer_account_operation/validate/transaction";
            public const string GET_STAFF_VALIDATE_TRANSACTIONS_FOR_APPROVAL = Base + "/customer_account_operation/get/staff/validateion/transactions/awaitingapproval";
            public const string ADD_VALIDATE_TRANSACTIONS = Base + "/customer_account_operation/add/validate/transaction/approval";
            public const string GET_TRANSACTIONS_BY_CURRENCIES_STRUCTURE = Base + "/customer_account_operation/get/transactions/bycurrency/andstructure";
        }

        public class Customer
        {
            public const string ADD_UPDATE_INDIVIDUAL_CUSTOMER = Base + "/accountopening/add/update/individual_information/customer";
            public const string ADD_UPDATE_INDIVIDUAL_CONTACT_DETAIL = Base + "/accountopening/add/update/individual_contact_details/customer";
            public const string ADD_UPDATE_INDIVIDUAL_EMPLOYMENT_DETAIL = Base + "/accountopening/add/update/individual_employment_details/customer";
            public const string ADD_UPDATE_INDIVIDUAL_IDENTIFICATION_DETAIL = Base + "/accountopening/add/update/individual_identifcation_details/customer";
            public const string ADD_UPDATE_INDIVIDUAL_NEXTOFKINS_DETAIL = Base + "/accountopening/add/update/individual_nextofkins_details/customer";

            public const string GET_INDIIDUAL_CORPORATE_LITE = Base + "/accountopening/get/individual_corporate/customer_lite";
            public const string ADD_UPDATE_INDIIDUAL_CORPORATE_THUMPRINT = Base + "/accountopening/add/update/individual_corporate/thumpprints";
            public const string ADD_INDIVIDUAL_CORPORATE_SIGNATORY = Base + "/accountopening/add/update/individual_corporate/signatory";
            public const string ADD_INDIVIDUAL_CORPORATE_KYC = Base + "/accountopening/add/update/individual_corporate/kyc";
            public const string DELETE_INDIVIDUAL_CORPORATE_SIGNATORY = Base + "/accountopening/delete/individual_corporate/signatory";






            public const string DELETE_KEYCONTACTS = Base + "/accountopening/delete/keycontacts"; 
            public const string ADD_UPDATE_CORPORATE_CUSTOMER = Base + "/accountopening/add/update/corporate/customer";
            public const string GET_ALL_CUSTOMER = Base + "/accountopening/get/all/customerlite";
            public const string GET_ALL_CUSTOMER_LITE_OTHER_INFO = Base + "/accountopening/get/all/customerlite/otherinfo";
            public const string GET_ALL_CUSTOMER_CASA = Base + "/accountopening/get/casa/list";
            public const string GET_CUSTOMER_BY_ID = Base + "/accountopening/get/customerdetailsbyid";
            public const string DELETE_CUSTOMER = Base + "/accountopening/delete/customer";
            public const string ADD_DEPOSITCUSTOMER = Base + "/accountopening/add/deposit/customer";
            public const string UPDATE_CASA = Base + "/accountopening/update/casa";
            public const string ADD_IDENTIFICATION = Base + "/accountopening/add/customeridentitfication";
            public const string DELETE_IDENTIFICATION = Base + "/accountopening/delete/customeridentitfication";
            public const string GET_ALL_IDENTIFICATION = Base + "/accountopening/get/all/customeridentitfications";
            public const string ADD_DIRECTORS = Base + "/accountopening/add/directors";
            public const string DELETE_DIRECTORS = Base + "/accountopening/delete/directors";
            public const string ADD_KEYCONTACTS = Base + "/accountopening/add/keycontacts";
            public const string DELETE_KYC = Base + "/accountopening/delete/kyc";

            


            public const string GET_SINGLE_COMPLETEDETAILS = Base + "/accountopening/get/sing/comopletedetails";

            public const string ADD_NEXT_OF_KIN = Base + "/accountopening/add/nextofkin";
            public const string DELETE_NEXT_OF_KIN = Base + "/accountopening/delete/nextofkin";

            public const string ADD_ACCOUNT_DETAIL = Base + "/accountopening/add/account/detail";
            public const string GET_ACCOUNT_DETAIL = Base + "/accountopening/account/detail";

            public const string GET_KEY_CONTACT_PERSONS = Base + "/accountopening/get/keycontactpersonse";
        }

        public class DepositCategory
        {
            public const string ADD_UPDATE_DEPOSITCATEGORY = Base + "/depositcategory/add/update/depositcategory";
            public const string GET_ALL_DEPOSITCATEGORY = Base + "/depositcategory/get/all/depositcategory";
            public const string GET_DEPOSITCATEGORY_BY_ID = Base + "/depositcategory/get/depositcategoryid";
            public const string DELETE_DEPOSITCATEGORY = Base + "/depositcategory/delete/depositcategory";
            public const string DOWNLOAD_DEPOSITCATEGORY = Base + "/depositcategory/download/depositcategory";
            public const string UPLOAD_DEPOSITCATEGORY = Base + "/depositcategory/upload/depositcategory";
        }

        public class Directors
        {
            public const string ADD_UPDATE_DIRECTORS = Base + "/accountopening/add/update/directors";
            public const string UPLOAD_DIRECTORS = Base + "/accountopening/upload/directorssignature";
            public const string GET_ALL_DIRECTORS = Base + "/accountopening/get/all/directors";
            public const string GET_DIRECTORS_BY_ID = Base + "/accountopening/get/directors";
            public const string DELETE_DIRECTORS = Base + "/accountopening/delete/directors";
        }

        public class DocumentUpload
        {
            public const string ADD_UPDATE_DOCUMENTUPLOAD = Base + "/accountopening/add/update/documentupload";
            public const string GET_ALL_DOCUMENTUPLOAD = Base + "/accountopening/get/all/documentupload";
            public const string GET_DOCUMENTUPLOAD_BY_ID = Base + "/accountopening/get/documentupload";
            public const string DELETE_DOCUMENTUPLOAD = Base + "/accountopening/delete/documentupload";
        }

        public class IdentityDetails
        {
            public const string ADD_UPDATE_IDENTITYDETAILS = Base + "/accountopening/add/update/identitydetails";
            public const string GET_ALL_IDENTITYDETAILS = Base + "/accountopening/get/all/identitydetails";
            public const string GET_IDENTITYDETAILS_BY_ID = Base + "/accountopening/get/identitydetails";
            public const string DELETE_IDENTITYDETAILS = Base + "/accountopening/delete/identitydetails";
        }

        public class KYCustomer
        {
            public const string ADD_UPDATE_KYCUSTOMER = Base + "/accountopening/add/update/kycustomer";
            public const string GET_ALL_KYCUSTOMER = Base + "/accountopening/get/all/kycustomer";
            public const string GET_KYCUSTOMER_BY_ID = Base + "/accountopening/get/kycustomer";
            public const string DELETE_KYCUSTOMER = Base + "/accountopening/delete/kycustomer";
        }

        public class NextOfKin
        {
            public const string ADD_UPDATE_NEXTOFKIN = Base + "/accountopening/add/update/nextofkin";
            public const string GET_ALL_NEXTOFKIN = Base + "/accountopening/get/all/nextofkin";
            public const string GET_NEXTOFKIN_BY_ID = Base + "/accountopening/get/nextofkin";
            public const string DELETE_NEXTOFKIN = Base + "/accountopening/delete/nextofkin";
        }

        public class SignatureUpload
        {
            public const string ADD_UPDATE_SIGNATUREUPLOAD = Base + "/accountopening/add/update/signatureupload";
            public const string GET_ALL_SIGNATUREUPLOAD = Base + "/accountopening/get/all/signatureupload";
            public const string GET_SIGNATUREUPLOAD_BY_ID = Base + "/accountopening/get/signatureuploadid";
            public const string GET_SIGNATUREUPLOAD_BY_IDS = Base + "/accountopening/get/signatureuploadbyids";
            public const string DELETE_SIGNATUREUPLOAD = Base + "/accountopening/delete/signatureupload";
        }

        public class Signatory
        {
            public const string ADD_UPDATE_SIGNATORY = Base + "/accountopening/add/update/signatory";
            public const string UPLOAD_SIGNATORY = Base + "/accountopening/upload/signatory";
            public const string GET_ALL_SIGNATORY = Base + "/accountopening/get/all/signatory";
            public const string GET_SIGNATORY_BY_ID = Base + "/accountopening/get/signatory";
            public const string DELETE_SIGNATORY = Base + "/accountopening/delete/signatory";
        }


        public class TillVault
        {
            public const string ADD_UPDATE_TILL_VAULT = Base + "/tillvault/add/update/tillVault";
            public const string ADD_UPDATE_TILL_VAULT_SETUP = Base + "/tillvault/add/update/tillVaultSetup";
            public const string DELETE_TILL_VAULT = Base + "/tillvault/delete/tillVault";
            public const string DELETE_TILL_VAULT_SETUP = Base + "/tillvault/delete/tillVaultSetup";
            public const string GET_SINGLE_TILL_VAULT_SETUP = Base + "/tillvault/get/singel/tillVaultSetup";
            public const string GET_ALL_TILL_VAULT_SETUP = Base + "/tillvault/get/all/tillVaultSetup";
            public const string DOWNLOAD_TILL_VAULT_SETUP = Base + "/tillvault/download/tillVaultSetup";
            public const string UPLOAD_TILL_VAULT_SETUP = Base + "/tillvault/get/upload/tillVaultSetup";

        }
         
        public class TransactionCorrection
        {
            public const string ADD_UPDATE_TRANSACTION_CORRECTION_SETUP = Base + "/transactioncorrectionsetup/add/update/transactioncorrectionsetup";
            public const string GET_ALL_TRANSACTION_CORRECTION_SETUP = Base + "/transactioncorrectionsetup/get/all/transactioncorrectionsetup";
            public const string GET_TRANSACTION_CORRECTION_SETUP_BY_ID = Base + "/transactioncorrectionsetup/get/single/transactioncorrectionsetup";
            public const string DELETE_TRANSACTION_CORRECTION_SETUP = Base + "/transactioncorrectionsetup/delete/transactioncorrectionsetup";
            public const string DOWNLOAD_TRANSACTION_CORRECTION_SETUP = Base + "/transactioncorrectionsetup/download/transactioncorrectionsetup";
            public const string UPLOAD_TRANSACTION_CORRECTION_SETUP = Base + "/transactioncorrectionsetup/upload/transactioncorrectionsetup"; 
        }

        public class TransactionCharge
        {
            public const string ADD_UPDATE_TRANSACTIONCHARGE = Base + "/transactioncharge/add/update/transactioncharge";
            public const string GET_ALL_TRANSACTIONCHARGE = Base + "/transactioncharge/get/all/transactioncharge";
            public const string GET_TRANSACTIONCHARGE_BY_ID = Base + "/transactioncharge/get/transactionchargebyid";
            public const string DELETE_TRANSACTIONCHARGE = Base + "/transactioncharge/delete/transactioncharge";
            public const string DOWNLOAD_TRANSACTIONCHARGE = Base + "/transactioncharge/download/transactioncharge";
            public const string UPLOAD_TRANSACTIONCHARGE = Base + "/transactioncharge/upload/transactioncharge";
        }

        public class TransactionTax
        {
            public const string ADD_UPDATE_TRANSACTIONTAX = Base + "/transactiontax/add/update/transactiontax";
            public const string GET_ALL_TRANSACTIONTAX = Base + "/transactiontax/get/all/transactiontax";
            public const string GET_TRANSACTIONTAX_BY_ID = Base + "/transactiontax/get/transactiontaxbyid";
            public const string DELETE_TRANSACTIONTAX = Base + "/transactiontax/delete/transactiontax";
            public const string DOWNLOAD_TRANSACTIONTAX = Base + "/transactiontax/download/transactiontax";
            public const string UPLOAD_TRANSACTIONTAX = Base + "/transactiontax/upload/transactiontax";
        }

        public class Transfer
        {
            public const string ADD_UPDATE_TRANSFER_SETUP = Base + "/transfer/add/update/transfersetup";
            public const string GET_ALL_TRANSFER_SETUP = Base + "/transfer/get/all/transfersetup";
            public const string GET_TRANSFER_SETUP_BY_ID = Base + "/transfer/get/transfersetupid";
            public const string DELETE_TRANSFER_SETUP = Base + "/transfer/delete/transfersetup";
            public const string DOWNLOAD_TRANSFER_SETUP = Base + "/transfer/download/transfersetup";
            public const string UPLOAD_TRANSFER_SETUP = Base + "/transfer/upload/transfersetup";

            public const string ADD_UPDATE_TRANSFER = Base + "/transfer/add/update/transfer";
            public const string GET_ALL_TRANSFER = Base + "/transfer/get/all/transfer";
            public const string GET_TRANSFER_BY_ID = Base + "/transfer/get/transferid";
            public const string DELETE_TRANSFER = Base + "/transfer/delete/transfer";
        }

        public class Withdrawal
        {
            public const string ADD_UPDATE_WITHDRAWAL_SETUP = Base + "/withdrawal/add/update/withdrawalsetup";
            public const string GET_ALL_WITHDRAWAL_SETUP = Base + "/withdrawal/get/all/withdrawalsetup";
            public const string GET_WITHDRAWAL_SETUP_BY_ID = Base + "/withdrawal/get/withdrawalsetupid";
            public const string DELETE_WITHDRAWAL_SETUP = Base + "/withdrawal/delete/withdrawalsetup";
            public const string DOWNLOAD_WITHDRAWAL_SETUP = Base + "/withdrawal/download/withdrawalsetup";
            public const string UPLOAD_WITHDRAWAL_SETUP = Base + "/withdrawal/upload/withdrawalsetup";

            public const string ADD_UPDATE_WITHDRAWAL = Base + "/withdrawal/add/update/withdrawal";
            public const string GET_ALL_WITHDRAWAL = Base + "/withdrawal/get/all/withdrawal";
            public const string GET_WITHDRAWAL_BY_ID = Base + "/withdrawal/get/withdrawalid";
            public const string DELETE_WITHDRAWAL = Base + "/withdrawal/delete/withdrawal";
        }

        #endregion
 
    }
}
