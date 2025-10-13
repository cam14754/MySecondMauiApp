namespace MySecondMauiApp.Tests;

public class MainViewModelTests
{
    [Theory]
    [InlineData(1, 1, 3)]
    public void Test1(int a, int b, int expected)
    {
        var sum = a + b;

        Assert.Equal(expected, sum);
    }
}