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
            var bills = _BillingManager.GetBills();
            return Ok(bills);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Patient")]
        public IActionResult GetBillsByPatientId([FromQuery] int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest("Invalid patient ID.");
            }

            

            //if (User.IsInRole("Patient"))
            //{
            //    var loggedInUserId = User.FindFirst("UserId")?.Value;

            //    if (loggedInUserId == null || loggedInUserId != patientId.ToString())
            //    {
            //        return Unauthorized();
            //    }
            //}

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
        public IActionResult UpdateBillStatus(int id, int status)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                _BillingManager.UpdateBillStatus(id, status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
