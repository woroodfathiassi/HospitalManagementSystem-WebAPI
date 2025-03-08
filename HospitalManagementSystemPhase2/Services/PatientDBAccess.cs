using HospitalManagementSystemPhase2.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemPhase2.Services
{
    public class PatientDBAccess
    {
        private readonly HMSDBContext _context;

        public PatientDBAccess(HMSDBContext context)
        {
            _context = context;
        }

        public List<Patient> GetAllPatients()
        {
            return _context.Patients.AsNoTracking().ToList();
        }

        public Patient GetPatientById(int id)
        {
            return _context.Patients.FirstOrDefault(p => p.Id == id);
        }

        public void AddNewPatient(Patient patient)
        {
            var user = new User { UserName = patient.Name, RoleId = 3 };
            _context.Users.Add(user);

            patient.User = user;
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        public void UpdatePatient(Patient pat, Patient patUpdated)
        {
            pat.Name = patUpdated.Name;
            pat.Age = patUpdated.Age;
            pat.Gender = patUpdated.Gender;
            pat.ContactNumber = patUpdated.ContactNumber;
            pat.Address = patUpdated.Address;

            _context.SaveChanges();
        }

        public void DeletePatient(Patient patient)
        {
            _context.Patients.Remove(patient);
            _context.SaveChanges();
        }
    }
}
