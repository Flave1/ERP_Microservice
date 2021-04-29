﻿using Deposit.Contracts.GeneralExtension;
using Deposit.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deposit.Handlers.Deposit.AccountSetup
{
	public class DeleteNextOfKinCommand : IRequest<Delete_response>
	{
		public List<int> ItemIds { get; set; }
		public class DeleteNextOfKinCommandHandler : IRequestHandler<DeleteNextOfKinCommand, Delete_response>
		{
			private readonly DataContext _dataContext;
			public DeleteNextOfKinCommandHandler(DataContext dataContext)
			{
				_dataContext = dataContext;
			}
			public async Task<Delete_response> Handle(DeleteNextOfKinCommand request, CancellationToken cancellationToken)
			{
				var resp = new Delete_response();
				try
				{
					if (request.ItemIds.Count() > 0)
					{
						foreach (var id in request.ItemIds)
						{
							var item = await _dataContext.deposit_nextofkin.FindAsync(id);
							if (item != null)
							{
								item.Deleted = true; 
								_dataContext.SaveChanges();
							}
						}
						resp.Deleted = true;
						resp.Status.Message.FriendlyMessage = "Successful";
						resp.Status.IsSuccessful = true;
						return resp;
					}
					else
					{ 
						resp.Status.Message.FriendlyMessage = "Please Select Item to delete";
						return resp;
					}
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}
	}
	 
}
