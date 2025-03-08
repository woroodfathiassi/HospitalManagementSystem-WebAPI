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
            try
            {
                var appointments = _AppointmentManager.GetAllAppointments();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public IActionResult ScheduleAppointment([FromBody] Appointment appointment)
        {
            try
            {
                _AppointmentManager.ScheduleAppointment(appointment);
                return Created();
            }
            catch(ArgumentNullException ex) { return BadRequest(ex.Message); }
            catch (KeyNotFoundException ex) { return BadRequest(ex.Message); }
            catch(InvalidOperationException ex) { return BadRequest($"Invalid operation: {ex.Message}"); }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Patient")]
        public IActionResult GetAppointmentByPatientId([FromQuery]int patientId)
        {
            try
            {
                if (User.IsInRole("Patient"))
                {
                    var loggedInUserId = User.FindFirst("UserId")?.Value;

                    if (!int.TryParse(loggedInUserId, out int userId))
                        return Unauthorized();

                    var doc = _AppointmentManager.GetPatientById(patientId);

                    if (userId != doc.UserId)
                        return Unauthorized();
                }

                var appointments = _AppointmentManager.GetScheduleByPatientId(patientId);
                return Ok(appointments);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult GetAppointmentByDoctorId([FromQuery] int doctorId)
        {
            try
            {
                if (User.IsInRole("Doctor"))
                {
                    var loggedInUserId = User.FindFirst("UserId")?.Value;

                    if (!int.TryParse(loggedInUserId, out int userId))
                        return Unauthorized();

                    var doc = _AppointmentManager.GetDoctorById(doctorId);

                    if (userId != doc.UserId)
                        return Unauthorized();
                }

                var appointments = _AppointmentManager.GetScheduleByDoctorId(doctorId);
                return Ok(appointments);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public IActionResult UpdateAppointmentStatus(int id, [FromQuery] int status)
        {
            try
            {
                var app = _AppointmentManager.GetAppointmentById(id);
                
                if (User.IsInRole("Doctor"))
                {
                    var loggedInUserId = User.FindFirst("UserId")?.Value;

                    if (!int.TryParse(loggedInUserId, out int userId))
                        return Unauthorized();

                    var doc = _AppointmentManager.GetDoctorById(app.DoctorId);

                    if (userId != doc.UserId)
                        return Unauthorized();
                }

                _AppointmentManager.UpdateAppointmentStatus(id, status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        [HttpPut("{id:int}")]
        public IActionResult CancelAppointment(int id)
        {
            var app = _AppointmentManager.GetAppointmentById(id);

            if (User.IsInRole("Patient"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (!int.TryParse(loggedInUserId, out int userId))
                    return Unauthorized();

                var pat = _AppointmentManager.GetPatientById(app.PatientId);

                if (userId != pat.UserId)
                    return Unauthorized();
            }


            if (User.IsInRole("Doctor"))
            {
                var loggedInUserId = User.FindFirst("UserId")?.Value;

                if (!int.TryParse(loggedInUserId, out int userId))
                    return Unauthorized();

                var doc = _AppointmentManager.GetDoctorById(app.DoctorId);

                if (userId != doc.UserId)
                    return Unauthorized();
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
            catch(ArgumentException ex)
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
