using ActivityTracker.Domain.Entities;
using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Queries;
using ActivityTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Infrastructure.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly AppDbContext _context;

    public ActivityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Activity> Items, int TotalCount)> GetAllAsync(ActivityQueryParams q)
    {
        var query = _context.Activities.AsQueryable();

        if (q.Status.HasValue)
            query = query.Where(a => a.Status == q.Status.Value);

        if (!string.IsNullOrWhiteSpace(q.AssignedUserId))
            query = query.Where(a => a.AssignedUserId.Contains(q.AssignedUserId));

        if (q.StartDate.HasValue)
        {
            var start = DateTime.SpecifyKind(q.StartDate.Value.Date, DateTimeKind.Utc);
            query = query.Where(a => a.ScheduledStart >= start);
        }

        if (q.EndDate.HasValue)
        {
            var end = DateTime.SpecifyKind(q.EndDate.Value.Date.AddDays(1), DateTimeKind.Utc);
            query = query.Where(a => a.ScheduledStart < end);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Activity?> GetByIdAsync(int id) =>
        await _context.Activities.FindAsync(id);

    public async Task<Activity> CreateAsync(Activity activity)
    {
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task<Activity> UpdateAsync(Activity activity)
    {
        _context.Activities.Update(activity);
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity is null) return false;
        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
        return true;
    }
}
