using Deposit.Contracts.Response.Common;
using Deposit.Contracts.Response.Deposit;
using Deposit.Requests;
using Deposit.Data;
using GOSLibraries.GOS_API_Response;
using MediatR;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.BankClosure
{
    public class DownloadBankClosureCommand : IRequest<DownloadResponse>
    {
        public class DownloadBankClosureHandler : IRequestHandler<DownloadBankClosureCommand, DownloadResponse>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            public DownloadBankClosureHandler(DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _serverRequest = serverRequest;
                _dataContext = dataContext;
            }
            public async Task<DownloadResponse> Handle(DownloadBankClosureCommand request, CancellationToken cancellationToken)
            {
                var response = new DownloadResponse
                {
                    ExcelFile = new byte[0],
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                        Message = new APIResponseMessage()
                    }
                };
                try
                {
                    var comp = await _serverRequest.GetAllCompanyAsync();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Company");
                    dt.Columns.Add("Product");
                    dt.Columns.Add("Charge");
                    dt.Columns.Add("Percentage");

                    var result = _dataContext.deposit_bankclosuresetup.Where(d => d.Deleted == false)
                        .Select( a =>  new Deposit_bankClosureSetupObjs
                        {
                            ProductId = a.ProductId,
                            Charge = a.Charge,
                            Structure = a.Structure,
                            Percentage = a.Percentage
                        }).ToList();

                    
                    foreach (var data in result)
                    {
                        var row = dt.NewRow();
                        row["Company"] = comp.companyStructures.FirstOrDefault(e => e.companyStructureId == data.Structure)?.name;
                        row["Product"] = _dataContext.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == data.ProductId)?.AccountName;
                        row["Charge"] = data.Charge;
                        row["Percentage"] = data.Percentage;
                        dt.Rows.Add(row);
                    } 

                    if (result.Count() > 0)
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage pck = new ExcelPackage())
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Bank Closure");
                            ws.DefaultColWidth = 20;
                            ws.Cells["A1"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.None);
                            response.ExcelFile = pck.GetAsByteArray();
                        }
                    }
                    return response;
                }
                catch (Exception e)
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = e?.Message ?? e?.InnerException?.Message;
                    response.Status.Message.FriendlyMessage = e.ToString();
                    return response;
                };
            }
        }
    }

}
