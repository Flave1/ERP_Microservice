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

namespace Deposit.Handlers.Deposit.AccountReactivationSetup
{
    public class DownloadAccountReactivationSetupCommand : IRequest<DownloadResponse>
    {
        public class DownloadAccountReactivationSetupHandler : IRequestHandler<DownloadAccountReactivationSetupCommand, DownloadResponse>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            public DownloadAccountReactivationSetupHandler(DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _serverRequest = serverRequest;
                _dataContext = dataContext;
            }
            public async Task<DownloadResponse> Handle(DownloadAccountReactivationSetupCommand request, CancellationToken cancellationToken)
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
                    dt.Columns.Add("Charge type");
                    dt.Columns.Add("Use preset chart");

                    var result = _dataContext.deposit_accountreactivationsetup.Where(d => d.Deleted == false)
                        .Select( a =>  new AccountReactivationSetupObj
                        {
                            Product = a.Product,
                            Charge = a.Charge,
                            Structure = a.Structure,
                            ChargeType = a.ChargeType,
                            PresetChart = a.PresetChart
                        }).ToList();

                    
                    foreach(var data in result)
                    {
                        var row = dt.NewRow();
                        row["Company"] = comp.companyStructures.FirstOrDefault(e => e.companyStructureId == data.Structure)?.name;
                        row["Product"] = _dataContext.deposit_accountsetup.FirstOrDefault(x => x.DepositAccountId == data.Product)?.AccountName;
                        row["Charge"] = data.Charge;
                        row["Charge type"] = data.ChargeType;
                        row["Use preset chart"] = data.PresetChart;
                        dt.Rows.Add(row);
                    } 

                    if (result.Count() > 0)
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage pck = new ExcelPackage())
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Account Reactivation setup");
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
