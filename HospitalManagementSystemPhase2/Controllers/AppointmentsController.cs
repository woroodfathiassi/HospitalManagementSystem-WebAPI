using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Managements;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        AppointmentManagement _AppointmentManager;
        public AppointmentsController(AppointmentManagement appointmentManager)
        {
            _AppointmentManager = appointmentManager;
        }

        [HttpGet]
        public IActionResult GetAppointments()
        {
            var appointments = _AppointmentManager.GetAllAppointments();
            return Ok(appointments);
        }

        [HttpPost]
        public IActionResult ScheduleAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Appointment data is required.");
            }

            try
            {
                _AppointmentManager.ScheduleAppointment(appointment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Created();
        }

        [HttpGet]
        public IActionResult GetAppointmentByPatientId([FromQuery]int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest("Invalid appointment with patient ID.");
            }

            var appointments = _AppointmentManager.GetScheduleByPatientId(patientId);

            if (!appointments.Any())
            {
                return NotFound($"No appointments found for patient ID {patientId}.");
            }

            return Ok(appointments);
        }

        [HttpGet]
        public IActionResult GetAppointmentByDoctorId([FromQuery] int doctorId)
        {
            if (doctorId <= 0)
            {
                return BadRequest("Invalid appointment with doctor ID.");
            }

            var appointments = _AppointmentManager.GetScheduleByDoctorId(doctorId);

            if (!appointments.Any())
            {
                return NotFound($"No appointments found for doctor ID {doctorId}.");
            }

            return Ok(appointments);
        }

        [HttpPut("{id:int}")]
        public IActionResult CancelAppointment(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            try
            {
                _AppointmentManager.CancelAppointment(id);
                return NoContent();
                //return Ok($"Appointment with ID {id} has been canceled.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
