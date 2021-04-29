using Deposit.Contracts.Response.Deposit;
using AutoMapper;
using GODP.Entities.Models;

namespace Deposit.MapProfiles
{
    public class DomainToRequestProfiles : Profile
    {
        public DomainToRequestProfiles()
        {
            CreateMap<deposit_accountype, AccountTypeObj>();
            CreateMap<deposit_category, CategoryObj>();
            CreateMap<deposit_cashiertellersetup, CashierTellerSetupObj>();
            CreateMap<deposit_cashierteller_form, CashierTellerFormObj>();
            CreateMap<deposit_transactioncharge, TransactionChargeObj>();
            CreateMap<deposit_transactiontax, TransactionTaxObj>();
            CreateMap<deposit_customeridentification, CustomerIdentificationObj>();
            CreateMap<deposit_customernextofkin, CustomerNextOfKinObj>();
            CreateMap<deposit_customercontactpersons, CustomerContactPersonsObj>();
            CreateMap<deposit_customerkyc, deposit_customerkyc>();
            CreateMap<deposit_customerdirectors, CustomerDirectorsObj>();
            CreateMap<deposit_customersignatory, CustomerSignatoryObj>();
            CreateMap<deposit_customersignature, CustomerSignatureObj>();
            CreateMap<deposit_customerkycdocumentupload, KyCustomerDocUploadObj>();
            CreateMap<deposit_businesscategory, BusinessCategoryObj>();

            CreateMap<deposit_changeofratesetup, ChangeOfRateSetupObj>(); 
            
            
        }
    }
}
