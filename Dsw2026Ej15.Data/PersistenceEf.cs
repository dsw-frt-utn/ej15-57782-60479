using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data;

public class PersistenceEf : IPersistence
{
    private readonly AppDbContext _context;
    public PersistenceEf(AppDbContext context)
    {
        _context = context;
    }

    public List<Speciality> GetAllSpecialities()
    {
        return _context.Specialities.ToList();
    }

    public Speciality? GetSpecialityById(Guid id)
    {
        return _context.Specialities.FirstOrDefault(s => s.Id == id);
    }

    public List<Doctor> GetAllDoctors()
    {
        return _context.Doctors.Include(d => d.Speciality).ToList();
    }

    public Doctor? GetDoctorById(Guid id)
    {
        return _context.Doctors.Include(d => d.Speciality).FirstOrDefault(d => d.Id == id);
    }

    public void AddDoctor(Doctor doctor)
    {
        if (doctor.Speciality != null)
        {
            _context.Specialities.Attach(doctor.Speciality);
        }

        _context.Doctors.Add(doctor);
        _context.SaveChanges();
    }

    public void UpdateDoctor(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        _context.SaveChanges(); 
    }
}