using Deposit.AuthHandler.Interface;
using Deposit.Contracts.Response.Deposit;
using Deposit.Repository.Interface.Deposit;
using Deposit.Data;
using GOSLibraries.Enums;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Deposit.DomainObjects.Deposit;

namespace Deposit.Repository.Implement.Deposit
{
    public class AccountSetupService : IAccountSetupService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;

        public string CategoryName { get; private set; }

        public AccountSetupService(DataContext dataContext, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _identityService = identityService;
        }

        bool AddORremovecharges(int[] charge_id, int accountsetupId)
        {
            var existing_charges = _dataContext.Account_setup_transaction_charges.Where(e => e.DepositAccountId == accountsetupId).ToList();
            if (existing_charges.Any()) 
                _dataContext.Account_setup_transaction_charges.RemoveRange(existing_charges);
            _dataContext.SaveChanges();

            List<Account_setup_transaction_charges> new_charges = new List<Account_setup_transaction_charges>();
            if (charge_id.Any())
            {
                charge_id.ToList().ForEach(c =>
                 new_charges.Add(new Account_setup_transaction_charges()
                 {
                     DepositAccountId = accountsetupId,
                     TransactionChargeId = c, 
                 }));
                _dataContext.Account_setup_transaction_charges.AddRange(new_charges);
                _dataContext.SaveChanges();
            }
            return true;
        }

        bool AddORremovetaxes(int[] tax_ids, int accountsetupId)
        {
            var existing_taxes = _dataContext.Account_setup_transaction_tax.Where(e => e.DepositAccountId == accountsetupId).ToList();
            if (existing_taxes.Any())
                _dataContext.Account_setup_transaction_tax.RemoveRange(existing_taxes);
            _dataContext.SaveChanges();

            List<Account_setup_transaction_tax> new_taxes = new List<Account_setup_transaction_tax>();
            if (tax_ids.Any())
            {
                tax_ids.ToList().ForEach(c =>
                 new_taxes.Add(new Account_setup_transaction_tax()
                 {
                     DepositAccountId = accountsetupId,
                     TransactionTaxId = c,
                 }));
                _dataContext.Account_setup_transaction_tax.AddRange(new_taxes);
                _dataContext.SaveChanges();
            }
            return true;
        }

        async Task<bool> IAccountSetupService.AddUpdateAccountSetupAsync(AddUpdateAccountSetupObj model)
        {
            try
            { 
                var domainObj = _dataContext.deposit_accountsetup.Find(model.DepositAccountId);
                if (domainObj == null)
                    domainObj = new deposit_accountsetup();

                domainObj.DepositAccountId = model.DepositAccountId;
                domainObj.Description = model.Description;
                domainObj.AccountName = model.AccountName;
                domainObj.AccountTypeId = model.AccountTypeId;
                domainObj.DormancyDays = model.DormancyDays;
                domainObj.DomancyDateCount = DateTime.UtcNow.AddDays(model.DormancyDays);
                domainObj.InitialDeposit = model.InitialDeposit;
                domainObj.CategoryId = model.CategoryId; 
                domainObj.CurrencyId = model.CurrencyId; 
                domainObj.InterestRate = model.InterestRate;
                domainObj.InterestType = model.InterestType;
                domainObj.CheckCollecting = model.CheckCollecting;
                domainObj.MaturityType = model.MaturityType;
                domainObj.PreTerminationLiquidationCharge = model.PreTerminationLiquidationCharge;
                domainObj.InterestAccrual = model.InterestAccrual;
                domainObj.Status = model.Status;
                domainObj.OperatedByAnother = model.OperatedByAnother;
                domainObj.CanNominateBenefactor = model.CanNominateBenefactor;
                domainObj.UsePresetChartofAccount = model.UsePresetChartofAccount;
                domainObj.TransactionPrefix = model.TransactionPrefix;
                domainObj.CancelPrefix = model.CancelPrefix;
                domainObj.RefundPrefix = model.RefundPrefix;
                domainObj.Useworkflow = model.Useworkflow;
                domainObj.CanPlaceOnLien = model.CanPlaceOnLien;
                
                if (domainObj.DepositAccountId == 0)
                    _dataContext.deposit_accountsetup.Add(domainObj);
                await _dataContext.SaveChangesAsync();

                AddORremovecharges(model.ApplicableChargesId, domainObj.DepositAccountId);
                AddORremovetaxes(model.ApplicableTaxId, domainObj.DepositAccountId);

                return true;
            }
            catch (Exception ex)
            { 
                throw ex;
            } 
        }
         
     
        public async Task<bool> DeleteAccountSetupAsync(int id)
        {
            var item = await _dataContext.deposit_accountsetup.FindAsync(id);
            if(item != null)
            {
                item.Deleted = true;
                await _dataContext.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<DepositAccountObj>> GetAllAccountSetupAsync()
        {
            return await _dataContext.deposit_accountsetup.Where(e => e.Deleted == false).Select(r => new DepositAccountObj(r)).ToListAsync();
        }

         
        public async Task<DepositAccountObj> GetAccountSetupByIdAsync(int id)
        {
            try
            {
                var item = _dataContext.deposit_accountsetup
                    .Include(a => a.Account_Setup_Transaction_Taxes)
                    .Include(a => a.Account_setup_transaction_charges)
                    .Include(a => a.deposit_category)
                    .Include(a => a.deposit_accountype)
                    .Where(e => e.DepositAccountId == id).Select(a => new DepositAccountObj(a)).ToList().FirstOrDefault();
             
                return await Task.Run(() => item);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<string> UploadAccountSetupAsync(List<byte[]> record)
        {
            try
            { 
                List<DepositAccountObj> uploadedRecord = new List<DepositAccountObj>();

                foreach (var byteItem in record)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (MemoryStream stream = new MemoryStream(byteItem))
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                        int totalRows = workSheet.Dimension.Rows;
                        int columns = workSheet.Dimension.Columns;

                        for (int i = 2; i <= totalRows; i++)
                        {
                            var data = new DepositAccountObj();
                            data.ExcelLineNumber = i;
                            data.AccountName = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : null;
                            data.AccountTypename = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : null;
                            data.DormancyDays = workSheet.Cells[i, 3].Value != null ? int.Parse(workSheet.Cells[i, 3].Value.ToString()) : 0;
                            data.InitialDeposit = workSheet.Cells[i, 4].Value != null ? decimal.Parse(workSheet.Cells[i, 4].Value.ToString()) : 0;
                            data.CategoryName = workSheet.Cells[i, 5].Value != null ? workSheet.Cells[i, 5].Value.ToString() : null;
                            data.InterestType = workSheet.Cells[i, 6].Value != null ? workSheet.Cells[i, 6].Value.ToString() : string.Empty;
                            data.InterestRate = decimal.Parse(workSheet.Cells[i, 7].Value.ToString()); 
                            data.MaturityType = workSheet.Cells[i, 8].Value != null ? workSheet.Cells[i, 8].Value.ToString() : null; 
                            data.TransactionPrefix = workSheet.Cells[i, 9].Value != null ? workSheet.Cells[i, 9].Value.ToString() : null;
                            data.CancelPrefix = workSheet.Cells[i, 10].Value != null ? workSheet.Cells[i, 10].Value.ToString() : null;
                            data.RefundPrefix = workSheet.Cells[i, 11].Value != null ? workSheet.Cells[i, 11].Value.ToString() : null;
                            data.OperatedByAnother = workSheet.Cells[i, 12].Value != null ? bool.Parse(workSheet.Cells[i, 12].Value.ToString()) : false;
                            data.UsePresetChartofAccount = workSheet.Cells[i, 13].Value != null ? bool.Parse(workSheet.Cells[i, 13].Value.ToString()) : false;
                            data.PreTerminationLiquidationCharge = workSheet.Cells[i, 14].Value != null ? bool.Parse(workSheet.Cells[i, 14].Value.ToString()) : false;
                            data.Status = workSheet.Cells[i, 15].Value != null ? bool.Parse(workSheet.Cells[i, 15].Value.ToString()) : false;
                            data.CanNominateBenefactor = workSheet.Cells[i, 16].Value != null ? bool.Parse(workSheet.Cells[i, 16].Value.ToString()) : false;
                            data.Useworkflow = workSheet.Cells[i, 17].Value != null ? bool.Parse(workSheet.Cells[i, 17].Value.ToString()) : false;
                            data.CheckCollecting = workSheet.Cells[i, 18].Value != null ? bool.Parse(workSheet.Cells[i, 18].Value.ToString()) : false;
                            data.CanPlaceOnLien = workSheet.Cells[i, 19].Value != null ? bool.Parse(workSheet.Cells[i, 19].Value.ToString()) : false;
                            uploadedRecord.Add(data);
                        }
                        stream.Flush();
                        excelPackage.Dispose();
                    }
                }
 
                if (uploadedRecord.Count > 0)
                {
                    foreach (var entity in uploadedRecord)
                    {
                        if (entity.DormancyDays == 0)
                            return $"Dormancy days cannot be empty detected on line {entity.ExcelLineNumber}";

                        if (string.IsNullOrEmpty(entity.AccountName))
                            return $"Account name cannot be empty detected on line {entity.ExcelLineNumber}";

                        var acttype = _dataContext.deposit_accountype.FirstOrDefault(x => x.Name == entity.AccountTypename)?.AccountTypeId ?? 0;
                        if(acttype == 0) 
                            return $"Unidentified Account type detected on line {entity.ExcelLineNumber}"; 

                        var category = _dataContext.deposit_category.FirstOrDefault(x => x.Name == entity.CategoryName)?.CategoryId??0;
                        if (category == 0)
                            return $"Unidentified Category detected on line {entity.ExcelLineNumber}";

                        var accountType =  _dataContext.deposit_accountsetup.Where(x => x.AccountName.ToLower() == entity.AccountName.ToLower()).FirstOrDefault();
                        if (accountType != null)
                        {
                            accountType.AccountName = entity.AccountName;
                            accountType.AccountTypeId = acttype;
                            accountType.DormancyDays = entity.DormancyDays;
                            accountType.InitialDeposit = entity.InitialDeposit;
                            accountType.CategoryId = category;
                            accountType.InterestType = entity.InterestType;
                            accountType.InterestRate = entity.InterestRate; 
                            accountType.MaturityType = entity.MaturityType;
                            accountType.InterestAccrual = entity.InterestAccrual;
                            accountType.TransactionPrefix = entity.TransactionPrefix;
                            accountType.CancelPrefix = entity.CancelPrefix;
                            accountType.RefundPrefix = entity.RefundPrefix;
                            accountType.OperatedByAnother = entity.OperatedByAnother;
                            accountType.UsePresetChartofAccount = entity.UsePresetChartofAccount;
                            accountType.PreTerminationLiquidationCharge = entity.PreTerminationLiquidationCharge;
                            accountType.Status = entity.Status;
                            accountType.CanNominateBenefactor = entity.CanNominateBenefactor;
                            accountType.Useworkflow = entity.Useworkflow;
                            accountType.CheckCollecting = entity.CheckCollecting;
                            accountType.CanPlaceOnLien = entity.CanPlaceOnLien;
                            _dataContext.SaveChanges();
                        }
                        else
                        {
                            accountType = new deposit_accountsetup();
                            accountType.AccountName = entity.AccountName;
                            accountType.AccountTypeId = acttype;
                            accountType.DormancyDays = entity.DormancyDays;
                            accountType.InitialDeposit = entity.InitialDeposit;
                            accountType.CategoryId = category;
                            accountType.InterestType = entity.InterestType;
                            accountType.InterestRate = entity.InterestRate;
                            accountType.MaturityType = entity.MaturityType;
                            accountType.InterestAccrual = entity.InterestAccrual;
                            accountType.TransactionPrefix = entity.TransactionPrefix;
                            accountType.CancelPrefix = entity.CancelPrefix;
                            accountType.RefundPrefix = entity.RefundPrefix;
                            accountType.OperatedByAnother = entity.OperatedByAnother;
                            accountType.UsePresetChartofAccount = entity.UsePresetChartofAccount;
                            accountType.PreTerminationLiquidationCharge = entity.PreTerminationLiquidationCharge;
                            accountType.Status = entity.Status;
                            accountType.CanNominateBenefactor = entity.CanNominateBenefactor;
                            accountType.Useworkflow = entity.Useworkflow;
                            accountType.CheckCollecting = entity.CheckCollecting;
                            accountType.CanPlaceOnLien = entity.CanPlaceOnLien;
                            _dataContext.deposit_accountsetup.Add(accountType);
                            _dataContext.SaveChanges();
                        }; 
                    }
                } 
                return "uploaded"; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] GenerateExportAccountSetup()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Account Name");
            dt.Columns.Add("Account Type");
            dt.Columns.Add("Dormancy Days");
            dt.Columns.Add("Initial Deposit");
            dt.Columns.Add("Category Available");
            dt.Columns.Add("Interest Type");
            dt.Columns.Add("Interest Rate"); 
            dt.Columns.Add("Maturity Type"); 
            dt.Columns.Add("Transaction Prefix");
            dt.Columns.Add("Cancel Prefix");
            dt.Columns.Add("Refund Prefix");
            dt.Columns.Add("Allow third party");
            dt.Columns.Add("Use preset COA");
            dt.Columns.Add("PTLC");
            dt.Columns.Add("Status");
            dt.Columns.Add("Nominate Benefactor");
            dt.Columns.Add("Use workflow");
            dt.Columns.Add("Cheque collecting");
            dt.Columns.Add("Can place on lien");

            var list = _dataContext.deposit_accountsetup
                  .Include(a => a.Account_Setup_Transaction_Taxes)
                  .Include(a => a.Account_setup_transaction_charges)
                  .Include(a => a.deposit_category)
                  .Include(a => a.deposit_accountype)
                  .Where(e => e.Deleted == false).Select(a => new DepositAccountObj(a)).ToList();

            foreach (var kk in list)
            {

                var row = dt.NewRow();
                row["Account Name"] = kk.AccountName;
                row["Account Type"] = kk.AccountTypename;
                row["Dormancy Days"] = kk.DormancyDays;
                row["Initial Deposit"] = kk.InitialDeposit;
                row["Category Available"] = kk.CategoryName;
                row["Interest Type"] = kk.InterestType;
                row["Interest Rate"] = kk.InterestRate; 
                row["Maturity Type"] = kk.MaturityType; 
                row["Transaction Prefix"] = kk.TransactionPrefix;
                row["Cancel Prefix"] = kk.CancelPrefix;
                row["Refund Prefix"] = kk.RefundPrefix;
                row["Allow third party"] = kk.OperatedByAnother;
                row["Use preset COA"] = kk.UsePresetChartofAccount;
                row["PTLC"] = kk.PreTerminationLiquidationCharge;
                row["Status"] = kk.Status;
                row["Nominate Benefactor"] = kk.CanNominateBenefactor;
                row["Use workflow"] = kk.Useworkflow;
                row["Cheque collecting"] = kk.CheckCollecting;
                row["Can place on lien"] = kk.CanPlaceOnLien;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (list != null && list.Any())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Account Setup");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }

 

      
    }
}
