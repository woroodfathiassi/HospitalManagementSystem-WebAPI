using Azure;
using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.MyExceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystemPhase2.Managements
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

        public List<Prescription> GetPatientPrescriptiond(int id)
        {
            return _context.Prescriptions.Where(p => p.PatientId == id).AsNoTracking().ToList();
        }

        public Patient GetPatientById(int id)
        {
            return _context.Patients.FirstOrDefault(p => p.Id == id);
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
                DoctorId = pre.DoctorId,
                PatientId = pre.PatientId,
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
            var prescription = _context.Prescriptions
            .Include(p => p.Medications)
            .FirstOrDefault(p => p.PrescriptionId == pre.PrescriptionId);

            if (prescription is null)
            {
                throw new KeyNotFoundException($"Prescription with {pre.PrescriptionId} ID not found.");
            }

            prescription.PatientId = pre.PatientId;
            prescription.DoctorId = pre.DoctorId;

            var totalPrice = 0m;

            var existingMedicationIds = prescription.Medications.Select(m => m.MedicationId).ToList();
            var newMedications = new List<Medication>();
            foreach (var med in pre.Medications)
            {
                var medication = _context.Medications.FirstOrDefault(p => p.MedicationId == med.MedicationId);


                if (medication == null)
                {
                    //throw new KeyNotFoundException($"There is no any medication with {pre.Medications[i].MedicationId} ID!");
                }
                else
                {
                    if (medication.Quantity <= 0)
                    {
                        //throw new MedicationOutOfStockException(pre.Medications[i].MedicationId);
                    }

                    if (existingMedicationIds.Contains(med.MedicationId))
                    {
                        totalPrice += medication.Price;
                        newMedications.Add(medication);
                    }
                    else if(!existingMedicationIds.Contains(med.MedicationId))
                    {
                        totalPrice += medication.Price;
                        if (medication.Quantity > 0)
                            medication.Quantity--;
                        newMedications.Add(medication);
                    }
                }
            }

            prescription.Medications = newMedications;

            var bill = _context.Bills.FirstOrDefault(p => p.PrescriptionId == pre.PrescriptionId);
            bill.Amount = totalPrice;
            
            _context.SaveChanges();
        }
    }
}
