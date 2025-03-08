using HospitalManagementSystemPhase2.Entities;

namespace HospitalManagementSystemPhase2.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public AppointmentStatus Status { get; set; }

        public int DoctorId { get; set; }

        public int PatientId { get; set; }
    }
}
