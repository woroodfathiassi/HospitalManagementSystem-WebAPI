using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Managements;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        MedicationManagement _medicationManager;

        public MedicationsController(MedicationManagement medicationManager)
        {
            _medicationManager = medicationManager;
        }

        [HttpGet]
        public IActionResult GetMedications()
        {
            var medications = _medicationManager.GetAllMedications();
            return Ok(medications);
        }

        [HttpPost]
        public IActionResult AddMedication([FromBody] Medication medication)
        {
            if (medication == null)
            {
                return BadRequest("Medication data is required.");
            }

            try
            {
                _medicationManager.AddMedication(medication);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Created();
            //return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateMedication(int id, [FromBody] Medication medication)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            if (medication == null)
            {
                return BadRequest("Medication data is required.");
            }

            var med = _medicationManager.GetMedicationById(id);

            if (med == null)
            {
                return NotFound($"Medication with ID {id} not found.");
            }

            if (id != medication.MedicationId)
            {
                return BadRequest("Invalid Ids.");
            }

            try
            {
                _medicationManager.UpdateMedication(medication);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteMedication(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            var medication = _medicationManager.GetMedicationById(id);

            if (medication == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            _medicationManager.DeleteMedication(id);
            return NoContent();
        }
    }
}
