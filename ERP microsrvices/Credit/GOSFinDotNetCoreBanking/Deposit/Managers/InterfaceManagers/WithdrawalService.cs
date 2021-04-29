using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using GODP.Entities.Models;
using OfficeOpenXml;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Deposit.Repository.Implement.Deposit
{
    public class WithdrawalService : IWithdrawalService
    {
        private readonly DataContext _dataContext; 
        private readonly IIdentityServerRequest _serverRequest;
        public WithdrawalService(DataContext dataContext, IIdentityServerRequest serverRequest)
        {
            _dataContext = dataContext; 
            _serverRequest = serverRequest;
        }

        #region Withdrawaletup
        public async Task<bool> AddUpdateWithdrawalSetupAsync(deposit_withdrawalsetup model)
        {
            if (model.WithdrawalSetupId == 0)
                await _dataContext.deposit_withdrawalsetup.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteWithdrawalSetupAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_withdrawalsetup.FindAsync(id);
            if(itemToDelete != null)
            {
                itemToDelete.Deleted = true;
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<List<WithdrawalSetupObj>> GetAllWithdrawalSetupAsync()
        {
            var comp = await _serverRequest.GetAllCompanyAsync(); 
            return _dataContext.deposit_withdrawalsetup
                .Include(x => x.deposit_accountsetup)
                .Include(x => x.deposit_accountype).Where(e => e.Deleted == false).Select(r => new WithdrawalSetupObj(r, comp)).ToList(); 
        }
        public async Task<deposit_withdrawalsetup> GetWithdrawalSetupByIdAsync(int id)
        {
            return await _dataContext.deposit_withdrawalsetup.FindAsync(id);
        }

        public async Task<string> UploadWithdrawalSetupAsync(List<byte[]> record)
        {
            try
            { 
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                List<WithdrawalSetupObj> uploadedRecord = new List<WithdrawalSetupObj>();
                if (record.Count() > 0)
                {
                    foreach (var byteItem in record)
                    {
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;

                            for (int i = 2; i <= totalRows; i++)
                            {

                                var item = new WithdrawalSetupObj();
                                item.ExcelLine = i;
                                item.CompanyName = workSheet.Cells[i, 1]?.Value?.ToString();
                                item.ProductName = workSheet.Cells[i, 2]?.Value?.ToString();
                                item.AccountTypeName = workSheet.Cells[i, 3].Value.ToString(); 
                                item.DailyWithdrawalLimit = Convert.ToDecimal(workSheet.Cells[i, 4]?.Value?.ToString());
                                uploadedRecord.Add(item);
                            }
                        }
                    }

                }
                var structure = await _serverRequest.GetAllCompanyAsync();
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var productId = _dataContext.deposit_accountsetup.FirstOrDefault(x => x.AccountName == item.ProductName)?.DepositAccountId ?? 0;
                        if (productId == 0)
                            return $"Unidentified product name {item.ExcelLine}";
                        var structureId = structure.companyStructures.FirstOrDefault(e => e.name == item.CompanyName)?.companyStructureId ?? 0;
                        if (structureId == 0)
                            return $"Unidentified company name {item.ExcelLine}";

                        var accountype = _dataContext.deposit_accountype.FirstOrDefault(f => f.Name == item.AccountTypeName)?.AccountTypeId;
                        if (accountype == 0)
                            return $"Unidentified account type  {item.ExcelLine}";

                        var Withdrawalexist = _dataContext.deposit_withdrawalsetup.FirstOrDefault(x => structureId == x.Structure && x.Product == productId && accountype == x.AccountType &&  x.Deleted == false);
                        if (Withdrawalexist != null)
                        {
                            Withdrawalexist.Product = productId;
                            Withdrawalexist.DailyWithdrawalLimit = item.DailyWithdrawalLimit;
                            Withdrawalexist.AccountType = accountype;
                            Withdrawalexist.Structure = structureId;
                            _dataContext.SaveChanges();
                        } 
                        else
                        {
                            Withdrawalexist = new deposit_withdrawalsetup();
                            Withdrawalexist.Product = productId;
                            Withdrawalexist.DailyWithdrawalLimit = item.DailyWithdrawalLimit;
                            Withdrawalexist.AccountType = accountype;
                            Withdrawalexist.Structure = structureId;
                            await _dataContext.deposit_withdrawalsetup.AddAsync(Withdrawalexist);
                            _dataContext.SaveChanges();
                        }
                    }
                } 
                return "success"; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public byte[] GenerateExportWithdrawalSetup()
        {
            var structure = _serverRequest.GetAllCompanyAsync().Result;
            DataTable dt = new DataTable();
            dt.Columns.Add("Company");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Account Type");
            dt.Columns.Add("Withdrawal Limit");
            var Withdrawal = GetAllWithdrawalSetupAsync().Result;
            foreach (var kk in Withdrawal)
            {
                var row = dt.NewRow();
                row["Company"] = kk?.CompanyName;
                row["Product Name"] = kk.ProductName;
                row["Account Type"] = kk.AccountTypeName; ;
                row["Withdrawal Limit"] = kk.DailyWithdrawalLimit;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (Withdrawal != null)
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Withdrawal");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region WithdrawalForm
        public async Task<bool> AddUpdateWithdrawalAsync(deposit_withdrawalform model)
        {
            if (model.WithdrawalFormId == 0)
                await _dataContext.deposit_withdrawalform.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteWithdrawalAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_withdrawalform.FindAsync(id);
            if(itemToDelete != null)
            {
                itemToDelete.Deleted = true;
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<deposit_withdrawalform>> GetAllWithdrawalAsync()
        {
            return await _dataContext.deposit_withdrawalform.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_withdrawalform> GetWithdrawalByIdAsync(int id)
        {
            return await _dataContext.deposit_withdrawalform.FindAsync(id);
        }

       
         
        #endregion
    }
}
