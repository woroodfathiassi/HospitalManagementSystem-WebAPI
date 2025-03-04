using HospitalManagementSystemPhase2.Entities;
using HospitalManagementSystemPhase2.Managements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        AppointmentManagement _AppointmentManager;
        public AppointmentsController(AppointmentManagement appointmentManager)
        {
            _AppointmentManager = appointmentManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAppointments()
        {
            var appointments = _AppointmentManager.GetAllAppointments();
            return Ok(appointments);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,Doctor,Patient")]
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
        [Authorize(Roles = "Admin,Patient")]
        public IActionResult GetAppointmentByPatientId([FromQuery]int patientId)
        {
            if (patientId <= 0)
            {
                return BadRequest("Invalid appointment with patient ID.");
            }

            if (User.IsInRole("Patient"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (loggedInUserId == null || loggedInUserId != patientId.ToString())
                {
                    return Unauthorized();
                }
            }

            var appointments = _AppointmentManager.GetScheduleByPatientId(patientId);

            if (!appointments.Any())
            {
                return NotFound($"No appointments found for patient ID {patientId}.");
            }

            return Ok(appointments);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult GetAppointmentByDoctorId([FromQuery] int doctorId)
        {
            if (doctorId <= 0)
            {
                return BadRequest("Invalid appointment with doctor ID.");
            }

            if (User.IsInRole("Doctor"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (loggedInUserId == null || loggedInUserId != doctorId.ToString())
                {
                    return Unauthorized();
                }
            }

            var appointments = _AppointmentManager.GetScheduleByDoctorId(doctorId);

            if (!appointments.Any())
            {
                return NotFound($"No appointments found for doctor ID {doctorId}.");
            }

            return Ok(appointments);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult UpdateAppointmentStatus(int id, int status)
        {
            if (id <= 0 || status <= 0)
            {
                return BadRequest("Invalid inputs.");
            }

            var app = _AppointmentManager.GetAppointmentById(id);

            if (User.IsInRole("Doctor"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (loggedInUserId == null || loggedInUserId != app.Doctor.UserId.ToString())
                {
                    return Unauthorized();
                }
            }

            try
            {
                _AppointmentManager.UpdateAppointmentStatus(id, status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult CancelAppointment(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            var app = _AppointmentManager.GetAppointmentById(id);

            if (User.IsInRole("Patient"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (loggedInUserId == null || loggedInUserId != app.Patient.UserId.ToString())
                {
                    return Unauthorized();
                }
            }

            if (User.IsInRole("Doctor"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;
                
                if (loggedInUserId == null || loggedInUserId != app.Doctor.UserId.ToString())
                {
                    return Unauthorized();
                }
            }

            try
            {
                _AppointmentManager.CancelAppointment(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
