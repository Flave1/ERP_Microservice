﻿using Deposit.AuthHandler.Interface;
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
    public class ChangeOfRatesService : IChangeOfRatesService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        private readonly IIdentityServerRequest _serverRequest;
        public ChangeOfRatesService(DataContext dataContext, IIdentityService identityService, IIdentityServerRequest serverRequest)
        {
            _dataContext = dataContext;
            _serverRequest = serverRequest;
            _identityService = identityService;
        }

        #region ChangeOfRateSetup
        public async Task<bool> AddUpdateChangeOfRatesSetupAsync(deposit_changeofratesetup model)
        {
            try
            {

                if (model.ChangeOfRateSetupId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_changeofratesetup.FindAsync(model.ChangeOfRateSetupId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_changeofratesetup.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteChangeOfRatesSetupAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_changeofratesetup.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_changeofratesetup>> GetAllChangeOfRatesSetupAsync()
        {
            return await _dataContext.deposit_changeofratesetup.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_changeofratesetup> GetChangeOfRatesSetupByIdAsync(int id)
        {
            return await _dataContext.deposit_changeofratesetup.FindAsync(id);
        }

        public async Task<bool> UploadChangeOfRatesSetupAsync(List<byte[]> record)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<ChangeOfRateSetupObj> uploadedRecord = new List<ChangeOfRateSetupObj>();
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
                                var item = new ChangeOfRateSetupObj
                                {
                                    CompanyName = workSheet.Cells[i, 1].Value.ToString(),
                                    ProductName = workSheet.Cells[i, 2].Value.ToString(),
                                    CanApply = bool.Parse(workSheet.Cells[i, 3].Value.ToString())
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }

                }
                var comp = _serverRequest.GetAllCompanyAsync().Result;
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var compnam = comp.companyStructures.FirstOrDefault(e => e.name == item.CompanyName)?.companyStructureId??0;
                        var prodName = _dataContext.deposit_accountsetup.FirstOrDefault(x => x.AccountName == item.ProductName)?.DepositAccountId??0;
                        var ChangeOfRatesexist = _dataContext.deposit_changeofratesetup.FirstOrDefault(x => x.Structure == compnam && x.ProductId == prodName && x.Deleted == false);
                        if (ChangeOfRatesexist != null)
                        {
                            ChangeOfRatesexist.CanApply = item.CanApply;
                            ChangeOfRatesexist.ProductId = prodName;
                            ChangeOfRatesexist.Structure = compnam; 
                        }

                        else
                        {
                            var ChangeOfRates = new deposit_changeofratesetup
                            {
                                CanApply = item.CanApply, 
                                Structure = compnam,
                                ProductId = prodName
                            };
                            await _dataContext.deposit_changeofratesetup.AddAsync(ChangeOfRates);
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
        public byte[] GenerateExportChangeOfRatesSetup()
        {
            try
            {
                var comp = _serverRequest.GetAllCompanyAsync().Result;
                DataTable dt = new DataTable(); 
                dt.Columns.Add("Structure");
                dt.Columns.Add("Product");
                dt.Columns.Add("Can Apply");
                var ChangeOfRates = (from a in _dataContext.deposit_changeofratesetup
                                     where a.Deleted == false
                                     select new deposit_changeofratesetup
                                     {
                                         CanApply = a.CanApply, 
                                         Structure = a.Structure,
                                         ProductId = a.ProductId
                                     }).ToList();
                foreach (var kk in ChangeOfRates)
                {
                    var row = dt.NewRow();
                    row["Structure"] = comp.companyStructures.FirstOrDefault(e => e.companyStructureId ==  kk.Structure)?.name;
                    row["Product"] = _dataContext.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == kk.ProductId)?.AccountName;
                    row["Can Apply"] = kk.CanApply;
                    dt.Rows.Add(row);
                }
                Byte[] fileBytes = null;

                if (ChangeOfRates != null)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add(" ChangeOfRates");
                        ws.DefaultColWidth = 20;
                        ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                        fileBytes = pck.GetAsByteArray();
                    }
                }
                return fileBytes;
            }
            catch (Exception ex)
            { 
                throw ex;
            }
           
        }
        #endregion

        #region ChangeOfRateForm
        public async Task<bool> AddUpdateChangeOfRatesAsync(deposit_changeofrates model)
        {
            try
            {

                if (model.ChangeOfRateId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_changeofrates.FindAsync(model.ChangeOfRateId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_changeofrates.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteChangeOfRatesAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_changeofrates.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_changeofrates>> GetAllChangeOfRatesAsync()
        {
            return await _dataContext.deposit_changeofrates.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_changeofrates> GetChangeOfRatesByIdAsync(int id)
        {
            return await _dataContext.deposit_changeofrates.FindAsync(id);
        }

        /*public async Task<bool> UploadChangeOfRatesAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<deposit_changeofrates> uploadedRecord = new List<deposit_changeofrates>();
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
                                var item = new deposit_changeofrates
                                {
                                    can = workSheet.Cells[i, 1].Value.ToString(),
                                    Description = workSheet.Cells[i, 2].Value.ToString()
                                };
                                uploadedRecord.Add(item);
                            }
                        }
                    }

                }
                if (uploadedRecord.Count > 0)
                {
                    foreach (var item in uploadedRecord)
                    {
                        var ChangeOfRatesexist = _dataContext.deposit_changeofrates.Where(x => x.ChangeOfRatesId == item.ChangeOfRatesId && x.Deleted == false).FirstOrDefault();
                        if (ChangeOfRatesexist != null)
                        {
                            ChangeOfRatesexist.Name = item.Name;
                            ChangeOfRatesexist.Description = item.Description;
                            ChangeOfRatesexist.Active = true;
                            ChangeOfRatesexist.Deleted = false;
                            ChangeOfRatesexist.UpdatedBy = item.UpdatedBy;
                            ChangeOfRatesexist.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var ChangeOfRates = new deposit_changeofrates
                            {
                                Name = item.Name,
                                Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_changeofrates.AddAsync(ChangeOfRates);
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
        public byte[] GenerateExportChangeOfRates()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var ChangeOfRates = (from a in _dataContext.deposit_changeofrates
                                 where a.Deleted == false
                                 select new deposit_changeofrates
                                 {
                                     Name = a.Name,
                                     ChangeOfRatesId = a.ChangeOfRatesId,
                                     Description = a.Description
                                 }).ToList();
            foreach (var kk in ChangeOfRates)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (ChangeOfRates != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(" ChangeOfRates");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }*/
        #endregion
    }
}
