using Deposit.Contracts.Response.Deposit;
using Deposit.Data;
using GODP.Entities.Models;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.TillVaults
{
    public class AddUpdateTillVaultCommand : IRequest<TillVaultSetupRegRespObj>
    {
        public int TillVaultSetupId { get; set; }

        public int? Structure { get; set; }

        public bool? PresetChart { get; set; } 
        public string StructureTillIdPrefix { get; set; } 
        public string TellerTillIdPrefix { get; set; }
        public class AddUpdateTillVaultCommandHandler : IRequestHandler<AddUpdateTillVaultCommand, TillVaultSetupRegRespObj>
        {
            private readonly ILoggerService _logger;
            private readonly DataContext _dataContext; 
            public AddUpdateTillVaultCommandHandler(ILoggerService logger, DataContext dataContext)
            {
                _dataContext = dataContext;
                _logger = logger;
            }
            public async Task<TillVaultSetupRegRespObj> Handle(AddUpdateTillVaultCommand request, CancellationToken cancellationToken)
            {
                var response = new TillVaultSetupRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
                try
                {
                    var domain = _dataContext.deposit_tillvaultsetup.Find(request.TillVaultSetupId);
                    if (domain == null)
                        domain = new deposit_tillvaultsetup();

                    domain.TillVaultSetupId = request.TillVaultSetupId;
                    domain.Structure = request.Structure;
                    domain.PresetChart = request.PresetChart;
                    domain.StructureTillIdPrefix = request.StructureTillIdPrefix;
                    domain.TellerTillIdPrefix = request.TellerTillIdPrefix;

                    if (domain.TillVaultSetupId > 0)
                        _dataContext.Entry(domain).CurrentValues.SetValues(domain);
                    else
                        _dataContext.deposit_tillvaultsetup.Add(domain);
                    await _dataContext.SaveChangesAsync();

                    response.TillVaultSetupId = domain.TillVaultSetupId;
                    response.Status.Message.FriendlyMessage = "successful";
                    return response;
                }
                catch (Exception e)
                {
                    response.Status.IsSuccessful = false;
                    response.Status.Message.FriendlyMessage = e?.Message ?? e.InnerException?.Message;
                    response.Status.Message.TechnicalMessage = e.ToString();
                    return response;
                }
            }
        }
    }
   
}
