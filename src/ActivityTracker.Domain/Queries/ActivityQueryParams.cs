using ActivityTracker.Domain.Enums;

namespace ActivityTracker.Domain.Queries;

public class ActivityQueryParams
{
    public ActivityStatus? Status { get; set; }
    public string? AssignedUserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    private int _page = 1;
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : value > 50 ? 50 : value;
    }
}
