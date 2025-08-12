namespace ThisNotesForYou;

public class NoteDto
{
    public Guid Id { get; }
    public string Title { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; }

    public NoteDto(Note note)
    {
        Id = note.Id;
        Title = note.Title;
        Text = note.Text;
        CreatedAt = note.CreatedAt;
    }
}