namespace MySecondMauiApp.Tests.ViewModelTests;

public class AddEditViewModelTests
{


    [Theory]
    [InlineData(1d, 2d)]
    [InlineData(0d, 2d)]
    [InlineData(1d, 0d)]
    [InlineData(-20d, -20d)]
    [InlineData(0d, 0d)]
    public async Task GetRockLocationTests_SaveDataCorrectly(object? latitude, object? longitude)
    {
        // Arrange
        var (viewModel, _, _, mockGeolocation) = UnitTestHelpers.GenerateAddEditViewModelWithRock();
#pragma warning disable CS8605 //Unboxing a possibly null value.
        var loc = new Location((double)latitude, (double)longitude);

        mockGeolocation.GetLastKnownLocationAsync().Returns(loc);

        // Act
        await viewModel.GetRockLocation();

        // Assert
        await mockGeolocation.Received(1).GetLastKnownLocationAsync();
        Assert.Equal(loc, viewModel.SelectedRock?.Location);
        viewModel.SelectedRock!.Location.Should().NotBeNull();
    }
}