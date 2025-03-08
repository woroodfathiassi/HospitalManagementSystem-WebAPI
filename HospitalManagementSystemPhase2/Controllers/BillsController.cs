using HospitalManagementSystemPhase2.DTOs;
using HospitalManagementSystemPhase2.Managements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BillsController: ControllerBase
    {
        BillingManagement _BillingManager;

        public BillsController(BillingManagement billingManager)
        {
            _BillingManager = billingManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult GetBills()
        {
            if (User.IsInRole("Doctor"))
            {
                var loggedInUserIdClaim = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(loggedInUserIdClaim) || !int.TryParse(loggedInUserIdClaim, out int loggedInUserId))
                {
                    return BadRequest("Invalid or missing UserId.");
                }

                var doctor = _BillingManager.GetDoctorByUserId(loggedInUserId);
                Console.WriteLine(doctor.ToString() + " "+ doctor.UserId);

                var Dbills = _BillingManager.GetBillsByDoctor(doctor);
                return Ok(Dbills);
            }

            var bills = _BillingManager.GetBills();
            return Ok(bills);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Patient")]
        // http://localhost:5268/api/Bills/GetBillsByPatientId?patientId=1
        public IActionResult GetBillsByPatientId([FromQuery] int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest("Invalid patient ID.");
            }

            var pat = _BillingManager.GetPatientById(patientId);

            if (User.IsInRole("Patient"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (!int.TryParse(loggedInUserId, out int userId))
                    return Unauthorized();

                if (userId != pat.UserId)
                    return Unauthorized();
            }

            try
            {
                var bills = _BillingManager.GetBillsByPatientId(patientId);
                
                if (!bills.Any())
                {
                    return NotFound($"No bills found for patient ID {patientId}.");
                }

                return Ok(bills);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateBillStatus(int id, [FromBody] UpdateBillStatusDto request)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                _BillingManager.UpdateBillStatus(id, request.Status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
