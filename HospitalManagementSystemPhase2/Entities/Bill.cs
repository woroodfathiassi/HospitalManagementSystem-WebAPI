﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystemPhase2.Entities
{
    public enum BillStatus
    {
        Paid = 1, 
        Unpaid = 2
    }

    public class Bill
    {
        public int BillId { get; set; }

        public decimal Amount { get; set; }

        public DateTime BillDate { get; set; }

        public BillStatus Status { get; set; }

        public int PrescriptionId { get; set; }
        public Prescription? Prescription { get; set; }

        public override string ToString()
        {
            return $"Bill ID: {BillId}, Amount: {Amount:C}, Bill Date: {BillDate}, Status: {Status}, Prescription ID: {PrescriptionId}";
        }
    }
}
