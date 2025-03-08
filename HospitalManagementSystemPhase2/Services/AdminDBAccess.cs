using HospitalManagementSystemPhase2.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemPhase2.Services
{
    public class AdminDBAccess
    {
        private readonly HMSDBContext _context;

        public AdminDBAccess(HMSDBContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.AsNoTracking().ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(p => p.UserId == id);
        }

        public Role GetRoleById(int id)
        {
            return _context.Roles.FirstOrDefault(a => a.Id == id);
        }

        //public void UpdateUserRole(User user, Role role)
        //{
        //    user.Role = role;
        //    _context.SaveChanges();
        //}

        public void UpdateUserRole(User user, Role role)
        {
            var currentRoleId = user.RoleId;

            if (currentRoleId == 2)
            {
                var doctor = _context.Doctors.FirstOrDefault(d => d.UserId == user.UserId);
                if (doctor != null)
                {
                    _context.Doctors.Remove(doctor);
                    user.Doctor = null;
                }
            }
            else if (currentRoleId == 3) 
            {
                var patient = _context.Patients.FirstOrDefault(p => p.UserId == user.UserId);
                if (patient != null)
                {
                    _context.Patients.Remove(patient);
                    user.Patient = null;
                }
            }

            user.Role = role;
            _context.SaveChanges();

            if (role.Id == 2) 
            {
                var newDoctor = new Doctor { UserId = user.UserId, Name = user.UserName };
                _context.Doctors.Add(newDoctor);
            }
            else if (role.Id == 3) 
            {
                var newPatient = new Patient { UserId = user.UserId, Name = user.UserName };
                _context.Patients.Add(newPatient);
            }

            _context.SaveChanges();
        }

    }
}
