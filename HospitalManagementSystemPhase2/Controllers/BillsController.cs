using HospitalManagementSystem.Managements;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BillsController: ControllerBase
    {
        BillingManagement _BillingManager;

        public BillsController(BillingManagement billingManager)
        {
            _BillingManager = billingManager;
        }

        [HttpGet]
        public IActionResult GetBills()
        {
            var bills = _BillingManager.GetBills();
            return Ok(bills);
        }

        [HttpGet]
        public IActionResult GetBillsByPatientId([FromQuery] int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest("Invalid patient ID.");
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
