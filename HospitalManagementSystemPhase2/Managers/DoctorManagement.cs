using HospitalManagementSystemPhase2.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystemPhase2.Managements
{
    public class DoctorManagement
    {
        private readonly HMSDBContext _context;
        public DoctorManagement(HMSDBContext context)
        {
            _context = context;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _context.Doctors.AsNoTracking().ToList();
        }

        public Patient GetDoctorById(int id)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == id);
            return patient;
        }

        public void AddNewDoctor(Doctor doctor)
        {
            ValidateDoctor(doctor);

            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }

        public void UpdateDoctor(Doctor doc)
        {
            var doctor = _context.Doctors.FirstOrDefault(p => p.Id == doc.Id);

            ValidateDoctor(doc);

            doctor.Name = doc.Name;
            doctor.Age = doc.Age;
            doctor.Gender = doc.Gender;
            doctor.ContactNumber = doc.ContactNumber;
            doctor.Address = doc.Address;
            doctor.Email = doc.Email;
            doctor.Specialty = doc.Specialty;

            _context.SaveChanges();
        }

        public void DeleteDoctor(int doctorId)
        {
            var doctor = _context.Doctors.FirstOrDefault(p => p.Id == doctorId);

            if (doctor is null)
            {
                return;
            }

            _context.Doctors.Remove(doctor);
            _context.SaveChanges();
        }

        private void ValidateDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Patient data cannot be null.");
            }

            if (string.IsNullOrEmpty(doctor.Name))
            {
                throw new ArgumentException("Patient name cannot be empty.");
            }

            if (doctor.Age < 0 || doctor.Age > 120)
            {
                throw new ArgumentException("Invalid age. Please enter a valid age between 0 and 120.");
            }

            if (string.IsNullOrEmpty(doctor.Gender) ||
                !(string.Equals(doctor.Gender, "Male", StringComparison.OrdinalIgnoreCase) ||
                  string.Equals(doctor.Gender, "Female", StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Invalid gender. Gender must be 'Male' or 'Female'.");
            }

            if (string.IsNullOrEmpty(doctor.ContactNumber) || doctor.ContactNumber.Length != 10 || !doctor.ContactNumber.All(char.IsDigit) || !doctor.ContactNumber.StartsWith("05"))
            {
                throw new ArgumentException("Invalid phone number. It must be 10 digits long and start with '05'.");
            }

            if (string.IsNullOrEmpty(doctor.Address))
            {
                throw new ArgumentException("Address cannot be empty.");
            }

            if (string.IsNullOrEmpty(doctor.Email) || !doctor.Email.Contains("@"))
            {
                throw new ArgumentException("Invalid email. Please enter a valid email.");
            }

            if (string.IsNullOrEmpty(doctor.Specialty))
            {
                throw new ArgumentException("Specialty cannot be empty. Please enter a valid specialty.");
            }
        }

    }
}
