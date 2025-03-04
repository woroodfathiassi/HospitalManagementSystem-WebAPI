using HospitalManagementSystemPhase2.Entities;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemPhase2.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<User>? Users { get; set; }
    }
}
