using HospitalManagementSystemPhase2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Entities
{
    public class Patient: Person
    {
        public List<Prescription>? Prescriptions { get; set; }

        public List<Appointment>? Appointments { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}, Contact Number: {ContactNumber}, Address: {Address}";
        }

    }
}
