using HospitalManagementSystem;
using HospitalManagementSystem.Entities;
using HospitalManagementSystemPhase2.DTOs;
using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.MyExceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalManagementSystemPhase2.Managers
{
    public class AuthManagement
    {
        private readonly HMSDBContext _context;
        private readonly IConfiguration _configuration;

        public AuthManagement(HMSDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void Register(UserDto user)
        {

            var existingUser = _context.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (existingUser != null)
                throw new UsernameAlreadyExistsException("Username already exists");


            var newUser = new User
            {
                UserName = user.UserName,
                Password = user.Password,
                RoleId = user.RoleId
            };

            var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            if (role == null)
                throw new ArgumentException("Invalid Role ID");
            else if (role.Name.Equals("Doctor"))
            {
                var doc = new Doctor { Name = newUser.UserName, User = newUser };
                _context.Doctors.Add(doc);
                newUser.Doctor = doc;
            }
            else if (role.Name.Equals("Patient"))
            {
                var pat = new Patient { Name = newUser.UserName, User = newUser };
                _context.Patients.Add(pat);
                newUser.Patient = pat;
            }

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        //public string Authenticate(LoginDto userlogin)
        //{
        //    var user = _context.Users
        //        .Include(u => u.Roles)
        //        .FirstOrDefault(u => u.UserName == userlogin.UserName && u.Password == userlogin.Password);

        //    if (user == null)
        //    {
        //        throw new UnauthorizedAccessException("Invalid username or password.");
        //    }

        //    return GenerateJwtToken(user);
        //}

        private string GenerateJwtToken(User user)
        {
            var secretKey = _configuration["JWT:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            SigningCredentials sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("UserName", user.UserName),
                new Claim("UserName", user.UserName)
            };
            JwtSecurityToken token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: sc
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
