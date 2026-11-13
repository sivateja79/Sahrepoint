using Sample.Api;
using Xunit;

namespace Sample.Api.Tests;

public class DocumentValidationTests
{
    [Theory]
    [InlineData("policy.pdf")]
    [InlineData("claim-2026.docx")]
    public void AcceptsValidFileNames(string name) =>
        Assert.True(DocumentValidation.IsValidName(name));

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("../secret.txt")]
    [InlineData("folder/file.pdf")]
    public void RejectsInvalidFileNames(string name) =>
        Assert.False(DocumentValidation.IsValidName(name));
}
