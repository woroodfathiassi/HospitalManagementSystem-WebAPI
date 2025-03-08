using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.Services;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemPhase2.Managers
{
    public class AdminManagement
    {
        private readonly AdminDBAccess _adminDBAccess;

        public AdminManagement(AdminDBAccess adminDBAccess)
        {
            _adminDBAccess = adminDBAccess;
        }

        public List<User> GetAllUsers()
        {
            return _adminDBAccess.GetAllUsers();
        }

        public User GetUserById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid User ID.");
            } 
            var user = _adminDBAccess.GetUserById(id);
            if (user != null)
            {
                return _adminDBAccess.GetUserById(id);
            }
            else
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            
        }

        public void UpdateUserRole(int userId, int roleId)
        {
            if (userId <= 0 || roleId <= 0)
            {
                throw new ArgumentException("Invalid inputs.");
            }

            var user = _adminDBAccess.GetUserById(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var role = _adminDBAccess.GetRoleById(roleId);

            if (role is null)
            {
                throw new KeyNotFoundException("Role not found.");
            }


            _adminDBAccess.UpdateUserRole(user, role);
        }
    }
}
