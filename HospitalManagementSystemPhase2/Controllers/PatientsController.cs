using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Managements;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatientsController: ControllerBase
    {
        PatientManagement _PatientManager;

        public PatientsController(PatientManagement patientManager)
        {
            _PatientManager = patientManager;
        }

        [HttpGet]
        public IActionResult GetPatients()
        {
            var patients = _PatientManager.GetAllPatients();
            return Ok(patients);
        }


        [HttpGet("{id:int}")]
        public IActionResult GetPatient(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid patient ID.");
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
        public IActionResult UpdatePatient(int id, [FromBody] Patient patient)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
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

            if (patient == null)
            {
                return BadRequest("Patient data is required.");
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
