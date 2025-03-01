using HospitalManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Managements
{
    public class MedicationManagement
    {
        private readonly HMSDBContext _context;
        public MedicationManagement(HMSDBContext context)
        {
            _context = context;
        }

        public List<Medication> GetAllMedications()
        {
            return _context.Medications.AsNoTracking().ToList();
        }

        public Medication GetMedicationById(int id)
        {
            return _context.Medications.FirstOrDefault(p => p.MedicationId == id);
        }

        public void AddMedication(Medication medication)
        {
            ValidateMedication(medication);

            _context.Medications.Add(medication);
            _context.SaveChanges();
        }

        public void UpdateMedication(Medication med)
        {
            var medication = _context.Medications.FirstOrDefault(m => m.MedicationId == med.MedicationId);

            ValidateMedication(med);

            medication.Name = med.Name;
            medication.Quantity = med.Quantity;
            medication.Price = med.Price;

            _context.SaveChanges();
        }

        public void TrackMedicationInventory()
        {
            var medications = _context.Medications.ToList();
            foreach (var medication in medications)
            {
                Console.WriteLine($"Id: {medication.MedicationId}, Name: {medication.Name}, Quantity: {medication.Quantity}");
            }
        }

        public void DeleteMedication(int id) 
        {
            var med = GetMedicationById(id);

            _context.Medications.Remove(med);
            _context.SaveChanges();
        }

        private void ValidateMedication(Medication medication)
        {
            if (string.IsNullOrWhiteSpace(medication.Name))
            {
                throw new ArgumentException("Medication name is required.");
            }

            if (medication.Quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative.");
            }

            if (medication.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }
        }
    }
}
