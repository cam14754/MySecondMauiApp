namespace MySecondMauiApp.Tests;

public class AddEditViewModelTests
{
    [Fact]
    public async Task AddEditViewModel_CanLoadData()
    {
        // Arrange
        var mockDataService = Substitute.For<IRockDataService>();
        var mockMediaPicker = Substitute.For<IMediaPicker>();
        var mockGeolocation = Substitute.For<IGeolocation>();
        var viewModel = new AddEditViewModel(mockDataService, mockGeolocation, mockMediaPicker);

        // Act

        // Assert
    }

}