using Deposit.Contracts.Response.Common;
using Deposit.Contracts.Response.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.BankClosure
{

    public class UploadTiilVaultsSetup : IRequest<UploadResponse>
    {
        public class UploadTiilVaultsSetupHandler : IRequestHandler<UploadTiilVaultsSetup, UploadResponse>
        {
            private readonly IHttpContextAccessor _accessor;
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            public UploadTiilVaultsSetupHandler(IHttpContextAccessor accessor, DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _dataContext = dataContext;
                _accessor = accessor;
                _serverRequest = serverRequest;
            }
            public async Task<UploadResponse> Handle(UploadTiilVaultsSetup request, CancellationToken cancellationToken)
            {
                var response = new UploadResponse
                {
                    File = new List<byte[]>(),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage()
                    }
                };
                try
                {
                    var files = _accessor.HttpContext.Request.Form.Files;
                    if (files.Count() < 1)
                    {
                        response.Status.Message.FriendlyMessage = "No file selected";
                        response.Status.IsSuccessful = false;
                        return response;
                    }

                    foreach (var fileBit in files)
                    {
                        if (fileBit.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                await fileBit.CopyToAsync(ms);
                                response.File.Add(ms.ToArray());
                            }
                        }

                    }
                    List<TillVaultSetupObj> uploadedRecord = new List<TillVaultSetupObj>();

                    foreach (var byteItem in response.File)
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;
                            int columns = workSheet.Dimension.Columns;
                            var data = new TillVaultSetupObj();
                            data.ExcelLine = 0;
                            for (int i = 2; i <= totalRows; i++)
                            {
                                data.ExcelLine = i;
                                data.CompanyName = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : string.Empty;
                                data.TellerTillIdPrefix = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : string.Empty;
                                data.StructureTillIdPrefix = workSheet.Cells[i, 3].Value != null ? workSheet.Cells[i, 3].Value.ToString() : string.Empty;
                                data.PresetChart = bool.Parse(workSheet.Cells[i, 4].Value.ToString());
                              
                                uploadedRecord.Add(data);
                            }
                        }
                    }
                    var structure = await _serverRequest.GetAllCompanyAsync();
                    var titles = await _serverRequest.GetAllJobTileAsync();
                    if (uploadedRecord.Count() > 0)
                    {
                        foreach (var record in uploadedRecord)
                        { 
                            var structureId = structure.companyStructures.FirstOrDefault(e => e.name == record.CompanyName)?.companyStructureId ?? 0;
                            var thisItem = _dataContext.deposit_tillvaultsetup.FirstOrDefault(g => g.Structure == structureId && g.Deleted == false);
                            if (thisItem != null)
                            { 
                                thisItem.StructureTillIdPrefix = record.StructureTillIdPrefix;
                                thisItem.TellerTillIdPrefix = record.TellerTillIdPrefix;
                                thisItem.PresetChart = record.PresetChart;
                            }
                            else
                            {
                                thisItem = new deposit_tillvaultsetup();
                                thisItem.Structure = structureId;
                                thisItem.StructureTillIdPrefix = record.StructureTillIdPrefix;
                                thisItem.TellerTillIdPrefix = record.TellerTillIdPrefix;
                                thisItem.PresetChart = record.PresetChart;
                                _dataContext.deposit_tillvaultsetup.Add(thisItem);
                            }
                            await _dataContext.SaveChangesAsync();
                        } 
                    } 
                }
                catch (Exception e)
                {
                    response.Status.Message.FriendlyMessage = e?.Message??e?.InnerException?.Message;
                    response.Status.IsSuccessful = false;
                    response.Status.Message.TechnicalMessage = e.ToString();
                    return response;
                }
                response.Status.Message.FriendlyMessage = "Successful";
                return response;
            }

        }
    }
}
