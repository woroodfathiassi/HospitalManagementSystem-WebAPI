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
    public class DoctorManagement
    {
        private readonly DoctorDBAccess _doctorDBAccess;
        public DoctorManagement(DoctorDBAccess doctorDBAccess)
        {
            _doctorDBAccess = doctorDBAccess;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctorDBAccess.GetAllDoctors();
        }

        public Doctor GetDoctorById(int docId)
        {
            if (docId <= 0)
                throw new ArgumentException("Invalid doctor ID.");

            var doc = _doctorDBAccess.GetDoctorById(docId);
            if(doc == null)
                throw new KeyNotFoundException($"Doctor with ID {docId} not found.");
            return doc; 
        }


        public void AddNewDoctor(Doctor doctor)
        {
            if(doctor == null)
                throw new ArgumentNullException("Doctor data is required.");

            _doctorDBAccess.AddNewDoctor(doctor);
        }

        public void UpdateDoctor(Doctor docUpdated)
        {
            if(docUpdated == null)
            {
                throw new ArgumentNullException("Doctor data is required.");
            }

            var doctor = _doctorDBAccess.GetDoctorById(docUpdated.Id);
            if (doctor == null)
                throw new KeyNotFoundException($"Doctor with ID {docUpdated.Id} not found.");

            ValidateDoctor(docUpdated);

            _doctorDBAccess.UpdateDoctor(doctor, docUpdated);
        }

        public void DeleteDoctor(int doctorId)
        {
            var doctor = _doctorDBAccess.GetDoctorById(doctorId);

            if (doctor is null)
                throw new ArgumentException($"Doctor with ID {doctorId} not found.");

            _doctorDBAccess.DeleteDoctor(doctor);
        }

        private void ValidateDoctor(Doctor doctor)
        {
            if (string.IsNullOrEmpty(doctor.Name))
            {
                throw new ArgumentNullException("Doctor name cannot be empty.");
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
