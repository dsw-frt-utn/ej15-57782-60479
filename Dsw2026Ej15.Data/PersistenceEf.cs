using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data;

public class PersistenceEf : IPersistence
{
    private readonly Dsw2026Ej15Context _context;

    public PersistenceEf(Dsw2026Ej15Context context)
    {
        _context = context;
    }

    public List<Speciality> GetAllSpecialities()
        => _context.Specialities.ToList();

    public Speciality? GetSpecialityById(Guid id)
        => _context.Specialities.FirstOrDefault(s => s.Id == id);

    public List<Doctor> GetAllDoctors()
        => _context.Doctors.Include(d => d.Speciality).ToList();

    public Doctor? GetDoctorById(Guid id)
        => _context.Doctors.Include(d => d.Speciality).FirstOrDefault(d => d.Id == id);

    public void AddDoctor(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        _context.SaveChanges();
    }

    public void UpdateDoctor(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        _context.SaveChanges();
    }
}
