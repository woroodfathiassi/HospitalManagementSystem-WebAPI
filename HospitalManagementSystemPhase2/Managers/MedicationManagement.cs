using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystemPhase2.Managements
{
    public class MedicationManagement
    {
        private readonly MedicationDBAccess _context;
        public MedicationManagement(MedicationDBAccess context)
        {
            _context = context;
        }

        public List<Medication> GetAllMedications()
        {
            return _context.GetAllMedications();
        }

        public Medication GetMedicationById(int id)
        {
            return _context.GetMedicationById(id);
        }

        public void AddMedication(Medication medication)
        {
            if (medication == null)
            {
                throw new ArgumentNullException("Medication data is required.");
            }

            ValidateMedication(medication);

            _context.AddMedication(medication);
        }

        public void UpdateMedication(int id, Medication newMed)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID.");
            }

            if (newMed == null)
            {
                throw new ArgumentNullException("Medication data is required.");
            }

            if (id != newMed.MedicationId)
            {
                throw new ArgumentException("Invalid Ids.");
            }

            var currentMed = _context.GetMedicationById(newMed.MedicationId);
            if (currentMed == null)
            {
                throw new KeyNotFoundException($"Medication with ID {newMed.MedicationId} not found.");
            }

            ValidateMedication(newMed);

            _context.UpdateMedication(newMed, currentMed);
        }

        public void DeleteMedication(int id) 
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID.");
            }

            var medication = _context.GetMedicationById(id);
            if (medication == null)
            {
                throw new KeyNotFoundException($"Medication with ID {medication.MedicationId} not found.");
            }

            _context.DeleteMedication(medication);
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
