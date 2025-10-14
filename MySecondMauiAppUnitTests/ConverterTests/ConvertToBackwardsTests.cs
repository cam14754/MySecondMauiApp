namespace MySecondMauiApp.Tests.ConverterTests;

public class ConvertToBackwardsTests
{
    [Theory]
    [InlineData("Rock", "kcoR")]
    [InlineData("Stone", "enotS")]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData("A", "A")]
    [InlineData("0", "0")]
    [InlineData(" ", " ")]
    [InlineData("12345", "54321")]
    [InlineData("Aaaaa", "aaaaA")]

    public void ConvertToBackwards_ShouldReturnBackwardsString(string? input, string expected)
    {
        // Arrange
        var converter = Substitute.For<ConvertToBackwards>();
        // Act
        var result = converter.Convert(input, typeof(string), null, null);
        // Assert
        Assert.Equal(expected, result);
    }
}
