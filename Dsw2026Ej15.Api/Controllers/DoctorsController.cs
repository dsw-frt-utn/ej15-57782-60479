using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static Dsw2026Ej15.Api.Models.DoctorModel;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpPost]
    public IActionResult CreateDoctor([FromBody] Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("El nombre del médico es un campo obligatorio.");

        if (string.IsNullOrWhiteSpace(request.LicenseNumber))
            throw new ValidationException("El número de matrícula es un campo obligatorio.");

        var speciality = _persistence.GetSpecialityById(request.SpecialityId);
        if (speciality == null)
            throw new ValidationException($"No se encontró la especialidad con el Id {request.SpecialityId}.");

        var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);

        _persistence.AddDoctor(doctor);

        return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, doctor);
    }

    [HttpGet]
    public IActionResult GetDoctors()
    {
        var doctors = _persistence.GetAllDoctors().Where(d => d.IsActive);
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public IActionResult GetDoctorById(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

        if (doctor == null || !doctor.IsActive)
        {
            return NotFound(new { message = $"No se encontró ningún médico activo con el Id {id}" });
        }

        var response = new Response(doctor.Name, doctor.LicenseNumber, doctor.Speciality.Name);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDoctor(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

        if (doctor == null || !doctor.IsActive)
        {
            return NotFound(new { message = $"No se encontró ningún médico activo con el Id {id}" });
        }

        doctor.IsActive = false;
        _persistence.UpdateDoctor(doctor);

        return NoContent();
    }
}