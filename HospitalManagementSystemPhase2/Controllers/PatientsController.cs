using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.Managements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PatientsController: ControllerBase
    {
        PatientManagement _PatientManager;

        public PatientsController(PatientManagement patientManager)
        {
            _PatientManager = patientManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult GetPatients()
        {
            try
            {
                var patients = _PatientManager.GetAllPatients();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult GetPatient(int id)
        {
            try
            {
                var pat = _PatientManager.GetPatientById(id);

                if (User.IsInRole("Patient"))
                {
                    var loggedInUserId = User.FindFirst("UserId")?.Value;

                    if (!int.TryParse(loggedInUserId, out int userId))
                        return Unauthorized();

                    if (userId != pat.UserId)
                        return Unauthorized();
                }

                return Ok(pat);
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
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult AddPatient([FromBody]Patient patient)
        {
            try
            {
                _PatientManager.AddNewPatient(patient);
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
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult UpdatePatient(int id, [FromBody] Patient patient)
        {
            try
            {
                if (id != patient.Id)
                {
                    return BadRequest("Invalid Ids.");
                }

                if (User.IsInRole("Patient"))
                {
                    var loggedInUserId = User.FindFirst("UserId")?.Value;

                    if (!int.TryParse(loggedInUserId, out int userId))
                        return Unauthorized();

                    var pat = _PatientManager.GetPatientById(id);

                    if (userId != pat.UserId)
                        return Unauthorized();
                }

                _PatientManager.UpdatePatient(patient);
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
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult DeletePatient(int id)
        {
            try
            {
                _PatientManager.DeletePatient(id);
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
