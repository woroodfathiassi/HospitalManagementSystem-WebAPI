﻿using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Managements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemPhase2.Controllers
{
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        DoctorManagement _DoctorManager;

        public DoctorsController(DoctorManagement doctorManager)
        {
            _DoctorManager = doctorManager;
        }

        [HttpGet]
        //[Authorize(Roles = "Doctor")]
        public IActionResult GetDoctors()
        {
            var doctors = _DoctorManager.GetAllDoctors();
            return Ok(doctors);
        }


        [HttpGet("{id:int}")]
        public IActionResult GetDoctor(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid doctor ID.");
            }

            var doctor = _DoctorManager.GetDoctorById(id);

            if (doctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }

            return Ok(doctor);
        }

        [HttpPost]
        public IActionResult AddDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null)
            {
                return BadRequest("Doctor data is required.");
            }

            try
            {
                _DoctorManager.AddNewDoctor(doctor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Created();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            var doc = _DoctorManager.GetDoctorById(id);

            if (doc == null)
            {
                return NotFound($"Patient with ID {id} not found.");
            }

            if (id != doctor.Id)
            {
                return BadRequest("Invalid Ids.");
            }

            if (doctor == null)
            {
                return BadRequest("Doctor data is required.");
            }

            try
            {
                _DoctorManager.UpdateDoctor(doctor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteDoctor(int id)
        {
            var doctor = _DoctorManager.GetDoctorById(id);

            if (doctor == null)
            {
                return NotFound($"Doctor with ID {id} not found.");
            }

            _DoctorManager.DeleteDoctor(id);
            return NoContent();
        }
    }
}
