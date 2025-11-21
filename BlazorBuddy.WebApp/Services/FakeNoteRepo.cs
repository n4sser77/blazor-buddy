
using BlazorBuddy.Models;

namespace BlazorBuddy.WebApp.Services;

public class FakeNoteRepo
{
    private readonly List<NoteDocument> _NoteDocuments = [
        new NoteDocument(){
            Owner= new UserProfile("testuser"),
            Title = "Test note",
            Id= Guid.NewGuid()
        },
    ];

    public Task<List<NoteDocument>> GetAllNoteDocumentsAsync()
    {
        return Task.FromResult(_NoteDocuments.ToList());
    }

    public Task<NoteDocument> GetNoteDocumentByIdAsync(Guid id)
    {
        var NoteDocument = _NoteDocuments.FirstOrDefault(n => n.Id == id);
        return Task.FromResult(NoteDocument!);
    }

    public Task AddNoteDocumentAsync(NoteDocument NoteDocument)
    {
        _NoteDocuments.Add(NoteDocument);
        return Task.CompletedTask;
    }

    public Task UpdateNoteDocumentAsync(NoteDocument NoteDocument)
    {
        var existing = _NoteDocuments.FirstOrDefault(n => n.Id == NoteDocument.Id);
        if (existing != null)
        {
            existing.Title = NoteDocument.Title;
            existing.Content = NoteDocument.Content;
        }
        return Task.CompletedTask;
    }

    public Task DeleteNoteDocumentAsync(Guid id)
    {
        var NoteDocument = _NoteDocuments.FirstOrDefault(n => n.Id == id);
        if (NoteDocument != null) _NoteDocuments.Remove(NoteDocument);
        return Task.CompletedTask;
    }
}



