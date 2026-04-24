using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Repositories;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class AdmissionService : IAdmissionService
{
    private readonly IRepository<Admission> _admissionRepository;
    private readonly ApplicationDbContext _context;

    public AdmissionService(
        IRepository<Admission> admissionRepository,
        ApplicationDbContext context)
    {
        _admissionRepository = admissionRepository;
        _context = context;
    }

    public async Task<Admission?> GetAdmissionByIdAsync(int id)
    {
        return await _context.Admissions
            .Include(a => a.Student)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Admission?> GetAdmissionByStudentIdAsync(int studentId)
    {
        return await _context.Admissions
            .Include(a => a.Student)
            .FirstOrDefaultAsync(a => a.StudentId == studentId);
    }

    public async Task<Admission?> GetAdmissionByAdmissionNumberAsync(string admissionNumber)
    {
        return await _context.Admissions
            .Include(a => a.Student)
            .FirstOrDefaultAsync(a => a.AdmissionNumber == admissionNumber);
    }

    public async Task<IEnumerable<Admission>> GetAllAdmissionsAsync()
    {
        return await _context.Admissions
            .Include(a => a.Student)
            .ToListAsync();
    }

    public async Task<IEnumerable<Admission>> GetPublishedAdmissionsAsync()
    {
        return await _context.Admissions
            .Include(a => a.Student)
            .Where(a => a.IsPublished)
            .ToListAsync();
    }

    public async Task<IEnumerable<Admission>> GetUnpublishedAdmissionsAsync()
    {
        return await _context.Admissions
            .Include(a => a.Student)
            .Where(a => !a.IsPublished)
            .ToListAsync();
    }

    public async Task<Admission> CreateAdmissionAsync(Admission admission)
    {
        admission.CreatedAt = DateTime.Now;
        await _admissionRepository.AddAsync(admission);
        await _admissionRepository.SaveChangesAsync();
        return admission;
    }

    public async Task UpdateAdmissionAsync(Admission admission)
    {
        admission.UpdatedAt = DateTime.Now;
        _admissionRepository.Update(admission);
        await _admissionRepository.SaveChangesAsync();
    }

    public async Task DeleteAdmissionAsync(int id)
    {
        var admission = await _admissionRepository.GetByIdAsync(id);
        if (admission != null)
        {
            _admissionRepository.Delete(admission);
            await _admissionRepository.SaveChangesAsync();
        }
    }

    public async Task PublishAdmissionAsync(int admissionId)
    {
        var admission = await _admissionRepository.GetByIdAsync(admissionId);
        if (admission == null)
            throw new KeyNotFoundException("Admission not found");

        if (admission.IsPublished)
            return;

        admission.IsPublished = true;
        admission.PublishDate = DateTime.Now;
        admission.UpdatedAt = DateTime.Now;
        _admissionRepository.Update(admission);
        await _admissionRepository.SaveChangesAsync();
    }

    public async Task PrintAdmissionNoticeAsync(int admissionId)
    {
        var admission = await _admissionRepository.GetByIdAsync(admissionId);
        if (admission == null)
            throw new KeyNotFoundException("Admission not found");

        if (admission.IsNoticePrinted)
            return;

        admission.IsNoticePrinted = true;
        admission.NoticePrintDate = DateTime.Now;
        admission.UpdatedAt = DateTime.Now;
        _admissionRepository.Update(admission);
        await _admissionRepository.SaveChangesAsync();
    }

    public async Task PrintMailingLabelAsync(int admissionId)
    {
        var admission = await _admissionRepository.GetByIdAsync(admissionId);
        if (admission == null)
            throw new KeyNotFoundException("Admission not found");

        if (admission.IsMailingLabelPrinted)
            return;

        admission.IsMailingLabelPrinted = true;
        admission.MailingLabelPrintDate = DateTime.Now;
        admission.UpdatedAt = DateTime.Now;
        _admissionRepository.Update(admission);
        await _admissionRepository.SaveChangesAsync();
    }

    public async Task GenerateAdmissionNumberAsync(int admissionId)
    {
        var admission = await _admissionRepository.GetByIdAsync(admissionId);
        if (admission == null)
            throw new KeyNotFoundException("Admission not found");

        if (!string.IsNullOrEmpty(admission.AdmissionNumber))
            return;

        var year = DateTime.Now.Year.ToString();
        var maxAdmissionNumber = await _context.Admissions
            .Where(a => a.AdmissionNumber.StartsWith(year))
            .Select(a => a.AdmissionNumber)
            .OrderByDescending(a => a)
            .FirstOrDefaultAsync();

        int sequence = 1;
        if (!string.IsNullOrEmpty(maxAdmissionNumber) && maxAdmissionNumber.Length > 4)
        {
            var sequencePart = maxAdmissionNumber.Substring(4);
            if (int.TryParse(sequencePart, out var seq))
            {
                sequence = seq + 1;
            }
        }

        admission.AdmissionNumber = $"{year}{sequence:D8}";
        admission.UpdatedAt = DateTime.Now;
        _admissionRepository.Update(admission);
        await _admissionRepository.SaveChangesAsync();
    }
}