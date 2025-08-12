using LiteDB;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ThisNotesForYou;

public static class Endpoints
{
    public static Task<Ok<List<NoteDto>>> GetNotes(int? pageSize, ILiteCollection<Note> col)
    {
        if (pageSize <= 0)
            throw new ArgumentException("pageSize must be positive", nameof(pageSize));

        var notes = col.Query()
            .OrderByDescending(x => x.CreatedAt)
            .Limit(pageSize ?? 10)
            .ToList();

        var dtos = notes.Select(note => new NoteDto(note)).ToList();

        return Task.FromResult(TypedResults.Ok(dtos));
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