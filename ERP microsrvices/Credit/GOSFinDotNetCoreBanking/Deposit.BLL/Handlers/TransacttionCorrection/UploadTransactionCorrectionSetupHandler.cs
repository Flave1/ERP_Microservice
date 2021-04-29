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

    public class UploadTransactionCorrectionSetup : IRequest<UploadResponse>
    {
        public class UploadTransactionCorrectionSetupHandler : IRequestHandler<UploadTransactionCorrectionSetup, UploadResponse>
        {
            private readonly IHttpContextAccessor _accessor;
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            public UploadTransactionCorrectionSetupHandler(IHttpContextAccessor accessor, DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _dataContext = dataContext;
                _accessor = accessor;
                _serverRequest = serverRequest;
            }
            public async Task<UploadResponse> Handle(UploadTransactionCorrectionSetup request, CancellationToken cancellationToken)
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
                    List<Contracts.Response.Deposit.TransactionCorrectionSetup> uploadedRecord = new List<Contracts.Response.Deposit.TransactionCorrectionSetup>();

                    var data = new Contracts.Response.Deposit.TransactionCorrectionSetup();
                    foreach (var byteItem in response.File)
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;
                            int columns = workSheet.Dimension.Columns;
                            data.ExcelLine = 0;
                            for (int i = 2; i <= totalRows; i++)
                            {
                                data.ExcelLine = i;
                                data.CompanyName = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : string.Empty;  
                                data.PresetChart = bool.Parse(workSheet.Cells[i, 2].Value.ToString());
                                data.JobTitleName = workSheet.Cells[i, 3].Value != null ? workSheet.Cells[i, 3].Value.ToString() : string.Empty;
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
                            var jobId = titles.commonLookups.FirstOrDefault(w => w.LookupName == record.JobTitleName)?.LookupId??0;
                            var structureId = structure.companyStructures.FirstOrDefault(e => e.name == record.CompanyName)?.companyStructureId ?? 0;
                            var thisItem = _dataContext.deposit_transactioncorrectionsetup.FirstOrDefault(g => g.Structure == structureId && g.JobTitleId == jobId && g.Deleted == false);
                            if (thisItem != null)
                            { 
                                thisItem.JobTitleId = (int)jobId;
                                thisItem.PresetChart = record.PresetChart;
                            }
                            else
                            {
                                thisItem = new deposit_transactioncorrectionsetup();
                                thisItem.Structure = structureId;
                                thisItem.JobTitleId = (int)jobId;
                                thisItem.PresetChart = record.PresetChart; 
                                _dataContext.deposit_transactioncorrectionsetup.Add(thisItem);
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
