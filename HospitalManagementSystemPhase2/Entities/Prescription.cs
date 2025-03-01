using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Entities
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }

        public DateTime PrescriptionDate { get; set; }

        public List<Medication> Medications { get; set; }

        public Bill Bill {  get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public override string ToString()
        {
            return $"Prescription Id: {PrescriptionId}, patientId: {PatientId}, doctorId: {DoctorId}";
        }
    }
}
