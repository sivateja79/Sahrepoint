using System.Collections.Concurrent;
using Sample.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var documents = new ConcurrentDictionary<Guid, DocumentItem>();

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "sample-api" }));
app.MapGet("/api/documents", () => Results.Ok(documents.Values.OrderBy(x => x.CreatedAt)));
app.MapGet("/api/documents/{id:guid}", (Guid id) =>
    documents.TryGetValue(id, out var doc) ? Results.Ok(doc) : Results.NotFound());
app.MapPost("/api/documents", (CreateDocumentRequest request) =>
{
    if (!DocumentValidation.IsValidName(request.FileName))
        return Results.BadRequest(new { error = "A valid file name (1–200 characters) is required." });
    var document = new DocumentItem(Guid.NewGuid(), request.FileName, DateTimeOffset.UtcNow);
    documents[document.Id] = document;
    return Results.Created($"/api/documents/{document.Id}", document);
});

app.Run();
