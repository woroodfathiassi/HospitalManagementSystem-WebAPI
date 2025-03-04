using HospitalManagementSystemPhase2.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystemPhase2.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [StringLength(120)]
        public string Name { get; set; }

        [Range(0, 120)]
        public int? Age { get; set; }

        public string? Gender { get; set; }

        [RegularExpression(@"^05\d{8}$", ErrorMessage = "Contact number must be 10 digits and start with '05'.")]
        public string? ContactNumber { get; set; }

        public string? Address { get; set; }
    }
}
