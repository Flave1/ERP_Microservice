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

    public class UploadBankClosureSetup : IRequest<UploadResponse>
    {
        public class UploadBankClosureSetupHandler : IRequestHandler<UploadBankClosureSetup, UploadResponse>
        {
            private readonly IHttpContextAccessor _accessor;
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            public UploadBankClosureSetupHandler(IHttpContextAccessor accessor, DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _dataContext = dataContext;
                _accessor = accessor;
                _serverRequest = serverRequest;
            }
            public async Task<UploadResponse> Handle(UploadBankClosureSetup request, CancellationToken cancellationToken)
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
                    if(files.Count() < 1)
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
                    List<Deposit_bankClosureSetupObjs> uploadedRecord = new List<Deposit_bankClosureSetupObjs>();

                    foreach (var byteItem in response.File)
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (MemoryStream stream = new MemoryStream(byteItem))
                        using (ExcelPackage excelPackage = new ExcelPackage(stream))
                        {
                            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];
                            int totalRows = workSheet.Dimension.Rows;
                            int columns = workSheet.Dimension.Columns; 
                            var data = new Deposit_bankClosureSetupObjs();
                            data.ExcelLine = 0;
                            for (int i = 2; i <= totalRows; i++)
                            {
                                data.ExcelLine = i;
                                data.CompanyName = workSheet.Cells[i, 1].Value != null ? workSheet.Cells[i, 1].Value.ToString() : string.Empty;
                                data.ProductName = workSheet.Cells[i, 2].Value != null ? workSheet.Cells[i, 2].Value.ToString() : string.Empty;
                                data.Charge = workSheet.Cells[i, 3].Value != null ? workSheet.Cells[i, 3].Value.ToString() : string.Empty;
                                data.Percentage = workSheet.Cells[i, 4].Value != null ? double.Parse(workSheet.Cells[i, 4].Value.ToString()) : new double(); 
                                uploadedRecord.Add(data);
                            }
                        }
                    }
                    var structure = await _serverRequest.GetAllCompanyAsync();
                    if(uploadedRecord.Count() > 0)
                    {
                        foreach(var record in uploadedRecord)
                        {
                            var productId = _dataContext.deposit_accountsetup.FirstOrDefault(x => x.AccountName == record.ProductName)?.DepositAccountId ?? 0;
                            var structureId = structure.companyStructures.FirstOrDefault(e => e.name == record.CompanyName)?.companyStructureId ?? 0;
                            var thisItem = _dataContext.deposit_bankclosuresetup.FirstOrDefault(g => g.ProductId == productId && g.Structure == structureId && g.Deleted == false);
                            if(thisItem != null)
                            {
                                thisItem.Charge = record.Charge;
                                thisItem.Percentage = record.Percentage;
                            }
                            else
                            {
                                thisItem = new deposit_bankclosuresetup();
                                thisItem.Structure = structureId;
                                thisItem.ProductId = productId;
                                thisItem.Charge = record.Charge;
                                thisItem.Percentage = record.Percentage;
                                _dataContext.deposit_bankclosuresetup.Add(thisItem);
                            }
                            await _dataContext.SaveChangesAsync(); 
                        }
                       
                    } 
                }
                catch (Exception e)
                {
                    response.Status.Message.FriendlyMessage = e?.Message ?? e?.InnerException?.Message;
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
