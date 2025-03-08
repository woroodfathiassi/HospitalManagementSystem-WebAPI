using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.Managements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize] 
    public class DoctorsController : ControllerBase
    {
        DoctorManagement _DoctorManager;

        public DoctorsController(DoctorManagement doctorManager)
        {
            _DoctorManager = doctorManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDoctors()
        {
            try
            {
                var doctors = _DoctorManager.GetAllDoctors();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult GetDoctor(int id) 
        {
            try
            {
                var doctor = _DoctorManager.GetDoctorById(id);

                if (User.IsInRole("Doctor"))
                {
                    var loggedInUserId = User.FindFirst("UserId")?.Value;

                    if (!int.TryParse(loggedInUserId, out int userId))
                        return Unauthorized();

                    if (userId != doctor.UserId)
                        return Unauthorized();
                }
                
                return Ok(doctor);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddDoctor([FromBody] Doctor doctor)
        {
            try
            {
                _DoctorManager.AddNewDoctor(doctor);
                return Created();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            try
            {
                if (id != doctor.Id)
                {
                    return BadRequest("Invalid Ids.");
                }

                if (User.IsInRole("Doctor"))
                {
                    var loggedInUserId = User.FindFirst("UserId")?.Value;

                    if (!int.TryParse(loggedInUserId, out int userId))
                        return Unauthorized();

                    var doc = _DoctorManager.GetDoctorById(id);

                    if (userId != doc.UserId)
                        return Unauthorized();
                }

                _DoctorManager.UpdateDoctor(doctor);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteDoctor(int id)
        {
            try
            {
                _DoctorManager.DeleteDoctor(id);
                return NoContent();
            }
            catch (ArgumentException ex)
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
