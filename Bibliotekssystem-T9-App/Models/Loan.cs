namespace Bibliotekssystem_T9_App.Models;

public class Loan
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string BorrowerId { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsReturned { get; set; }
}