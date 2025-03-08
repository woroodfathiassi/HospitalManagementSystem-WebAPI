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
        // http://localhost:5268/api/Prescriptions/GetPrescriptions
        public IActionResult GetPrescriptions()
        {
            var prescriptions = _PrescriptionManager.GetAllPrescriptions();
            return Ok(prescriptions);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        //http://localhost:5268/api/Prescriptions/AddPrescription
        //{
        //    "patientId": 1,
        //    "doctorId": 5,
        //    "medications": [
        //        { "MedicationId": 20 },
        //        { "MedicationId": 10 }
        //    ]
        //}
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
        // http://localhost:5268/api/Prescriptions/GetPrescription/1
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
        // http://localhost:5268/api/Prescriptions/UpdatePrescription/2
        //{
        //    "PrescriptionId" : 2,
        //    "patientId": 1,
        //    "doctorId": 5,
        //    "medications": [
        //        { "MedicationId": 10 },
        //        { "MedicationId": 20 },
        //        { "MedicationId": 30 }
        //    ]
        //}
        public IActionResult UpdatePrescription(int id, [FromBody] Prescription prescription)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Id.");
            }

            if (prescription == null)
            {
                return BadRequest("Prescription data is required.");
            }

            var pre = _PrescriptionManager.GetPrescriptionById(id);

            if (pre == null)
            {
                return NotFound($"Prescription with ID {id} not found.");
            }

            if (id != prescription.PrescriptionId)
            {
                return BadRequest("Invalid Ids.");
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
        //http://localhost:5268/api/Prescriptions/GetPatientPrescriptiond?patientId=3
        public IActionResult GetPatientPrescriptiond([FromQuery] int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest("Invalid patient ID.");
            }

            var pat = _PrescriptionManager.GetPatientById(patientId);

            if (User.IsInRole("Patient"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (!int.TryParse(loggedInUserId, out int userId))
                    return Unauthorized();

                if (userId != pat.UserId)
                    return Unauthorized();
            }

            var prescriptions = _PrescriptionManager.GetPatientPrescriptiond(patientId);
            return Ok(prescriptions);
        }
    }
}
