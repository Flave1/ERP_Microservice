using Deposit.AuthHandler.Interface;
using Deposit.Contracts.Response.Deposit;
using Deposit.Repository.Interface.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GODP.Entities.Models;
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
    public class CashierTellerService : ICashierTellerService
    {
        private readonly DataContext _dataContext; 
        private readonly IIdentityServerRequest _serverRequest;

        public CashierTellerService(DataContext dataContext, IIdentityServerRequest serverRequest)
        {
            _serverRequest = serverRequest;
            _dataContext = dataContext; 
        }   

        #region CashierTellerSetup
        public async Task<bool> AddUpdateCashierTellerSetupAsync(deposit_cashiertellersetup model)
        {
            if (model.DepositCashierTellerSetupId == 0)
                await _dataContext.deposit_cashiertellersetup.AddAsync(model);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCashierTellerSetupAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_cashiertellersetup.FindAsync(id);
            if(itemToDelete != null) 
            { 
                itemToDelete.Deleted = true;
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<CashierTellerSetupObj>> GetAllCashierTellerSetupAsync()
        {
            var comp = await _serverRequest.GetAllCompanyAsync();
            var sraffList = await _serverRequest.GetAllStaffAsync();
            return await _dataContext.deposit_cashiertellersetup.Include(f => f.deposit_accountsetup).Where(x => x.Deleted == false).Select(f => new CashierTellerSetupObj(f, sraffList, comp)).ToListAsync();
        }

        CashierTellerSetupObj ICashierTellerService.GetCashierTellerSetupById(int id)
        { 
            return _dataContext.deposit_cashiertellersetup.Where(x => x.Deleted == false && x.DepositCashierTellerSetupId == id).Select(f => new CashierTellerSetupObj(f)).ToList().FirstOrDefault();
        }

        public async Task<string> UploadCashierTellerSetupAsync(List<byte[]> records)
        {
            try
            {
                List<CashierTellerSetupObj> uploadedRecord = new List<CashierTellerSetupObj>();
                foreach (var record in records)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (MemoryStream stream = new MemoryStream(record))
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                        int totalRows = workSheet.Dimension.Rows;

                        for (int i = 2; i <= totalRows; i++)
                        {
                            uploadedRecord.Add(new CashierTellerSetupObj
                            {
                                ExcelLine = i,
                                CompanyName = workSheet.Cells[i, 1]?.Value?.ToString(),
                                ProductName = workSheet.Cells[i, 2]?.Value?.ToString(),
                                Staff_name = workSheet.Cells[i, 3]?.Value?.ToString(),
                                Cashier_numer = workSheet.Cells[i, 4]?.Value?.ToString(),
                                PresetChart = workSheet.Cells[i, 5]?.Value != null ? bool.Parse(workSheet.Cells[i, 5]?.Value?.ToString()) : false,
                            });
                        }
                    }
                }
                var comp = await _serverRequest.GetAllCompanyAsync();
                var stf = await _serverRequest.GetAllStaffAsync();
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        if (string.IsNullOrEmpty(item.Cashier_numer))
                            return $"Sub strcuture is empty detected on line {item.ExcelLine}";

                        var staffid = stf.staff.FirstOrDefault(d => d.staffCode == item.Staff_name)?.staffId ?? 0;
                        if (staffid == 0)
                            return $"Unidentified staff code detected on line {item.ExcelLine}";

                        var compid = comp.companyStructures.FirstOrDefault(d => d.name == item.CompanyName)?.companyStructureId ?? 0;
                        if (compid == 0)
                            return $"Unidentified company name detected on line {item.ExcelLine}";

                        var prod = _dataContext.deposit_accountsetup.FirstOrDefault(d => d.AccountName == item.ProductName)?.DepositAccountId ?? 0;
                        if (prod == 0)
                            return $"Unidentified product name detected on line {item.ExcelLine}";
                        var ellersetup = _dataContext.deposit_cashiertellersetup.FirstOrDefault(x => x.Employee_ID == staffid  && x.Deleted == false);
                        if (ellersetup != null)
                        {
                            ellersetup.Structure = compid;
                            ellersetup.ProductId = prod;
                            ellersetup.Employee_ID = staffid;
                            ellersetup.PresetChart = item.PresetChart;
                            ellersetup.Sub_strructure = item.Cashier_numer;
                            _dataContext.SaveChanges();
                        }
                        else
                        {
                            ellersetup = new deposit_cashiertellersetup(); 
                            ellersetup.Structure = compid;
                            ellersetup.ProductId = prod;
                            ellersetup.Employee_ID = staffid;
                            ellersetup.PresetChart = item.PresetChart;
                            ellersetup.Sub_strructure = item.Cashier_numer;
                            await _dataContext.deposit_cashiertellersetup.AddAsync(ellersetup);
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

        public byte[] GenerateExportCashierTellerSetup()
        {
            var comp = _serverRequest.GetAllCompanyAsync().Result;
            DataTable dt = new DataTable();
            dt.Columns.Add("Company Name");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Staff Code");
            dt.Columns.Add("Sub Structure");
            dt.Columns.Add("Preset Chart");

            var list = GetAllCashierTellerSetupAsync().Result;
            foreach (var kk in list)
            {
                var row = dt.NewRow();
                row["Company Name"] = kk?.CompanyName;
                row["Product Name"] = kk.ProductName;
                row["Staff Code"] = kk?.Staff_code;
                row["Sub Structure"] = kk?.Cashier_numer;
                row["Preset Chart"] = kk.PresetChart;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (list != null && list.Any())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Cashier Teller setup");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region CashierTellerForm
        public async Task<bool> AddUpdateCashierTellerFormAsync(deposit_cashierteller_form model)
        {
            try
            {

                if (model.Id > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_cashierteller_form.FindAsync(model.Id);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_cashierteller_form.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteCashierTellerFormAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_cashierteller_form.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_cashierteller_form>> GetAllCashierTellerFormAsync()
        {
            return await _dataContext.deposit_cashierteller_form.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_cashierteller_form> GetCashierTellerFormByIdAsync(int id)
        {
            return await _dataContext.deposit_cashierteller_form.FindAsync(id);
        }

        #endregion
    }
}

