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

namespace Deposit.Handlers.TransactionCorrectionSetups
{
    public class AddUpdateTransactionCorrectionSetupCommandHandler : IRequestHandler<AddUpdateTransactionCorrectionSetupCommand, TransactionCorrectionSetupRegResp>
    {
        private readonly ILoggerService _logger;
        private readonly DataContext _dataContext;
        public AddUpdateTransactionCorrectionSetupCommandHandler(ILoggerService logger, DataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }
        public async Task<TransactionCorrectionSetupRegResp> Handle(AddUpdateTransactionCorrectionSetupCommand request, CancellationToken cancellationToken)
        {
            var response = new TransactionCorrectionSetupRegResp { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                var domain = _dataContext.deposit_transactioncorrectionsetup.Find(request.TransactionCorrectionSetupId);
                if (domain == null)
                    domain = new deposit_transactioncorrectionsetup();

                domain.TransactionCorrectionSetupId = request.TransactionCorrectionSetupId;
                domain.Structure = request.Structure;
                domain.PresetChart = request.PresetChart;
                domain.JobTitleId = request.JobTitleId; 

                if (domain.TransactionCorrectionSetupId > 0)
                    _dataContext.Entry(domain).CurrentValues.SetValues(domain);
                else
                    _dataContext.deposit_transactioncorrectionsetup.Add(domain);
                await _dataContext.SaveChangesAsync();

                response.TransactionCorrectionSetupId = domain.TransactionCorrectionSetupId;
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
