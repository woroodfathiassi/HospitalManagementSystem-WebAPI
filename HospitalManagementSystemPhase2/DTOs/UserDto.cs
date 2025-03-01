using HospitalManagementSystemPhase2.Entities;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemPhase2.DTOs
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters long.")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string PasswordConf { get; set; }

        public int RoleId { get; set; }
    }
}
