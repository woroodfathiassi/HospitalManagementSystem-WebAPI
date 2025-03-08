using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystemPhase2.Managements
{
    public class PatientManagement
    {
        private readonly PatientDBAccess _patientDBAccess;
        public PatientManagement(PatientDBAccess patientDBAccess) 
        {
            _patientDBAccess = patientDBAccess;
        }

        public List<Patient> GetAllPatients()
        {
            return _patientDBAccess.GetAllPatients();
        }

        public Patient GetPatientById(int patId)
        {
            if (patId <= 0)
                throw new ArgumentException("Invalid doctor ID.");

            var pat = _patientDBAccess.GetPatientById(patId);
            if (pat == null)
                throw new KeyNotFoundException($"Patient with ID {patId} not found.");
            return pat;
        }

        public void AddNewPatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentNullException("Patient data is required.");

            _patientDBAccess.AddNewPatient(patient);
        }

        public void UpdatePatient(Patient patUpdated)
        {
            if (patUpdated == null)
            {
                throw new ArgumentNullException("Patient data is required.");
            }

            var patient = _patientDBAccess.GetPatientById(patUpdated.Id);
            if (patient == null)
                throw new KeyNotFoundException($"Patient with ID {patUpdated.Id} not found.");

            ValidatePatient(patUpdated);

            _patientDBAccess.UpdatePatient(patient, patUpdated);
        }

        public void DeletePatient(int patientId)
        {
            var patient = _patientDBAccess.GetPatientById(patientId);

            if (patient is null)
                throw new ArgumentException($"Patient with ID {patientId} not found.");

            _patientDBAccess.DeletePatient(patient);
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
