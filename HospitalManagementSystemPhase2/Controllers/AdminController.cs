using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        AdminManagement _AdminManager;
        public AdminController(AdminManagement AdminManager)
        {
            _AdminManager = AdminManager;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var doctors = _AdminManager.GetAllUsers();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("{id:int}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = _AdminManager.GetUserById(id);
                return Ok(user);
            }
            catch(ArgumentException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUserRole(int id, [FromQuery]int roleId)
        {
            try
            {
                _AdminManager.UpdateUserRole(id, roleId);
                return NoContent();
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
