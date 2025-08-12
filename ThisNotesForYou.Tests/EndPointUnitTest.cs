using LiteDB;
using Microsoft.AspNetCore.Http.HttpResults;
using ThisNotesForYou;


public class EndpointUnitTests
{
    [Fact]
    public async Task GetNotes_Returns_Ok_With_Items()
    {
        var (db, col, path) = NewCollection();
        try
        {
            for (int i = 0; i < 12; i++)
                col.Insert(new Note { Title = $"Note {i}", Text = "t" });

            var result = Endpoints.GetNotes(5, col);

            Assert.IsType<Ok<List<NoteDto>>>(result);
            var list = result.Value!;
            Assert.Equal(5, list.Count);
        }
        finally { db.Dispose(); TryDelete(path); }
    }

    [Fact]
    public void CreateNote_BadRequest_When_Title_Missing()
    {
        var (db, col, path) = NewCollection();
        try
        {
            var result = Endpoints.CreateNote(new CreateNote("", "x"), col);
            Assert.IsType<BadRequest<string>>(result);
        }
        finally { db.Dispose(); TryDelete(path); }
    }

    [Fact]
    public void DeleteNote_NoContent_When_Found()
    {
        var (db, col, path) = NewCollection();
        try
        {
            var note = new Note { Title = "t", Text = "x" };
            col.Insert(note);
            var result = Endpoints.DeleteNote(note.Id, col);
            Assert.IsType<NoContent>(result);
        }
        finally { db.Dispose(); TryDelete(path); }
    }

    private static (ILiteDatabase, ILiteCollection<Note>, string Path) NewCollection()
    {
        var path = Path.Combine(Path.GetTempPath(), $"notes_{Guid.NewGuid():N}.db");
        var db = new LiteDatabase($"Filename={path};Connection=shared");
        var col = db.GetCollection<Note>("notes");
        col.EnsureIndex(x => x.CreatedAt);
        return (db, col, path);
    }

    private static void TryDelete(string path)
    {
        try { if (File.Exists(path)) File.Delete(path); } catch { }
    }
}