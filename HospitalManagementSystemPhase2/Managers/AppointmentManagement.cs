using HospitalManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Managements
{
    public class AppointmentManagement
    {
        private readonly HMSDBContext _context;

        public AppointmentManagement(HMSDBContext context) 
        {
            
            _context = context;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments.AsNoTracking().ToList();
        }

        public Appointment GetAppointmentById(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(p => p.AppointmentId == id);
            return appointment;
        }

        public void ScheduleAppointment(Appointment appointment)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.Id == appointment.PatientId);

            if (patient == null)
            {
                throw new KeyNotFoundException($"There is no any patient with {appointment.PatientId} ID!");
            }

            var doctor = _context.Doctors.FirstOrDefault(p => p.Id == appointment.DoctorId);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"There is no any doctor with {appointment.DoctorId} ID!");
            }

            bool isDoctorAvailiable = _context.Appointments.Any(d => d.DoctorId == appointment.DoctorId && d.AppointmentDate.Equals(appointment.AppointmentDate));

            if (isDoctorAvailiable)
            {
                throw new InvalidOperationException($"Doctor {doctor.Name} is not available at {appointment.AppointmentDate}");
            }

            bool hasPatientSchedule = _context.Appointments.Any(d => d.PatientId == appointment.PatientId && d.AppointmentDate.Equals(appointment.AppointmentDate));

            if (hasPatientSchedule)
            {
                throw new InvalidOperationException($"Patient {patient.Name} is not available at {appointment.AppointmentDate}");
            }

            _context.Appointments.Add(new Entities.Appointment 
            { 
                AppointmentDate = appointment.AppointmentDate, 
                Patient = patient, 
                Doctor = doctor, 
                Status = AppointmentStatus.Scheduled 
            });
            _context.SaveChanges();
        }
    
        public List<Appointment> GetScheduleByPatientId(int patientId)
        {
            bool isPatient = _context.Appointments.Any(p => p.PatientId == patientId);

            if (!isPatient)
            {
                throw new KeyNotFoundException($"There is no any patient with {patientId} ID!");
            }

            var appointments = _context.Appointments.Include(p => p.Patient).Where(p => p.PatientId == patientId).AsNoTracking().ToList();

            return appointments;
        }

        public List<Appointment> GetScheduleByDoctorId(int doctorId)
        {
            bool isDoctor = _context.Appointments.Any(p => p.DoctorId == doctorId);

            if (!isDoctor)
            {
                throw new KeyNotFoundException($"There is no any doctor with {doctorId} ID!");
            }

            var appointments = _context.Appointments.Include(p => p.Doctor).Where(p => p.DoctorId == doctorId).AsNoTracking().ToList();

            return appointments;
        }

        public void CancelAppointment(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(p => p.AppointmentId == id);

            if (appointment == null) 
            {
                throw new KeyNotFoundException($"There is no any appointment with {id} ID!");
            }

            appointment.Status = AppointmentStatus.Canceled;
            _context.SaveChanges();
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
            var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentId == id);

            if (appointment is null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            appointment.Status = (AppointmentStatus)status;

            _context.SaveChanges();
        }
    }
}
