using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystemPhase2.Entities
{
    public class Medication
    {
        public int MedicationId { get; set; }

        [StringLength(120)]
        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public List<Prescription>? Prescriptions { get; set; }

        public override string ToString()
        {
            return $"Medication ID: {MedicationId}, Name: {Name}, Quantity: {Quantity}, Price: {Price:C}"; 
        }

    }
}
