using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.Managements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var patients = _PatientManager.GetAllPatients();
            return Ok(patients);
        }


        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult GetPatient(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid patient ID.");
            }

            if (User.IsInRole("Patient"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (loggedInUserId == null || loggedInUserId != id.ToString())
                {
                    return Unauthorized();
                }
            }

            var patient = _PatientManager.GetPatientById(id);

            if (patient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            return Ok(patient);
        }
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetPatient([FromRoute] int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest("Invalid patient ID.");
        //    }

        //    var patient = await _PatientManager.GetPatientByIdAsync(id);

        //    return patient != null
        //        ? Ok(patient)
        //        : NotFound($"Patient with ID {id} not found.");
        //}

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult AddPatient([FromBody]Patient patient)
        {
            if (patient == null)
            {
                return BadRequest("Patient data is required.");
            }

            try
            {
                _PatientManager.AddNewPatient(patient);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Created();
            //return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult UpdatePatient(int id, [FromBody] Patient patient)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            if (patient == null)
            {
                return BadRequest("Patient data is required.");
            }

            var pat = _PatientManager.GetPatientById(id);

            if (pat == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            if (id != patient.Id)
            {
                return BadRequest("Invalid Ids.");
            }

            if (User.IsInRole("Patient"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (loggedInUserId == null || loggedInUserId != id.ToString())
                {
                    return Unauthorized(); 
                }
            }

            try
            {
                _PatientManager.UpdatePatient(patient);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult DeletePatient(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            var patient = _PatientManager.GetPatientById(id); 

            if (patient == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            _PatientManager.DeletePatient(id);
            return NoContent();
        }

    }
}
