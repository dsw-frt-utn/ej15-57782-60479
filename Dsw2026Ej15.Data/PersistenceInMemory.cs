using System.Text.Json;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;

namespace Dsw2026Ej15.Data;

public class PersistenceInMemory : IPersistence
{
    private readonly List<Speciality> _specialities = new();
    private readonly List<Doctor> _doctors = new();

    public PersistenceInMemory()
    {
        LoadSpecialities();
    }
    private void LoadSpecialities()
    {
        try
        {
            var json = File.ReadAllText("specialities.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var specialities = JsonSerializer.Deserialize<List<Speciality>>(json, options);
            if (specialities != null)
            {
                _specialities.AddRange(specialities);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error crítico al cargar JSON: {ex.Message}");
        }
    }

    public List<Speciality> GetAllSpecialities() => _specialities;
    public Speciality? GetSpecialityById(Guid id) => _specialities.FirstOrDefault(s => s.Id == id);

    public List<Doctor> GetAllDoctors() => _doctors;

    public Doctor? GetDoctorById(Guid id) => _doctors.FirstOrDefault(d => d.Id == id);

    public void AddDoctor(Doctor doctor)
    {
        _doctors.Add(doctor);
    }

    public void UpdateDoctor(Doctor doctor)
    {
        var index = _doctors.FindIndex(d => d.Id == doctor.Id);
        if (index != -1)
        {
            _doctors[index] = doctor;
        }
    }

    public Speciality? GetSpecialityByld(Guid id)
    {
        throw new NotImplementedException();
    }
}