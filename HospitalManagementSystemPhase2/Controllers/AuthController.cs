using HospitalManagementSystemPhase2.DTOs;
using HospitalManagementSystemPhase2.Managers;
using HospitalManagementSystemPhase2.MyExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AuthController: ControllerBase
    {
        AuthManagement _AuthManager;

        public AuthController(AuthManagement authManager)
        {
            _AuthManager = authManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Register([FromBody]UserDto user)
        {
            try
            {
                _AuthManager.Register(user);
                return Created();
            }
            catch (UsernameAlreadyExistsException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto user)
        {
            try
            {
                var token = _AuthManager.Authenticate(user);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
