using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Domain.Exceptions;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpPost]
    public IActionResult CreateDoctor([FromBody] Doctor doctor)
    {
        if (string.IsNullOrWhiteSpace(doctor.Name))
            throw new ValidationException("El nombre del médico es un campo obligatorio.");

        if (doctor.Speciality == null || doctor.Speciality.Id == Guid.Empty)
            throw new ValidationException("La especialidad del médico es obligatoria.");

        var existingSpeciality = _persistence.GetSpecialityById(doctor.Speciality.Id);
        if (existingSpeciality == null)
            throw new ValidationException($"No se encontró la especialidad con el Id {doctor.Speciality.Id}.");

        doctor.Speciality = existingSpeciality;
        doctor.IsActive = true;

        _persistence.AddDoctor(doctor);

        return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, doctor);
    }

    [HttpGet]
    public IActionResult GetDoctors()
    {
        var doctors = _persistence.GetAllDoctors();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public IActionResult GetDoctorById(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

        if (doctor == null)
        {
            return NotFound(new { message = $"No se encontró ningún médico con el Id {id}" });
        }

        return Ok(doctor);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDoctor(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

        if (doctor == null)
        {
            return NotFound(new { message = $"No se encontró ningún médico con el Id {id}" });
        }

        doctor.IsActive = false;

        _persistence.UpdateDoctor(doctor);

        return NoContent();
    }
}