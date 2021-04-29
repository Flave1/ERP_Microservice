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
        private readonly IIdentityService _identityService;
        private readonly IIdentityServerRequest _serverRequest;

        public CashierTellerService(DataContext dataContext, IIdentityService identityService, IIdentityServerRequest serverRequest)
        {
            _serverRequest = serverRequest;
            _dataContext = dataContext;
            _identityService = identityService;
        }   

        #region CashierTellerSetup
        public async Task<bool> AddUpdateCashierTellerSetupAsync(deposit_cashiertellersetup model)
        {
            try
            {

                if (model.DepositCashierTellerSetupId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_cashiertellersetup.FindAsync(model.DepositCashierTellerSetupId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_cashiertellersetup.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteCashierTellerSetupAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_cashiertellersetup.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_cashiertellersetup>> GetAllCashierTellerSetupAsync()
        {
            return await _dataContext.deposit_cashiertellersetup.Where(x => x.Deleted == false).ToListAsync()??new List<deposit_cashiertellersetup>();
        }

        public async Task<deposit_cashiertellersetup> GetCashierTellerSetupByIdAsync(int id)
        {
            return await _dataContext.deposit_cashiertellersetup.FindAsync(id);
        }

        public async Task<bool> UploadCashierTellerSetupAsync(List<byte[]> records)
        {
            try
            { 
                List<CashierTellerSetupObj> uploadedRecord = new List<CashierTellerSetupObj>();
              foreach(var record in records)
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
                                CompanyName = workSheet.Cells[i, 1].Value.ToString(),
                                ProductName = workSheet.Cells[i, 2].Value.ToString(),
                                PresetChart = workSheet.Cells[i, 3].Value != null ? bool.Parse(workSheet.Cells[i, 3].Value.ToString()) : false,
                            });
                        }
                    }
                }
                var comp = _serverRequest.GetAllCompanyAsync().Result;
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var compid = comp.companyStructures.FirstOrDefault(d => d.name == item.CompanyName)?.companyStructureId;
                        var prod = _dataContext.deposit_accountsetup.FirstOrDefault(d => d.AccountName == item.ProductName)?.DepositAccountId;
                        var category = _dataContext.deposit_cashiertellersetup.Where(x => x.PresetChart == item.PresetChart && x.Deleted == false).FirstOrDefault();
                        if (category != null)
                        {
                            category.Structure = compid??0;
                            category.ProductId = prod??0;
                            category.PresetChart = item.PresetChart; 
                        }
                        else
                        {
                            var structure = new deposit_cashiertellersetup
                            {
                                Structure = compid??0,
                                ProductId = prod??0,
                                PresetChart = item.PresetChart,  
                            };
                            await _dataContext.deposit_cashiertellersetup.AddAsync(structure);
                        }
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

        public byte[] GenerateExportCashierTellerSetup()
        {
            var comp = _serverRequest.GetAllCompanyAsync().Result;
            DataTable dt = new DataTable();
            dt.Columns.Add("Company Name");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Preset Chart");
            var category = (from a in _dataContext.deposit_cashiertellersetup
                            where a.Deleted == false
                            select new deposit_cashiertellersetup
                            {
                                Structure = a.Structure,
                                ProductId = a.ProductId,
                                PresetChart = a.PresetChart
                            }).ToList();
            foreach (var kk in category)
            {
                var row = dt.NewRow();
                row["Company Name"] = comp.companyStructures.FirstOrDefault(d => d.companyStructureId == kk.Structure)?.name;
                row["Product Name"] = _dataContext.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == kk.ProductId)?.AccountName;
                row["Preset Chart"] = kk.PresetChart;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (category != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Cashier Teller");
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

