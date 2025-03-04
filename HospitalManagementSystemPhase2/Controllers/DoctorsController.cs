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
            var doctors = _DoctorManager.GetAllDoctors();
            return Ok(doctors);
        }


        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult GetDoctor(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid doctor ID.");
            }

            if (User.IsInRole("Doctor"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (loggedInUserId == null || loggedInUserId != id.ToString())
                {
                    return Unauthorized();
                }
            }

            var doctor = _DoctorManager.GetDoctorById(id);

            if (doctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }

            return Ok(doctor);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null)
            {
                return BadRequest("Doctor data is required.");
            }

            try
            {
                _DoctorManager.AddNewDoctor(doctor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Created();
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            if (doctor == null)
            {
                return BadRequest("Doctor data is required.");
            }

            if (id != doctor.Id)
            {
                return BadRequest("Invalid Ids.");
            }

            var doc = _DoctorManager.GetDoctorById(id);

            if (doc == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            if (User.IsInRole("Doctor"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (loggedInUserId == null || loggedInUserId != id.ToString())
                {
                    return Unauthorized();
                }
            }

            try
            {
                _DoctorManager.UpdateDoctor(doctor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteDoctor(int id)
        {
            var doctor = _DoctorManager.GetDoctorById(id);

            if (doctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }

            _DoctorManager.DeleteDoctor(id);
            return NoContent();
        }
    }
}
