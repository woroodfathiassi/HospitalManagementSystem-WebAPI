using HospitalManagementSystemPhase2.DTOs;
using HospitalManagementSystemPhase2.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemPhase2.Services
{
    public class AppointmentDBAccess
    {
        private readonly HMSDBContext _context;

        public AppointmentDBAccess(HMSDBContext context)
        {
            _context = context;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments.AsNoTracking().ToList();
        }

        public Appointment GetAppointmentById(int id)
        {
            return _context.Appointments.FirstOrDefault(p => p.AppointmentId == id);
        }

        public List<AppointmentDto> GetScheduleByPatientId(int patientId)
        {
            var appointments = _context.Appointments
                                        .Where(a => a.PatientId == patientId)
                                        .Select(a => new AppointmentDto
                                        {
                                            AppointmentId = a.AppointmentId,
                                            AppointmentDate = a.AppointmentDate,
                                            Status = a.Status,
                                            DoctorId = a.DoctorId,
                                            PatientId = a.PatientId
                                        })
                                        .ToList();
            return appointments;
        }

        public bool CheckPatientById(int id)
        {
            return _context.Patients.Any(p => p.Id == id);
        }

        public List<AppointmentDto> GetScheduleByDoctorId(int doctorId)
        {
            var appointments = _context.Appointments
                                           .Where(a => a.DoctorId == doctorId)
                                           .Select(a => new AppointmentDto
                                           {
                                               AppointmentId = a.AppointmentId,
                                               AppointmentDate = a.AppointmentDate,
                                               Status = a.Status,
                                               DoctorId = a.DoctorId,
                                               PatientId = a.PatientId
                                           })
                                           .ToList();
            return appointments;
        }

        public bool CheckDoctorById(int id)
        {
            return _context.Doctors.Any(p => p.Id == id);
        }

        public Doctor GetDoctorById(int id)
        {
            return _context.Doctors.FirstOrDefault(p => p.Id == id);
        }

        public Patient GetPatientById(int id)
        {
            return _context.Patients.FirstOrDefault(p => p.Id == id);
        }

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

        public void ScheduleAppointment(Appointment newApp)
        {
            _context.Appointments.Add(newApp);
            _context.SaveChanges();
        }

        public bool isDoctorAvailiable(Appointment appointment)
        {
            return _context.Appointments.Any(d => d.DoctorId == appointment.DoctorId 
                                         && d.AppointmentDate.Equals(appointment.AppointmentDate));
        }

        public bool hasPatientSchedule(Appointment appointment)
        {
            return _context.Appointments.Any(d => d.PatientId == appointment.PatientId 
                                        && d.AppointmentDate.Equals(appointment.AppointmentDate));
        }

    }
}
