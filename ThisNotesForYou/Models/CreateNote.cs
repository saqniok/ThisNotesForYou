namespace ThisNotesForYou.Models;
public class CreateNote
{
    public string Title { get; }
    public string Text { get; }

    public CreateNote(string title, string text)
    {
        Title = title;
        Text = text;
    }
}