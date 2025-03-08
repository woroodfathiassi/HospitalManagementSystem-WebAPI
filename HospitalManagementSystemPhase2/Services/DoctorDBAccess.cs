using HospitalManagementSystemPhase2.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemPhase2.Services
{
    public class DoctorDBAccess
    {
        private readonly HMSDBContext _context;

        public DoctorDBAccess(HMSDBContext context)
        {
            _context = context;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _context.Doctors.AsNoTracking().ToList();
        }

        public Doctor GetDoctorById(int id)
        {
            return _context.Doctors.FirstOrDefault(p => p.Id == id);
        }

        public void AddNewDoctor(Doctor doctor)
        {
            var user = new User { UserName = doctor.Name, RoleId = 2 };
            _context.Users.Add(user);

            doctor.User = user;
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }

        public void UpdateDoctor(Doctor doc, Doctor docUpdated)
        {
            doc.Name = docUpdated.Name;
            doc.Age = docUpdated.Age;
            doc.Gender = docUpdated.Gender;
            doc.ContactNumber = docUpdated.ContactNumber;
            doc.Address = docUpdated.Address;
            doc.Email = docUpdated.Email;
            doc.Specialty = docUpdated.Specialty;

            _context.SaveChanges();
        }

        public void DeleteDoctor(Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
            _context.SaveChanges();
        }
    }
}
