using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.Managements;
using HospitalManagementSystemPhase2.MyExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PrescriptionsController : ControllerBase
    {
        PrescriptionManagement _PrescriptionManager;
        public PrescriptionsController(PrescriptionManagement prescriptionManager)
        {
            _PrescriptionManager = prescriptionManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPrescriptions()
        {
            var prescriptions = _PrescriptionManager.GetAllPrescriptions();
            return Ok(prescriptions);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult AddPrescription([FromBody] Prescription prescription)
        {
            if (prescription == null)
            {
                return BadRequest("Prescription data is required.");
            }

            try
            {
                _PrescriptionManager.IssuePrescription(prescription);
                return Created();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (MedicationOutOfStockException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPrescription(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid prescription ID.");
            }

            var prescription = _PrescriptionManager.GetPrescriptionById(id);

            if (prescription == null)
            {
                return NotFound($"Prescription with ID {id} not found.");
            }

            return Ok(prescription);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdatePrescription(int id, [FromBody] Prescription prescription)
        {
            var pre = _PrescriptionManager.GetPrescriptionById(id);

            if (pre == null)
            {
                return NotFound($"Prescription with ID {id} not found.");
            }

            if (id != prescription.PrescriptionId)
            {
                return BadRequest("Invalid Ids.");
            }

            if (prescription == null)
            {
                return BadRequest("Prescription data is required.");
            }

            try
            {
                _PrescriptionManager.UpdatePrescription(prescription);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MedicationOutOfStockException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Patient")]
        public IActionResult GetPatientPrescriptiond([FromQuery] int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest("Invalid patient ID.");
            }

            if (User.IsInRole("Patient"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (loggedInUserId == null || loggedInUserId != patientId.ToString())
                {
                    return Unauthorized();
                }
            }

            var prescriptions = _PrescriptionManager.GetPatientPrescriptiond(patientId);
            return Ok(prescriptions);
        }
    }
}
