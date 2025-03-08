using HospitalManagementSystemPhase2;
using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.DTOs;
using HospitalManagementSystemPhase2.MyExceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HospitalManagementSystemPhase2.Services;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HospitalManagementSystemPhase2.Managers
{
    public class AccountManagement
    {
        private readonly AccountDBAccess _accountDBAccess;
        private readonly IConfiguration _configuration;

        public AccountManagement(AccountDBAccess accountDBAccess, IConfiguration configuration)
        {
            _accountDBAccess = accountDBAccess;
            _configuration = configuration;
        }

        public void Register(UserDto user)
        {
            if (user == null)
                throw new ArgumentNullException("User data cannot be null.");


            var existingUser = _accountDBAccess.GetUserByUsername(user.UserName);
            if (existingUser != null)
                throw new UsernameAlreadyExistsException("Username already exists");

            var role = _accountDBAccess.GetRoleById(user.RoleId);
            if (role == null)
                throw new ArgumentException("Invalid Role ID");

            var newUser = new User
            {
                UserName = user.UserName,
                Password = user.Password,
                Role = role
            };

            using (var transaction = _accountDBAccess.BeginTransaction())
            {
                try
                {
                    _accountDBAccess.RegisterNewUser(newUser);

                    if (role.Name.Equals("Doctor", StringComparison.OrdinalIgnoreCase))
                    {
                        var doctor = new Doctor { Name = newUser.UserName, User = newUser };
                        _accountDBAccess.AddNewDoctor(doctor);
                        newUser.Doctor = doctor;
                    }
                    else if (role.Name.Equals("Patient", StringComparison.OrdinalIgnoreCase))
                    {
                        var patient = new Patient { Name = newUser.UserName, User = newUser };
                        _accountDBAccess.AddNewPatient(patient);
                        newUser.Patient = patient;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("User registration failed. Transaction rolled back.", ex);
                }
            }
        }

        public string Authenticate(LoginDto userlogin)
        {
            var user = _accountDBAccess.GetLoginUser(userlogin);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return GenerateJwtToken(user);
        }

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
                new Claim(ClaimTypes.Role, user.Role.Name)
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
