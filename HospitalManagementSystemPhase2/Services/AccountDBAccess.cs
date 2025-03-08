using HospitalManagementSystemPhase2.DTOs;
using HospitalManagementSystemPhase2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HospitalManagementSystemPhase2.Services
{
    public class AccountDBAccess
    {
        private readonly HMSDBContext _context;

        public AccountDBAccess(HMSDBContext context)
        {
            _context = context;
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }

        public Role GetRoleById(int id)
        {
            return _context.Roles.FirstOrDefault(a => a.Id == id);
        }

        public void RegisterNewUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void AddNewDoctor(Doctor doc)
        {
            _context.Doctors.Add(doc);
            _context.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.BeginTransaction();
        }

        public void AddNewPatient(Patient pat)
        {
            _context.Patients.Add(pat);
            _context.SaveChanges();
        }

        public User GetLoginUser(LoginDto userlogin)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserName == userlogin.UserName && u.Password == userlogin.Password);

            return user;
        }
    }
}
