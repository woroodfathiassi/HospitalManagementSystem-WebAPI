using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HospitalManagementSystemPhase2.Entities
{
    public enum AppointmentStatus
    {
        Scheduled, 
        Completed, 
        Canceled
    }

    public class Appointment
    {
        public int AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public AppointmentStatus Status { get; set; }

        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        public override string ToString()
        {
            return $"Appointment ID: {AppointmentId}, Date: {AppointmentDate}, Status: {Status}";
        }
    }
}
