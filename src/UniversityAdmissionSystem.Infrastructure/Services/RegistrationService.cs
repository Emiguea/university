using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Repositories;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IRepository<Registration> _registrationRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly ApplicationDbContext _context;

    public RegistrationService(
        IRepository<Registration> registrationRepository,
        IRepository<Student> studentRepository,
        ApplicationDbContext context)
    {
        _registrationRepository = registrationRepository;
        _studentRepository = studentRepository;
        _context = context;
    }

    public async Task<Registration?> GetRegistrationByIdAsync(int id)
    {
        return await _context.Registrations
            .Include(r => r.Student)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Registration?> GetRegistrationByStudentIdAsync(int studentId)
    {
        return await _context.Registrations
            .Include(r => r.Student)
            .FirstOrDefaultAsync(r => r.StudentId == studentId);
    }

    public async Task<IEnumerable<Registration>> GetAllRegistrationsAsync()
    {
        return await _context.Registrations
            .Include(r => r.Student)
            .ToListAsync();
    }

    public async Task<IEnumerable<Registration>> GetRegisteredStudentsAsync()
    {
        return await _context.Registrations
            .Include(r => r.Student)
            .Where(r => r.IsRegistered)
            .ToListAsync();
    }

    public async Task<IEnumerable<Registration>> GetUnregisteredStudentsAsync()
    {
        var registeredStudentIds = await _context.Registrations
            .Where(r => r.IsRegistered)
            .Select(r => r.StudentId)
            .ToListAsync();

        var unregisteredStudents = await _context.Students
            .Where(s => !registeredStudentIds.Contains(s.Id))
            .ToListAsync();

        return unregisteredStudents.Select(s => new Registration
        {
            StudentId = s.Id,
            Student = s,
            IsRegistered = false
        });
    }

    public async Task<Registration> CreateRegistrationAsync(Registration registration)
    {
        registration.CreatedAt = DateTime.Now;
        await _registrationRepository.AddAsync(registration);
        await _registrationRepository.SaveChangesAsync();
        return registration;
    }

    public async Task UpdateRegistrationAsync(Registration registration)
    {
        registration.UpdatedAt = DateTime.Now;
        _registrationRepository.Update(registration);
        await _registrationRepository.SaveChangesAsync();
    }

    public async Task DeleteRegistrationAsync(int id)
    {
        var registration = await _registrationRepository.GetByIdAsync(id);
        if (registration != null)
        {
            _registrationRepository.Delete(registration);
            await _registrationRepository.SaveChangesAsync();
        }
    }

    public async Task RegisterStudentAsync(int studentId, string registrationLocation, string registrar)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException("Student not found");

        var existingRegistration = await _context.Registrations
            .FirstOrDefaultAsync(r => r.StudentId == studentId);

        if (existingRegistration != null && existingRegistration.IsRegistered)
            throw new InvalidOperationException("Student is already registered");

        if (existingRegistration != null)
        {
            existingRegistration.IsRegistered = true;
            existingRegistration.RegistrationDate = DateTime.Now;
            existingRegistration.RegistrationLocation = registrationLocation;
            existingRegistration.Registrar = registrar;
            existingRegistration.UpdatedAt = DateTime.Now;
            _registrationRepository.Update(existingRegistration);
        }
        else
        {
            var newRegistration = new Registration
            {
                StudentId = studentId,
                IsRegistered = true,
                RegistrationDate = DateTime.Now,
                RegistrationLocation = registrationLocation,
                Registrar = registrar,
                CreatedAt = DateTime.Now
            };
            await _registrationRepository.AddAsync(newRegistration);
        }

        await _registrationRepository.SaveChangesAsync();
    }

    public async Task CancelRegistrationAsync(int studentId)
    {
        var registration = await _context.Registrations
            .FirstOrDefaultAsync(r => r.StudentId == studentId);

        if (registration == null)
            throw new KeyNotFoundException("Registration not found");

        if (!registration.IsRegistered)
            throw new InvalidOperationException("Student is not registered");

        registration.IsRegistered = false;
        registration.UpdatedAt = DateTime.Now;
        _registrationRepository.Update(registration);
        await _registrationRepository.SaveChangesAsync();
    }
}