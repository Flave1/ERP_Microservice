using Deposit.AuthHandler.Interface;
using Deposit.Contracts.Response.Deposit;
using Deposit.Repository.Interface.Deposit;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.Enums;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<bool> AddUpdateAccountSetupAsync(deposit_accountsetup model)
        {
            using (var trans = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    if(model.DepositAccountId > 0)
                    {
                        _dataContext.Entry(model).CurrentValues.SetValues(model);
                    }
                    else
                        _dataContext.deposit_accountsetup.Add(model);

                    var output = _dataContext.SaveChanges() > 0;

                    var response = AddUpdateItems(model.DepositAccountId, model.ApplicableChargesId, model.ApplicableTaxId, model.CreatedBy);
                    if (response)
                    {
                        trans.Commit();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally { trans.Dispose(); }
            }
        }
         
        private bool AddUpdateItems(int DepositAccountId, int[] ApplicableChargesId, int[] ApplicableTaxId, string createdBy)
        {
            try
            {
                List<deposit_selectedTransactioncharge> selectCharge = new List<deposit_selectedTransactioncharge>();
                List<deposit_selectedTransactiontax> selectTax = new List<deposit_selectedTransactiontax>();
                if (DepositAccountId > 0)
                {
                    var targetCharge = _dataContext.deposit_selectedTransactioncharge.Where(x => x.AccountId == DepositAccountId).ToList();
                    var targetTax = _dataContext.deposit_selectedTransactiontax.Where(x => x.AccountId == DepositAccountId).ToList();
                    if (targetCharge.Any())
                    {
                        foreach (var item in targetCharge)
                        {
                            _dataContext.deposit_selectedTransactioncharge.Remove(item);
                        }
                    }
                    if (targetTax.Any())
                    {
                        foreach (var item in targetTax)
                        {
                            _dataContext.deposit_selectedTransactiontax.Remove(item);
                        }
                    }
                }

                if (ApplicableChargesId.Length > 0)
                {
                    foreach (var item in ApplicableChargesId)
                    {
                        var charge = new deposit_selectedTransactioncharge()
                        {
                            TransactionChargeId = item,
                            AccountId = DepositAccountId,
                            Active = true,
                            Deleted = false,
                            CreatedOn = DateTime.Now,
                            CreatedBy = createdBy,
                        };
                        _dataContext.deposit_selectedTransactioncharge.Add(charge);
                    }
                }

                if (ApplicableTaxId.Length > 0)
                {
                    foreach (var item in ApplicableTaxId)
                    {
                        var tax = new deposit_selectedTransactiontax()
                        {
                            TransactionTaxId = item,
                            AccountId = DepositAccountId,
                            Active = true,
                            Deleted = false,
                            CreatedOn = DateTime.Now,
                            CreatedBy = createdBy,
                        };
                        _dataContext.deposit_selectedTransactiontax.Add(tax);
                    }
                }
                var response = _dataContext.SaveChanges() > 0;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAccountSetupAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_accountsetup.FindAsync(id);
            if(itemToDelete != null)
            {
                itemToDelete.Deleted = true;
                _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            }
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<List<DepositAccountObj>> GetAllAccountSetupAsync()
        {
            try
            {

            var category = (from a in _dataContext.deposit_accountsetup
                            where a.Deleted == false
                            select
                           new DepositAccountObj
                           {
                               DepositAccountId = a.DepositAccountId,
                               AccountName = a.AccountName,
                               Description = a.Description,
                               AccountTypeId = a.AccountTypeId,
                               AccountTypename = _dataContext.deposit_accountype.Where(x => x.AccountTypeId == a.AccountTypeId).FirstOrDefault().Name,
                               DormancyDays = a.DormancyDays,
                               InitialDeposit = a.InitialDeposit,
                               CategoryId = a.CategoryId,
                               CategoryName = _dataContext.deposit_category.Where(x => x.CategoryId == a.CategoryId).FirstOrDefault().Name,
                               BusinessCategoryId = a.BusinessCategoryId,
                               InterestRate = a.InterestRate,
                               InterestType = a.InterestType,
                               CheckCollecting = a.CheckCollecting,
                               MaturityType = a.MaturityType,
                               GLMapping = a.GLMapping,
                               BankGl = a.BankGl,
                               PreTerminationLiquidationCharge = a.PreTerminationLiquidationCharge,
                               InterestAccrual = a.InterestAccrual,
                               InterestAccrualName = a.InterestAccrual == 1 ? "Day 0" : "Day 1",
                               Status = a.Status,
                               OperatedByAnother = a.OperatedByAnother,
                               CanNominateBenefactor = a.CanNominateBenefactor,
                               UsePresetChartofAccount = a.UsePresetChartofAccount,
                               TransactionPrefix = a.TransactionPrefix,
                               CancelPrefix = a.CancelPrefix,
                               RefundPrefix = a.RefundPrefix,
                               Useworkflow = a.Useworkflow,
                               CanPlaceOnLien = a.CanPlaceOnLien
                           }).ToList();
            return await Task.Run(()=>category);
        }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
    }
}


 


        public async Task<DepositAccountObj> GetAccountSetupByIdAsync(int id)
        {
            try
            {
                var category = (from a in _dataContext.deposit_accountsetup
                                where a.Deleted == false && a.DepositAccountId == id
                                select
                               new DepositAccountObj
                               {
                                   DepositAccountId = a.DepositAccountId,
                                   AccountName = a.AccountName,
                                   Description = a.Description,
                                   AccountTypeId = a.AccountTypeId,
                                   AccountTypename = _dataContext.deposit_accountype.FirstOrDefault(x => x.AccountTypeId == a.AccountTypeId).Name,
                                   DormancyDays = a.DormancyDays,
                                   InitialDeposit = a.InitialDeposit,
                                   CategoryId = a.CategoryId,
                                   CategoryName = _dataContext.deposit_category.FirstOrDefault(x => x.CategoryId == a.CategoryId).Name,
                                   BusinessCategoryId = a.BusinessCategoryId,
                                   InterestRate = a.InterestRate,
                                   InterestType = a.InterestType,
                                   CheckCollecting = a.CheckCollecting,
                                   MaturityType = a.MaturityType,
                                   GLMapping = a.GLMapping,
                                   BankGl = a.BankGl,
                                   PreTerminationLiquidationCharge = a.PreTerminationLiquidationCharge,
                                   InterestAccrual = a.InterestAccrual,
                                   InterestAccrualName = a.InterestAccrual == 1 ? "Day 0" : "Day 1",
                                   Status = a.Status,
                                   OperatedByAnother = a.OperatedByAnother,
                                   CanNominateBenefactor = a.CanNominateBenefactor,
                                   UsePresetChartofAccount = a.UsePresetChartofAccount,
                                   TransactionPrefix = a.TransactionPrefix,
                                   CancelPrefix = a.CancelPrefix,
                                   RefundPrefix = a.RefundPrefix,
                                   Useworkflow = a.Useworkflow,
                                   CanPlaceOnLien = a.CanPlaceOnLien,
                                   CurrencyId = a.CurrencyId, 
                               }).FirstOrDefault();
                if (category != null)
                {
                    category.ApplicableChargesId = _dataContext.deposit_selectedTransactioncharge.Where(d => d.AccountId == category.DepositAccountId).Select(x => (int)x.TransactionChargeId).ToArray();
                    category.ApplicableTaxId = _dataContext.deposit_selectedTransactiontax.Where(d => d.AccountId == category.DepositAccountId).Select(x => (int)x.TransactionTaxId).ToArray();
                }
                return await Task.Run(() => category);
            }
            catch (Exception)
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
                    }
                }
 
                if (uploadedRecord.Count > 0)
                {
                    foreach (var entity in uploadedRecord)
                    {
                        var acttype = _dataContext.deposit_accountype.FirstOrDefault(x => x.Name == entity.AccountTypename)?.AccountTypeId ?? 0;

                        var category = _dataContext.deposit_category.FirstOrDefault(x => x.Name == entity.CategoryName)?.CategoryId??0;

                        var accountTypeExist =  _dataContext.deposit_accountsetup.Where(x => x.AccountName.ToLower() == entity.AccountName.ToLower()).FirstOrDefault();
                        if (accountTypeExist != null)
                        {
                            accountTypeExist.AccountName = entity.AccountName;
                            accountTypeExist.AccountTypeId = acttype;
                            accountTypeExist.DormancyDays = entity.DormancyDays;
                            accountTypeExist.InitialDeposit = entity.InitialDeposit;
                            accountTypeExist.CategoryId = entity.CategoryId;
                            accountTypeExist.InterestType = entity.InterestType;
                            accountTypeExist.InterestRate = entity.InterestRate; 
                            accountTypeExist.MaturityType = entity.MaturityType;
                            accountTypeExist.InterestAccrual = entity.InterestAccrual;
                            accountTypeExist.TransactionPrefix = entity.TransactionPrefix;
                            accountTypeExist.CancelPrefix = entity.CancelPrefix;
                            accountTypeExist.RefundPrefix = entity.RefundPrefix;
                            accountTypeExist.OperatedByAnother = entity.OperatedByAnother;
                            accountTypeExist.UsePresetChartofAccount = entity.UsePresetChartofAccount;
                            accountTypeExist.PreTerminationLiquidationCharge = entity.PreTerminationLiquidationCharge;
                            accountTypeExist.Status = entity.Status;
                            accountTypeExist.CanNominateBenefactor = entity.CanNominateBenefactor;
                            accountTypeExist.Useworkflow = entity.Useworkflow;
                            accountTypeExist.CheckCollecting = entity.CheckCollecting;
                            accountTypeExist.CanPlaceOnLien = entity.CanPlaceOnLien; 
                        }
                        else
                        {
                            var accountType = new deposit_accountsetup
                            {
                                AccountName = entity.AccountName,
                                AccountTypeId = acttype,
                                DormancyDays = entity.DormancyDays,
                                InitialDeposit = entity.InitialDeposit,
                                CategoryId = entity.CategoryId,
                                InterestType = entity.InterestType,
                                InterestRate = entity.InterestRate, 
                                MaturityType = entity.MaturityType,
                                InterestAccrual = entity.InterestAccrual,
                                TransactionPrefix = entity.TransactionPrefix,
                                CancelPrefix = entity.CancelPrefix,
                                RefundPrefix = entity.RefundPrefix,
                                OperatedByAnother = entity.OperatedByAnother,
                                UsePresetChartofAccount = entity.UsePresetChartofAccount,
                                PreTerminationLiquidationCharge = entity.PreTerminationLiquidationCharge,
                                Status = entity.Status,
                                CanNominateBenefactor = entity.CanNominateBenefactor,
                                Useworkflow = entity.Useworkflow,
                                CheckCollecting = entity.CheckCollecting,
                                CanPlaceOnLien = entity.CanPlaceOnLien, 
                            };
                            await _dataContext.deposit_accountsetup.AddAsync(accountType);
                        }
                    }
                }

                var response = _dataContext.SaveChanges() > 0;
                return "uploaded";

            }
            catch (Exception ex)
            {
                return ex?.Message ?? ex?.InnerException?.Message;
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
            var statementType = (from a in _dataContext.deposit_accountsetup
                                 where a.Deleted == false
                                 select new DepositAccountObj
                                 {
                                     DepositAccountId = a.DepositAccountId,
                                     AccountName = a.AccountName,
                                     AccountTypeId = a.AccountTypeId,
                                     AccountTypename = _dataContext.deposit_accountype.Where(x => x.AccountTypeId == a.AccountTypeId).FirstOrDefault().Name,
                                     DormancyDays = a.DormancyDays,
                                     InitialDeposit = a.InitialDeposit,
                                     CategoryId = a.CategoryId, 
                                     CategoryName = _dataContext.deposit_category.Where(x => x.CategoryId == a.CategoryId).FirstOrDefault().Name,
                                     InterestType = a.InterestType,
                                     InterestRate = a.InterestRate,
                                     CurrencyId = a.CurrencyId, 
                                     MaturityType = a.MaturityType,
                                     InterestAccrual = a.InterestAccrual, 
                                     TransactionPrefix = a.TransactionPrefix,
                                     CancelPrefix = a.CancelPrefix,
                                     RefundPrefix = a.RefundPrefix,
                                     OperatedByAnother = a.OperatedByAnother,
                                     UsePresetChartofAccount = a.UsePresetChartofAccount,
                                     PreTerminationLiquidationCharge = a.PreTerminationLiquidationCharge,
                                     Status = a.Status,
                                     CanNominateBenefactor = a.CanNominateBenefactor,
                                     Useworkflow = a.Useworkflow,
                                     CheckCollecting = a.CheckCollecting,
                                     CanPlaceOnLien = a.CanPlaceOnLien
                                 }).ToList();

            foreach (var kk in statementType)
            {

                var row = dt.NewRow();
                row["Account Name"] = kk.AccountName;
                row["Account Type"] = kk.AccountTypename;
                row["Dormancy Days"] = kk.DormancyDays;
                row["Initial Deposit"] = kk.InitialDeposit;
                row["Category Available"] = kk.CategoryId;
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

            if (statementType != null)
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

        public bool AddUpdateDepositForm(DepositformObj entity)
        {
            return true;
            //try
            //{ 
            //    var Category  = _dataContext.deposit_depositform.Find(entity.DepositFormId);
            //    if (Category == null)
            //        Category = new deposit_depositform();


            //    Category.DepositFormId = entity.DepositFormId;
            //    Category.Structure = entity.Structure;
            //    Category.Operation = (int)OperationsEnum.DepositFormSubmit;
            //    Category.TransactionId = entity.TransactionId;
            //    Category.AccountNumber = entity.AccountNumber;
            //    Category.Amount = entity.Amount;
            //    Category.ValueDate = entity.ValueDate;
            //    Category.TransactionDate = DateTime.Now;
            //    Category.TransactionDescription = entity.TransactionDescription;
            //    Category.TransactionParticulars = entity.TransactionParticulars;
            //    Category.Remark = entity.Remark;
            //    Category.ModeOfTransaction = entity.ModeOfTransaction;
            //    Category.InstrumentNumber = entity.InstrumentNumber;
            //    Category.InstrumentDate = entity.InstrumentDate;
            //    Category.AccountId = entity.AccountId;

            //    var response = false;

            //    using (var trans = _dataContext.Database.BeginTransaction())
            //    {
            //        try
            //        {
            //            response = _dataContext.SaveChanges() > 0;
            //            var thisAccount = _dataContext.deposit_accountopening.Find(Category.AccountId);
            //            if(thisAccount != null)
            //            {
            //                thisAccount.IsAddedToDepositForm = true;
            //                _dataContext.SaveChanges();
            //            }
            //            if (response)
            //            {
            //                var casa = _dataContext.credit_casa.FirstOrDefault(x => x.AccountNumber == entity.AccountNumber);
            //                if(casa != null)
            //                {
            //                    casa.AvailableBalance = casa.AvailableBalance + entity.Amount;
            //                    casa.LedgerBalance = casa.LedgerBalance + entity.Amount;
            //                    _dataContext.SaveChanges();
            //                }
            //            } 
            //            trans.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            trans.Rollback();
            //            throw new Exception(ex.Message);
            //        }
            //    }
            //    return response;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }

        //public IEnumerable<DepositformObj> GetAllDepositForm()
        //{
        //    try
        //    {
        //        //var category = (from a in _dataContext.deposit_depositform
        //        //                join b in _dataContext.deposit_accountopening on a.AccountNumber equals b.AccountNumber
        //        //                where a.Deleted == false
        //        //                select

        //        //                       new DepositformObj
        //        //                       {
        //        //                           DepositFormId = a.DepositFormId,
        //        //                           Structure = a.Structure,
        //        //                           //CompanyName = _dataContext.cor_companystructure.Where(x => x.CompanyStructureId == a.Structure).FirstOrDefault().Name,
        //        //                           Operation = a.Operation,
        //        //                           TransactionId = a.TransactionId,
        //        //                           AccountNumber = a.AccountNumber,
        //        //                           AcountName = b.Firstname + " " + b.Surname,
        //        //                           AvailableBalance = _dataContext.credit_casa.Where(a => a.AccountNumber == b.AccountNumber).FirstOrDefault().AvailableBalance,
        //        //                           Amount = a.Amount,
        //        //                           ValueDate = a.ValueDate,
        //        //                           TransactionDate = a.TransactionDate,
        //        //                           TransactionDescription = a.TransactionDescription,
        //        //                           TransactionParticulars = a.TransactionParticulars,
        //        //                           Remark = a.Remark,
        //        //                           ModeOfTransaction = a.ModeOfTransaction,
        //        //                           InstrumentDate = a.InstrumentDate,
        //        //                           InstrumentNumber = a.InstrumentNumber,
        //        //                       }).ToList().OrderByDescending(x => x.TransactionDate);
        //        //return category;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
       

        //public DepositformObj GetDepositForm(int id)
        //{
        //    try
        //    {
        //        var category = (from a in _dataContext.deposit_depositform
        //                        join b in _dataContext.deposit_accountopening on a.AccountNumber equals b.AccountNumber
        //                        where a.Deleted == false
        //                        select
        //                               new DepositformObj
        //                               {
        //                                   DepositFormId = a.DepositFormId,
        //                                   Structure = a.Structure,
        //                                   //CompanyName = _dataContext.cor_companystructure.Where(x => x.CompanyStructureId == a.Structure).FirstOrDefault().Name,
        //                                   Operation = a.Operation,
        //                                   TransactionId = a.TransactionId,
        //                                   AccountNumber = a.AccountNumber,
        //                                   AcountName = b.Firstname + " " + b.Surname,
        //                                   AvailableBalance = _dataContext.credit_casa.Where(a => a.AccountNumber == b.AccountNumber).FirstOrDefault().AvailableBalance,
        //                                   Amount = a.Amount,
        //                                   ValueDate = a.ValueDate,
        //                                   TransactionDate = a.TransactionDate,
        //                                   TransactionDescription = a.TransactionDescription,
        //                                   TransactionParticulars = a.TransactionParticulars,
        //                                   Remark = a.Remark,
        //                                   ModeOfTransaction = a.ModeOfTransaction,
        //                                   InstrumentDate = a.InstrumentDate,
        //                                   InstrumentNumber = a.InstrumentNumber,
        //                               }).FirstOrDefault();
        //        return category;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public bool DeleteDepositionForm(int id)
        //{
        //    try
        //    {
        //        var depositForm = _dataContext.deposit_depositform.Find(id);
        //        if (depositForm != null)
        //        {
        //            depositForm.Deleted = true;
        //        }
        //        return _dataContext.SaveChanges() > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
