using LiteDB;
using ThisNotesForYou;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ILiteDatabase>(_ => new LiteDatabase("Filename=notes.db;Connection=shared"));
builder.Services.AddSingleton(sp =>
{
    var db = sp.GetRequiredService<ILiteDatabase>();
    var col = db.GetCollection<Note>("notes");
    col.EnsureIndex(x => x.CreatedAt);
    col.EnsureIndex(x => x.Id);
    return col;
});

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

var notes = app.MapGroup("/notes");
notes.MapGet("/", (int? pageSize, ILiteCollection<Note> col) => Endpoints.GetNotes(pageSize, col));
notes.MapPost("/", (CreateNote input, ILiteCollection<Note> col) => Endpoints.CreateNote(input, col));
notes.MapDelete("/{id:guid}", (Guid id, ILiteCollection<Note> col) => Endpoints.DeleteNote(id, col));

app.Run();