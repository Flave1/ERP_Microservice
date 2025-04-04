﻿using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Deposit.Contracts.Response.Finance
{
    public partial class TransactionObj
    {
        public int TransactionId { get; set; }

        public string BatchCode { get; set; }

        public string ReferenceNo { get; set; }

        public int SubGLId { get; set; }

        public int OperationId { get; set; }

        public int? CasaAccountId { get; set; }

        public decimal DebitAmount { get; set; }

        public decimal CreditAmount { get; set; }

        public decimal? RunningBalance { get; set; }

        public string Description { get; set; }
        public DateTime ValueDate { get; set; }

        public DateTime PostedDate { get; set; }

        public int CurrencyId { get; set; }

        public bool IsApproved { get; set; }

        public int? CompanyId { get; set; }

        public string PostedBy { get; set; }

        public string ApprovedBy { get; set; }

        public DateTime ApprovedDate { get; set; }
        public DateTime ApprovedDateTime { get; set; }
        public int SourceApplicationId { get; set; }
        public int GlAccountId { get; set; }
        public string SourceReferenceNumber { get; set; }

        public string JournalType { get; set; }
        public DateTime TransactionDate { get; set; }
        public double CurrencyRate { get; set; }
        public string CasaAccountNumber { get; set; }
        public int RateType { get; set; }
        public decimal Amount { get; set; }
        public int DebitGL { get; set; }
        public int CreditGL { get; set; }

    }

    public enum TransactionType
    {
        Credit = 1,
        Debit
    }

    public class FinTransacRegRespObj
    {
        public int TransactionId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class SecretKeysRespObj
    {
        public FlutterKeys keys { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class FlutterKeys
    {
        public int flutterWaveKeysId { get; set; }
        public string secret_keys { get; set; }
        public string public_keys { get; set; }
        public bool useFlutterWave { get; set; }
    }
}
