namespace ThisNotesForYou.Blazor.Services;
public sealed class CreateNoteRequest
{
    public string Title { get; set; } = "";
    public string Text { get; set; } = "";
}