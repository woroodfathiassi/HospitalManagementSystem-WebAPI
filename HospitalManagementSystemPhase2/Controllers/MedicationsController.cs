using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.Managements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MedicationsController : ControllerBase
    {
        MedicationManagement _medicationManager;

        public MedicationsController(MedicationManagement medicationManager)
        {
            _medicationManager = medicationManager;
        }

        [HttpGet]
        // http://localhost:5268/api/Medications/GetMedications
        public IActionResult GetMedications()
        {
            try
            {
                var medications = _medicationManager.GetAllMedications();
                return Ok(medications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        [HttpPost]
        // http://localhost:5268/api/Medications/AddMedication
        public IActionResult AddMedication([FromBody] Medication medication)
        {
            try
            {
                _medicationManager.AddMedication(medication);
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

            return Created();
        }

        [HttpPut("{id:int}")]
        // http://localhost:5268/api/Medications/UpdateMedication/1
        public IActionResult UpdateMedication(int id, [FromBody] Medication medication)
        {
            try
            {
                _medicationManager.UpdateMedication(id, medication);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
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

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        // http://localhost:5268/api/Medications/DeleteMedication/4
        public IActionResult DeleteMedication(int id)
        {
            try
            {
                _medicationManager.DeleteMedication(id);
                return NoContent();
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
    }
}
