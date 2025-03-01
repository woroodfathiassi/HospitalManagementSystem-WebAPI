using Azure;
using HospitalManagementSystem.Entities;
using HospitalManagementSystemPhase2.MyExceptions;
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
    public class PrescriptionManagement
    {
        private readonly HMSDBContext _context;
        private BillingManagement _billingManagement;
        public PrescriptionManagement(HMSDBContext context, BillingManagement billingManagement) 
        {
            _context = context;
            _billingManagement = billingManagement;
        }

        public List<Prescription> GetAllPrescriptions()
        {
            return _context.Prescriptions.AsNoTracking().ToList();
        }

        public Prescription GetPrescriptionById(int id)
        {
            return _context.Prescriptions.FirstOrDefault(p => p.PrescriptionId == id);
            
        }

        public void IssuePrescription(Prescription pre)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == pre.PatientId);

            if (patient == null)
            {
                throw new KeyNotFoundException($"There is no any patient with {pre.PatientId} ID!");
            }

            var doctor = _context.Doctors.FirstOrDefault(p => p.Id == pre.DoctorId);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"There is no any doctor with {pre.DoctorId} ID!");
            }

            var medicationIds = pre.Medications;
            var totalPrice = 0m;
            var medicationAvaliables = new List<Medication>();
            for (int i = 0; i < medicationIds.Count; i++) 
            { 
                var medication = _context.Medications.FirstOrDefault(p => p.MedicationId == medicationIds[i].MedicationId);

                if (medication == null)
                {
                    throw new KeyNotFoundException($"There is no any medication with {medicationIds[i]} ID!");
                }
                else
                {
                    if(medication.Quantity <= 0)
                    {
                        throw new MedicationOutOfStockException(medicationIds[i].MedicationId);
                    }
                    else
                    {
                        totalPrice += medication.Price;
                        medication.Quantity --;
                        medicationAvaliables.Add(medication);
                    }
                }
                
            }

            var prescription = new Prescription
            {
                PrescriptionDate = DateTime.Now,
                DoctorId = pre.PatientId,
                PatientId = pre.DoctorId,
                Medications = medicationAvaliables
            };

            _context.Prescriptions.Add(prescription);
          
            _context.SaveChanges();

            int insertedPrescriptionId = prescription.PrescriptionId; 

            _billingManagement.AddNewBill(new Bill
            {
                PrescriptionId = insertedPrescriptionId,
                Amount = totalPrice,
                BillDate = DateTime.Now,
                Status = BillStatus.Unpaid
            });
        }

        public void ViewPrescriptions()
        {
            var prescriptions = _context.Prescriptions.AsNoTracking().ToList();
            foreach (var prescription in prescriptions)
            {
                Console.WriteLine(prescription.ToString());
            }
        }

        public void UpdatePrescription(Prescription pre)
        {
            var prescription = _context.Prescriptions.FirstOrDefault(p => p.PrescriptionId == pre.PrescriptionId);

            if (prescription is null)
            {
                throw new KeyNotFoundException($"Prescription with {pre.PrescriptionId} ID not found.");
            }

            prescription.PatientId = pre.PatientId;
            prescription.DoctorId = pre.DoctorId;

            var totalPrice = 0m;
            var medicationAvaliables = new List<Medication>();
            for (int i = 0; i < pre.Medications.Count; i++)
            {
                var medication = _context.Medications.FirstOrDefault(p => p.MedicationId == pre.Medications[i].MedicationId);

                if (medication == null)
                {
                    throw new KeyNotFoundException($"There is no any medication with {pre.Medications[i].MedicationId} ID!");
                }
                else
                {
                    if (medication.Quantity <= 0)
                    {
                        throw new MedicationOutOfStockException(pre.Medications[i].MedicationId);
                    }
                    else
                    {
                        totalPrice += medication.Price;
                        medication.Quantity--;
                        medicationAvaliables.Add(medication);
                    }
                }

            }

            prescription.Medications = medicationAvaliables;

            var bill = _context.Bills.FirstOrDefault(p => p.PrescriptionId == pre.PrescriptionId);
            bill.Amount = totalPrice;
            
            _context.SaveChanges();
        }

        //public void DeletePrescription(int preId)
        //{
        //    var pre = _context.Prescriptions.FirstOrDefault(p => p.PrescriptionId == preId);

        //    if (pre is null)
        //    {
        //        Console.WriteLine("Prescription not found.");
        //        return;
        //    }

        //    _context.Prescriptions.Remove(pre);
        //    _context.SaveChanges();
        //    Console.WriteLine("Prescription deleted successfully.");
        //}
    }
}
