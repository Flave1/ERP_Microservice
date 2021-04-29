﻿using Deposit.AuthHandler.Interface;
using Deposit.Repository.Interface.Deposit;
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
    public class BusinessCategoryService : IBusinessCategoryService
    {
        private readonly DataContext _dataContext;
        private readonly IIdentityService _identityService;


        public BusinessCategoryService(DataContext dataContext, IIdentityService identityService)
        {
            _dataContext = dataContext;
            _identityService = identityService;
        }
        public async Task<bool> AddUpdateBusinessCategoryAsync(deposit_businesscategory model)
        {
            try
            {
                
                if (model.BusinessCategoryId > 0)
                {
                    var itemToUpdate = await _dataContext.deposit_businesscategory.FindAsync(model.BusinessCategoryId);
                    _dataContext.Entry(itemToUpdate).CurrentValues.SetValues(model);
                }
                else
                    await _dataContext.deposit_businesscategory.AddAsync(model);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteBusinessCategoryAsync(int id)
        {
            var itemToDelete = await _dataContext.deposit_businesscategory.FindAsync(id);
            if(itemToDelete != null)
            {
                _dataContext.deposit_businesscategory.Remove(itemToDelete); 
            } 
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<deposit_businesscategory>> GetAllBusinessCategoryAsync()
        {
            return await _dataContext.deposit_businesscategory.Where(x => x.Deleted == false).ToListAsync();
        }

        public async Task<deposit_businesscategory> GetBusinessCategoryByIdAsync(int id)
        {
            return await _dataContext.deposit_businesscategory.FindAsync(id);
        }

        public async Task<string> UploadBusinessCategoryAsync(List<byte[]> record)
        {
            try
            { 
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                List<deposit_businesscategory> uploadedRecord = new List<deposit_businesscategory>();
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
                                var item = new deposit_businesscategory
                                {
                                    Name = workSheet.Cells[i, 1].Value.ToString(),
                                    Description = workSheet.Cells[i, 2].Value.ToString(),
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
                        var category = _dataContext.deposit_businesscategory.FirstOrDefault(x => x.Name == item.Name && x.Deleted == false);
                        if (category != null)
                        {
                            category.Name = item.Name;
                            category.Description = item.Description; 
                        }

                        else
                        {
                            var businesscategory = new deposit_businesscategory
                            {
                                Name = item.Name,
                                Description = item.Description
                            };
                            await _dataContext.deposit_businesscategory.AddAsync(businesscategory);
                        }
                    }
                }

                var response = _dataContext.SaveChanges() > 0;
                return "uploaded";

            }
            catch (Exception ex)
            {
                return ex?.Message?? ex?.InnerException?.Message;
            }
        }

        public byte[] GenerateExportBusinessCategory()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            var category = (from a in _dataContext.deposit_businesscategory
                              where a.Deleted == false
                              select new deposit_businesscategory
                              {
                                  Name = a.Name,
                                  BusinessCategoryId = a.BusinessCategoryId,
                                  Description = a.Description
                              }).ToList();
            foreach (var kk in category)
            {
                var row = dt.NewRow();
                row["Name"] = kk.Name;
                row["Description"] = kk.Description;
                dt.Rows.Add(row);
            }
            Byte[] fileBytes = null;

            if (category != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Business category");
                    ws.DefaultColWidth = 20;
                    ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                    fileBytes = pck.GetAsByteArray();
                }
            }
            return fileBytes;
        }
    }
}


