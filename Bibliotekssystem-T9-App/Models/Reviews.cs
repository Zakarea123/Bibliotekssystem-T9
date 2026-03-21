namespace Bibliotekssystem_T9_App.Models;

public class BookReviews
{
    public int ReviewId { get; set; }
    public string BookTitle { get; set; }
    public string ReviewerName { get; set; }
    public int Rating { get; set; }
    public string? Text { get; set; }
}