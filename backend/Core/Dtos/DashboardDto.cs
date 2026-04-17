namespace Core.Dtos;

public class DashboardDto
{
    public int TotalLinks { get; set; }  
    public int TotalClicks { get; set; }
    public int ClicksToday { get; set; }
    public IList<LinkDto> TopLinks { get; set; } = [];
}