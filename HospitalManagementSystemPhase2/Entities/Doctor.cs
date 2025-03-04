using HospitalManagementSystemPhase2.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystemPhase2.Entities
{
    public class Doctor: Person
    {
        [StringLength(120)]
        public string? Email { get; set; }

        [StringLength(120)]
        public string? Specialty { get; set; }

        public List<Prescription>? Prescriptions { get; set; }

        public List<Appointment>? Appointments { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}, Contact Number: {ContactNumber}, Address: {Address}, Email: {Email}, Specialty: {Specialty}";
        }

    }
}
