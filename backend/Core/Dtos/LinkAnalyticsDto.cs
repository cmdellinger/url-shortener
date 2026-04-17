namespace Core.Dtos;

public class LinkAnalyticsDto
{
    public int TotalClickEvents { get; set; }
    public int ClickEventsToday { get; set; }
    public int ClickEventsWeek { get; set; }
    public IList<ClicksByDateDto> ClickEvents30Days { get; set; } = [];
    public IList<ReferrerDto> TopReferrer { get; set; } = [];
}
