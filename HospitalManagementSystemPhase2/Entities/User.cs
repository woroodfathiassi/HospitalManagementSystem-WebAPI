using HospitalManagementSystemPhase2.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemPhase2.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }

    }
}
