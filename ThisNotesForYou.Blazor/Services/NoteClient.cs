using System.Net.Http.Json;

namespace ThisNotesForYou.Blazor.Services;

public sealed class NotesClient
{
    private readonly HttpClient _http;

    public NotesClient(HttpClient http) => _http = http;

    public Task<List<Note>> GetNotes()
        => _http.GetFromJsonAsync<List<Note>>("notes")!;

    public async Task AddNote(CreateNoteRequest req)
    {
        var resp = await _http.PostAsJsonAsync("notes", req);
        resp.EnsureSuccessStatusCode();
    }

    public async Task DeleteNote(Guid id)
    {
        var resp = await _http.DeleteAsync($"notes/{id}");
        resp.EnsureSuccessStatusCode();
    }
}
