﻿using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Deposit.Contracts.Response.Deposit
{
    public class TransactionChargeObj
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public deposit_transactioncharge()
        //{
        //    deposit_accountsetup = new HashSet<deposit_accountsetup>();
        //    deposit_selectedTransactioncharge = new HashSet<deposit_selectedTransactioncharge>();
        //}

        public int TransactionChargeId { get; set; }

        public string Name { get; set; }

        public string FixedOrPercentage { get; set; }

        public decimal? Amount_Percentage { get; set; }

        public string Description { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<deposit_accountsetup> deposit_accountsetup { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<deposit_selectedTransactioncharge> deposit_selectedTransactioncharge { get; set; }
    }

    public class AddUpdateTransactionChargeObj
    {
        public int TransactionChargeId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string FixedOrPercentage { get; set; }

        [Column("Amount/Percentage")]
        public decimal? Amount_Percentage { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }

    public class TransactionChargeRegRespObj
    {
        public TransactionChargeRegRespObj()
        {
            Status = new APIResponseStatus { Message = new APIResponseMessage() };
        }
        public int TransactionChargeId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class TransactionChargeRespObj
    {
        public TransactionChargeRespObj()
        {
            TransactionCharges = new List<TransactionChargeObj>();
            Status = new APIResponseStatus { Message = new APIResponseMessage() };
        }
        public List<TransactionChargeObj> TransactionCharges { get; set; }
        public byte[] export { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}

