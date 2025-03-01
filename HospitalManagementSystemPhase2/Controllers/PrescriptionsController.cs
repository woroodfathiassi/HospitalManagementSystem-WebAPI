using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Managements;
using HospitalManagementSystemPhase2.MyExceptions;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        PrescriptionManagement _PrescriptionManager;
        public PrescriptionsController(PrescriptionManagement prescriptionManager)
        {
            _PrescriptionManager = prescriptionManager;
        }

        [HttpGet]
        public IActionResult GetPrescriptions()
        {
            var prescriptions = _PrescriptionManager.GetAllPrescriptions();
            return Ok(prescriptions);
        }

        [HttpPost]
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
        public IActionResult GetDoctor(int id)
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
    }
}
