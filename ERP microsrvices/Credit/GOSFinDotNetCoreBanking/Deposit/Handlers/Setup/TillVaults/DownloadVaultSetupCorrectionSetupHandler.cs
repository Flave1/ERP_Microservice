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

namespace Deposit.Handlers.Deposit.VaultSetup
{
    public class DownloadVaultSetupCommand : IRequest<DownloadResponse>
    {
        public class DownloadVaultSetupHandler : IRequestHandler<DownloadVaultSetupCommand, DownloadResponse>
        {
            private readonly DataContext _dataContext;
            private readonly IIdentityServerRequest _serverRequest;
            public DownloadVaultSetupHandler(DataContext dataContext, IIdentityServerRequest serverRequest)
            {
                _serverRequest = serverRequest;
                _dataContext = dataContext;
            }
            public async Task<DownloadResponse> Handle(DownloadVaultSetupCommand request, CancellationToken cancellationToken)
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
                    var titles = await _serverRequest.GetAllJobTileAsync();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Company"); 
                    dt.Columns.Add("Teller TillId Prefix");
                    dt.Columns.Add("Structure TillId Prefix");
                    dt.Columns.Add("Use preset Chart");

                    var result = _dataContext.deposit_tillvaultsetup.Where(d => d.Deleted == false)
                        .Select( a =>  new TillVaultSetupObj
                        { 
                            Structure = a.Structure,
                            PresetChart = a.PresetChart, 
                            StructureTillIdPrefix = a.StructureTillIdPrefix,
                            TellerTillIdPrefix = a.TellerTillIdPrefix
                        }).ToList();

                    
                    foreach(var data in result)
                    {
                        var row = dt.NewRow();
                        row["Company"] = comp.companyStructures.FirstOrDefault(e => e.companyStructureId == data.Structure)?.name; 
                        row["Teller TillId Prefix"] = data.TellerTillIdPrefix;
                        row["Structure TillId Prefix"] = data.StructureTillIdPrefix;
                        row["Use preset Chart"] = data.PresetChart;
                        dt.Rows.Add(row);
                    } 

                    if (result.Count() > 0)
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage pck = new ExcelPackage())
                        {
                            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Till Vaults setup");
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
