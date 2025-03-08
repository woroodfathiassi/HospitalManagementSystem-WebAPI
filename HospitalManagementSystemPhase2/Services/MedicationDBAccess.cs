using HospitalManagementSystemPhase2.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemPhase2.Services
{
    public class MedicationDBAccess
    {
        private readonly HMSDBContext _context;

        public MedicationDBAccess(HMSDBContext context)
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
            _context.Medications.Add(medication);
            _context.SaveChanges();
        }

        public void UpdateMedication(Medication newMed, Medication currentMed)
        {
            currentMed.Name = newMed.Name;
            currentMed.Quantity = newMed.Quantity;
            currentMed.Price = newMed.Price;

            _context.SaveChanges();
        }

        public void DeleteMedication(Medication med)
        {
            _context.Medications.Remove(med);
            _context.SaveChanges();
        }

    }
}
