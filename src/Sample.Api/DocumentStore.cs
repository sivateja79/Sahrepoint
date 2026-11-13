namespace Sample.Api;

public record DocumentItem(Guid Id, string FileName, DateTimeOffset CreatedAt);
public record CreateDocumentRequest(string FileName);

public static class DocumentValidation
{
    public static bool IsValidName(string? fileName) =>
        !string.IsNullOrWhiteSpace(fileName) &&
        fileName.Length <= 200 &&
        fileName.IndexOfAny(['/', '\\']) < 0;
}
