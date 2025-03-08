using HospitalManagementSystemPhase2.DTOs;
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
    public class AppointmentManagement
    {
        private readonly AppointmentDBAccess _context;

        public AppointmentManagement(AppointmentDBAccess context) 
        {
            
            _context = context;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _context.GetAllAppointments();
        }

        public Appointment GetAppointmentById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid ID.");

            return _context.GetAppointmentById(id);
        }

        public void ScheduleAppointment(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new ArgumentNullException("Appointment data is required.");
            }

            var patient = _context.GetPatientById(appointment.PatientId);

            if (patient == null)
            {
                throw new KeyNotFoundException($"There is no any patient with {appointment.PatientId} ID!");
            }

            var doctor = _context.GetDoctorById(appointment.DoctorId);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"There is no any doctor with {appointment.DoctorId} ID!");
            }

            bool isDoctorAvailiable = _context.isDoctorAvailiable(appointment);

            if (isDoctorAvailiable)
            {
                throw new InvalidOperationException($"Doctor {doctor.Name} is not available at {appointment.AppointmentDate}");
            }

            bool hasPatientSchedule = _context.hasPatientSchedule(appointment);

            if (hasPatientSchedule)
            {
                throw new InvalidOperationException($"Patient {patient.Name} is not available at {appointment.AppointmentDate}");
            }

            var newApp = new Appointment
            {
                AppointmentDate = appointment.AppointmentDate,
                Patient = patient,
                Doctor = doctor,
                Status = AppointmentStatus.Scheduled
            };
             
            _context.ScheduleAppointment(newApp);
        }
    
        public List<AppointmentDto> GetScheduleByPatientId(int patientId)
        {
            if (patientId <= 0)
            {
                throw new ArgumentException("Invalid appointment with patient ID.");
            }

            bool isPatient = _context.CheckPatientById(patientId);

            if (!isPatient)
            {
                throw new KeyNotFoundException($"There is no any patient with {patientId} ID!");
            }

            var appointments = _context.GetScheduleByPatientId(patientId);
            if (!appointments.Any())
            {
                 throw new KeyNotFoundException($"No appointments found for patient ID {patientId}.");
            }

            return appointments;
        }

        public List<AppointmentDto> GetScheduleByDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new ArgumentException($"Invalid appointment with doctor with ID {doctorId}.");
            }

            bool isDoctor = _context.CheckDoctorById(doctorId);

            if (!isDoctor)
            {
                throw new KeyNotFoundException($"There is no any doctor with ID {doctorId}!");
            }

            var appointments = _context.GetScheduleByDoctorId(doctorId);
            if (!appointments.Any())
            {
                throw new KeyNotFoundException($"No appointments found for doctor ID {doctorId}.");
            }

            return appointments;
        }

        public Doctor GetDoctorById(int docId)
        {
            if (docId <= 0)
                throw new ArgumentException("Invalid doctor ID.");

            var doc = _context.GetDoctorById(docId);
            if (doc == null)
                throw new KeyNotFoundException($"Doctor with ID {docId} not found.");
            return doc;
        }

        public Patient GetPatientById(int patId)
        {
            if (patId <= 0)
                throw new ArgumentException("Invalid doctor ID.");

            var pat = _context.GetPatientById(patId);
            if (pat == null)
                throw new KeyNotFoundException($"Patient with ID {patId} not found.");
            return pat;
        }

        public void CancelAppointment(int id)
        {
            _context.CancelAppointment(id);
        }

        //public void UpdateAppointment(int id, int patientId, int doctorId, DateTime datetime, int status)
        //{
        //    var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentId == id);

        //    if (appointment is null)
        //    {
        //        throw new KeyNotFoundException("Appointment not found.");
        //    }

        //    appointment.PatientId = patientId;
        //    appointment.DoctorId = doctorId;
        //    appointment.AppointmentDate = datetime;
        //    appointment.Status = (AppointmentStatus)status;

        //    _context.SaveChanges();
        //}

        public void UpdateAppointmentStatus(int id, int status)
        {
            if (id <= 0 || !new[] { 1, 2, 3 }.Contains(status))
            {
                throw new ArgumentException("Invalid inputs.");
            }

            _context.UpdateAppointmentStatus(id, status);
        }
    }
}
