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
    public class WithdrawalService : IWithdrawalService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;
        private readonly IIdentityServerRequest _serverRequest;
        public WithdrawalService(DataContext dataContext, IIdentityService identityService, IIdentityServerRequest serverRequest)
        {
            _dataContext = dataContext;
            _identityService = identityService;
            _serverRequest = serverRequest;
        }

        #region Withdrawaletup
        public async Task<bool> AddUpdateWithdrawalSetupAsync(deposit_withdrawalsetup model)
        {
            try
            {

                if (model.WithdrawalSetupId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_withdrawalsetup.FindAsync(model.WithdrawalSetupId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_withdrawalsetup.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteWithdrawalSetupAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_withdrawalsetup.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<List<WithdrawalSetupObj>> GetAllWithdrawalSetupAsync()
        {
            var entity = (from a in _dataContext.deposit_withdrawalsetup
                          where a.Deleted == false
                          select

                         new WithdrawalSetupObj
                         {
                             WithdrawalSetupId = a.WithdrawalSetupId,
                             Structure = a.Structure, 
                             Product = a.Product,
                             ProductName = _dataContext.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == a.Product).AccountName,
                             PresetChart = a.PresetChart,
                             AccountType = a.AccountType,
                             AccountTypeName = _dataContext.deposit_accountype.FirstOrDefault(x => x.AccountTypeId == a.AccountType).Name,
                             DailyWithdrawalLimit = a.DailyWithdrawalLimit,
                             Charge = a.Charge,
                             WithdrawalCharges = a.WithdrawalCharges,
                             ChargeType = a.ChargeType,
                             
                         }).ToList()??new List<WithdrawalSetupObj>();

            return entity;
        }

        public async Task<deposit_withdrawalsetup> GetWithdrawalSetupByIdAsync(int id)
        {
            return await _dataContext.deposit_withdrawalsetup.FindAsync(id);
        }

        public async Task<bool> UploadWithdrawalSetupAsync(List<byte[]> record)
        {
            try
            { 
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
                                var item = new WithdrawalSetupObj
                                {
                                    CompanyName = workSheet.Cells[i, 1].Value.ToString(),
                                    ProductName = workSheet.Cells[i, 1].Value.ToString(),
                                    AccountTypeName = workSheet.Cells[i, 1].Value.ToString(),
                                    DailyWithdrawalLimit = decimal.Parse(workSheet.Cells[i, 1].Value.ToString())
                                };
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
                        var structureId = structure.companyStructures.FirstOrDefault(e => e.name == item.CompanyName)?.companyStructureId ?? 0;
                        var accountype = _dataContext.deposit_accountype.FirstOrDefault(f => f.Name == item.AccountTypeName)?.AccountTypeId;
                        var Withdrawalexist = _dataContext.deposit_withdrawalsetup.FirstOrDefault(x => x.WithdrawalSetupId == item.WithdrawalSetupId && x.Deleted == false);
                        if (Withdrawalexist != null)
                        {
                            Withdrawalexist.Product = productId;
                            Withdrawalexist.DailyWithdrawalLimit = item.DailyWithdrawalLimit;
                            Withdrawalexist.AccountType = accountype;
                            Withdrawalexist.Structure = structureId;
                        } 
                        else
                        {
                            Withdrawalexist = new deposit_withdrawalsetup();
                            Withdrawalexist.Product = productId;
                            Withdrawalexist.DailyWithdrawalLimit = item.DailyWithdrawalLimit;
                            Withdrawalexist.AccountType = accountype;
                            Withdrawalexist.Structure = structureId;
                            await _dataContext.deposit_withdrawalsetup.AddAsync(Withdrawalexist);
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
        public byte[] GenerateExportWithdrawalSetup()
        {
            var structure = _serverRequest.GetAllCompanyAsync().Result;
            DataTable dt = new DataTable();
            dt.Columns.Add("Company");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("Account Type");
            dt.Columns.Add("Withdrawal Limit");
            var Withdrawal = (from a in _dataContext.deposit_withdrawalsetup
                            where a.Deleted == false
                            select new deposit_withdrawalsetup
                            {
                                Product = a.Product,
                                Structure = a.Structure,
                                DailyWithdrawalLimit = a.DailyWithdrawalLimit,
                                PresetChart = a.PresetChart,
                                AccountType = a.AccountType,
                                WithdrawalCharges = a.WithdrawalCharges,

                            }).ToList();
            foreach (var kk in Withdrawal)
            {
                var row = dt.NewRow();
                row["Company"] = structure.companyStructures.FirstOrDefault(r => r.companyStructureId == kk.Structure)?.name;
                row["Product Name"] = _dataContext.deposit_accountsetup.FirstOrDefault(e => e.DepositAccountId == kk.Product)?.AccountName;
                row["Account Type"] = _dataContext.deposit_accountype.FirstOrDefault(e => e.AccountTypeId == kk.AccountType)?.Name; ;
                row["Withdrawal Limit"] = kk.DailyWithdrawalLimit;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (Withdrawal != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
            try
            {

                if (model.WithdrawalFormId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_withdrawalform.FindAsync(model.WithdrawalFormId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_withdrawalform.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteWithdrawalAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_withdrawalform.FindAsync(id);
            itemToDelete.Deleted = true;
            _dataContext.Entry(itemToDelete).CurrentValues.SetValues(itemToDelete);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_withdrawalform>> GetAllWithdrawalAsync()
        {
            return await _dataContext.deposit_withdrawalform.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_withdrawalform> GetWithdrawalByIdAsync(int id)
        {
            return await _dataContext.deposit_withdrawalform.FindAsync(id);
        }

       
        /*public async Task<bool> UploadWithdrawalAsync(List<byte[]> record, string createdBy)
        {
            try
            {
                if (record == null) return false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<deposit_withdrawalform> uploadedRecord = new List<deposit_withdrawalform>();
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
                                var item = new deposit_Withdrawal
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
                        var Withdrawalexist = _dataContext.deposit_withdrawalform.Where(x => x.WithdrawalId == item.WithdrawalId && x.Deleted == false).FirstOrDefault();
                        if (Withdrawalexist != null)
                        {
                            Withdrawalexist.Name = item.Name;
                            Withdrawalexist.Description = item.Description;
                            Withdrawalexist.Active = true;
                            Withdrawalexist.Deleted = false;
                            Withdrawalexist.UpdatedBy = item.UpdatedBy;
                            Withdrawalexist.UpdatedOn = DateTime.Now;
                        }

                        else
                        {
                            var Withdrawal = new deposit_Withdrawal
                            {
                                Name = item.Name,
                                Description = item.Description,
                                Active = true,
                                Deleted = false,
                                CreatedBy = createdBy,
                                CreatedOn = DateTime.Now,
                            };
                            await _dataContext.deposit_withdrawalform.AddAsync(Withdrawal);
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
        public byte[] GenerateExportWithdrawal()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var Withdrawal = (from a in _dataContext.deposit_Withdrawal
                                 where a.Deleted == false
                                 select new deposit_Withdrawal
                                 {
                                     Name = a.Name,
                                     WithdrawalId = a.WithdrawalId,
                                     Description = a.Description
                                 }).ToList();
            foreach (var kk in Withdrawal)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (Withdrawal != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(" Withdrawal");
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
