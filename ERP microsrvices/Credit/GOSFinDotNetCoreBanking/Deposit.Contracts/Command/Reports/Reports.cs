﻿using Finance.Contracts.Response.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Command.Reports
{
    public class GenerateOfferLetterQuery : IRequest<OfferLetterDetailObj>
    {
        public string ApplicationReference { get; set; }
    }
}
