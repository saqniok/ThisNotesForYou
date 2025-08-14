namespace ThisNotesForYou.Blazor.Services;

public sealed class Note
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Text { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}