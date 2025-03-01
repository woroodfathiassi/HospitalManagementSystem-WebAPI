using Azure;
using HospitalManagementSystem.Entities;
using HospitalManagementSystemPhase2.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Managements
{
    public class BillingManagement
    {
        private readonly HMSDBContext _context;

        public BillingManagement(HMSDBContext context) 
        {
            _context = context;
        }

        public void AddNewBill(Bill bill)
        {
            _context.Bills.Add(bill);
            _context.SaveChanges();
        }

        public List<Bill> GetBills()
        {
            var bills = _context.Bills.AsNoTracking().ToList();
            return bills;
        }

        //public List<Bill> GetBillsByPatientId(int patientId)
        //{
        //    bool isPatient = _context.Patients.Any(p => p.Id == patientId);

        //    if (!isPatient)
        //    {
        //        throw new KeyNotFoundException($"There is no any patient with {patientId} ID!");
        //    }

        //    var bills = _context.Bills
        //                        .Include(b => b.Prescription) 
        //                        .Where(b => b.Prescription.PatientId == patientId)
        //                        .AsNoTracking()
        //                        .ToList();
        //    return bills;
        //}

        public List<BillDto> GetBillsByPatientId(int patientId)
        {
            bool isPatient = _context.Patients.Any(p => p.Id == patientId);

            if (!isPatient)
            {
                throw new KeyNotFoundException($"There is no any patient with {patientId} ID!");
            }

            var bills = _context.Bills
                                .Where(b => b.Prescription.PatientId == patientId)
                                .Select(b => new BillDto
                                {
                                    BillId = b.BillId,
                                    PrescriptionId = b.PrescriptionId
                                })
                                .ToList();

            return bills;
        }

        
        public void UpdateBillStatus(int id, int status)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.BillId == id);

            if (bill == null)
            {
                throw new KeyNotFoundException($"There is no any bill with {id} ID!");
            }

            bill.Status = (BillStatus)status;
            _context.SaveChanges();
        }
    }
}
