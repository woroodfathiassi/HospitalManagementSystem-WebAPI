using HospitalManagementSystemPhase2.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystemPhase2.Managements
{
    public class PatientManagement
    {
        private readonly HMSDBContext _context;
        public PatientManagement(HMSDBContext context) 
        {
            _context = context;
        }

        public List<Patient> GetAllPatients()
        {
            return _context.Patients.AsNoTracking().ToList();
        }

        public Patient GetPatientById(int id)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == id);
            return patient;
        }

        public void AddNewPatient(Patient patient)
        {
            ValidatePatient(patient);

            _context.Patients.Add(patient);
            _context.SaveChanges();

            //await _context.Patients.AddAsync(patient);
            //await _context.SaveChangesAsync();
        }

        public void UpdatePatient(Patient pat)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == pat.Id);

            ValidatePatient(pat);

            patient.Name = pat.Name;
            patient.Age = pat.Age;
            patient.Gender = pat.Gender;
            patient.ContactNumber = pat.ContactNumber;
            patient.Address = pat.Address;

            //_context.Entry(patient).CurrentValues.SetValues(pat);

            _context.SaveChanges();
        }

        public void DeletePatient(int patientId)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == patientId);

            if (patient is null)
            {
                return;
            }

            _context.Patients.Remove(patient);
            _context.SaveChanges();
        }

        private void ValidatePatient(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient), "Patient data cannot be null.");
            }

            if (string.IsNullOrEmpty(patient.Name))
            {
                throw new ArgumentException("Patient name cannot be empty.");
            }

            if (patient.Age < 0 || patient.Age > 120)
            {
                throw new ArgumentException("Invalid age. Please enter a valid age between 0 and 120.");
            }

            if (string.IsNullOrEmpty(patient.Gender) ||
                !(string.Equals(patient.Gender, "Male", StringComparison.OrdinalIgnoreCase) ||
                  string.Equals(patient.Gender, "Female", StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Invalid gender. Gender must be 'Male' or 'Female'.");
            }

            if (string.IsNullOrEmpty(patient.ContactNumber) || patient.ContactNumber.Length != 10 || !patient.ContactNumber.All(char.IsDigit) || !patient.ContactNumber.StartsWith("05"))
            {
                throw new ArgumentException("Invalid phone number. It must be 10 digits long and start with '05'.");
            }

            if (string.IsNullOrEmpty(patient.Address))
            {
                throw new ArgumentException("Address cannot be empty.");
            }
        }


    }
}
