namespace Bibliotekssystem_T9_App.Models;

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public string? Identifier { get; set; }
    public bool IsActive { get; set; }
    public string ItemType { get; set; } = "";
    public int ItemTypeId { get; set; }
}