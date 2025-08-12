using LiteDB;

namespace ThisNotesForYou;

    public static class Endpoints
    {
        public static IResult GetNotes(int? pageSize, ILiteCollection<Note> col)
        {
            var notes = col.Query()
                .OrderByDescending(x => x.CreatedAt)
                .Limit(pageSize ?? 10)
                .ToList();

            var dtos = notes.Select(note => new NoteDto(note)).ToList();
            return Results.Ok(dtos);
        }

        public static IResult CreateNote(CreateNote input, ILiteCollection<Note> col)
        {
            if (string.IsNullOrWhiteSpace(input.Title))
                return Results.BadRequest("Title is required");

            var note = new Note
            {
                Title = input.Title,
                Text = input.Text,
                CreatedAt = DateTime.UtcNow
            };
            col.Insert(note);
            return Results.Created($"/notes/{note.Id}", new NoteDto(note));
        }

        public static IResult DeleteNote(Guid id, ILiteCollection<Note> col)
        {
            if (col.Delete(id))
                return Results.NoContent();
            else
                return Results.NotFound();
        }
    }

