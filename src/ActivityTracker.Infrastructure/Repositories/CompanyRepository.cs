using ActivityTracker.Domain.Entities;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Company>> GetAllAsync() =>
        await _context.Companies
            .OrderBy(c => c.Name)
            .ToListAsync();

    public async Task<Company?> GetByIdAsync(int id) =>
        await _context.Companies.FindAsync(id);

    public async Task<Company?> GetByCodeAsync(string code) =>
        await _context.Companies
            .FirstOrDefaultAsync(c => c.Code == code.ToUpper());

    public async Task<Company> CreateAsync(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<Company> UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company is null) return false;
        company.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
