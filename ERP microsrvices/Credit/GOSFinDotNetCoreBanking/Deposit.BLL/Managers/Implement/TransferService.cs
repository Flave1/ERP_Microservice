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
    public class TransferService : ITransferService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        private readonly IIdentityServerRequest _serverRequest;
        public TransferService(DataContext dataContext, IIdentityServerRequest serverRequest, IIdentityService identityService)
        {
            _serverRequest = serverRequest;
            _dataContext = dataContext;
            _identityService = identityService;
        }

        #region TransferSetup
        public async Task<bool> AddUpdateTransferSetupAsync(deposit_transfersetup model)
        {
            try
            {

                if (model.TransferSetupId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_transfersetup.FindAsync(model.TransferSetupId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_transfersetup.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteTransferSetupAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_transfersetup.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_transfersetup>> GetAllTransferSetupAsync()
        {
            return await _dataContext.deposit_transfersetup.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_transfersetup> GetTransferSetupByIdAsync(int id)
        {
            return await _dataContext.deposit_transfersetup.FindAsync(id);
        }

        public async Task<bool> UploadTransferSetupAsync(List<byte[]> record)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<TransferSetupObj> uploadedRecord = new List<TransferSetupObj>();
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
                                var item = new TransferSetupObj();
                                item. CompanyName = workSheet.Cells[i, 1].Value.ToString() != null ? workSheet.Cells[i, 1].Value.ToString() : string.Empty;
                                item.ProductName = workSheet.Cells[i, 2].Value.ToString() != null ? workSheet.Cells[i, 2].Value.ToString() : string.Empty;
                                item.AccountTypeName = workSheet.Cells[i, 3].Value.ToString() != null ? workSheet.Cells[i, 3].Value.ToString() : string.Empty;
                                item.DailyWithdrawalLimit = workSheet.Cells[i, 4].Value.ToString() != null ? workSheet.Cells[i, 4].Value.ToString() : string.Empty;
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
                        var compid = comp.companyStructures.FirstOrDefault(e => e.name.Trim().ToLower() == item.CompanyName.Trim().ToLower())?.companyStructureId;
                        var prod = _dataContext.deposit_accountsetup.FirstOrDefault(e => e.AccountName.Trim().ToLower() == item.ProductName.Trim().ToLower())?.DepositAccountId;
                        var type = _dataContext.deposit_accountype.FirstOrDefault(e => e.Name.Trim().ToLower() == item.AccountTypeName.Trim().ToLower())?.AccountTypeId; 

                        var Transferexist = _dataContext.deposit_transfersetup.FirstOrDefault(x => x.Structure == compid && x.Product == prod && x.Deleted == false);
                        if (Transferexist != null)
                        {
                            Transferexist.Product = prod;
                            Transferexist.Structure = compid;
                            Transferexist.DailyWithdrawalLimit = item.DailyWithdrawalLimit;
                            Transferexist.AccountType = type;
                        }

                        else
                        {
                            var Transfer = new deposit_transfersetup
                            {
                                Product = prod, 
                                Structure = compid,
                                DailyWithdrawalLimit = item.DailyWithdrawalLimit,
                                AccountType = type
                            };
                            await _dataContext.deposit_transfersetup.AddAsync(Transfer);
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
        public byte[] GenerateExportTransferSetup()
        {
            var comp = _serverRequest.GetAllCompanyAsync().Result;
            DataTable dt = new DataTable();
            dt.Columns.Add("Company");
            dt.Columns.Add("Product");
            dt.Columns.Add("Account type");
            dt.Columns.Add("Transfer Limit");
            var Transfer = (from a in _dataContext.deposit_transfersetup
                                 where a.Deleted == false
                                 select new TransferSetupObj
                                 {
                                     Product = a.Product,
                                     Structure = a.Structure,
                                     AccountType = a.AccountType,
                                     DailyWithdrawalLimit = a.DailyWithdrawalLimit,
                                 }).ToList();
            foreach (var kk in Transfer)
            {
                var row = dt.NewRow();
                row["Company"] = comp.companyStructures.FirstOrDefault(e => e.companyStructureId==  kk.Structure)?.name;
                row["Product"] = _dataContext.deposit_accountsetup.FirstOrDefault(e => e.DepositAccountId == kk.Product)?.AccountName;
                row["Account type"] = _dataContext.deposit_accountype.FirstOrDefault(e => e.AccountTypeId== kk.AccountType)?.Name;
                row["Transfer Limit"] = kk.DailyWithdrawalLimit;                
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (Transfer != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(" Transfer");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
        #endregion

        #region TransferForm
        public async Task<bool> AddUpdateTransferAsync(deposit_transferform model)
        {
            try
            {

                if (model.TransferFormId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_transferform.FindAsync(model.TransferFormId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_transferform.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteTransferAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_transferform.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_transferform>> GetAllTransferAsync()
        {
            return await _dataContext.deposit_transferform.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_transferform> GetTransferByIdAsync(int id)
        {
            return await _dataContext.deposit_transferform.FindAsync(id);
        }

        /*public async Task<bool> UploadTransferAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<deposit_transferform> uploadedRecord = new List<deposit_transferform>();
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
                                var item = new deposit_Transfer
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
                        var Transferexist = _dataContext.deposit_transferform.Where(x => x.TransferId == item.TransferId && x.Deleted == false).FirstOrDefault();
                        if (Transferexist != null)
                        {
                            Transferexist.Name = item.Name;
                            Transferexist.Description = item.Description;
                            Transferexist.Active = true;
                            Transferexist.Deleted = false;
                            Transferexist.UpdatedBy = item.UpdatedBy;
                            Transferexist.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var Transfer = new deposit_Transfer
                            {
                                Name = item.Name,
                                Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_transferform.AddAsync(Transfer);
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
        public byte[] GenerateExportTransfer()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var Transfer = (from a in _dataContext.deposit_Transfer
                                 where a.Deleted == false
                                 select new deposit_Transfer
                                 {
                                     Name = a.Name,
                                     TransferId = a.TransferId,
                                     Description = a.Description
                                 }).ToList();
            foreach (var kk in Transfer)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (Transfer != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(" Transfer");
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
