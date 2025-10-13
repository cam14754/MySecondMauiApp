namespace MySecondMauiApp.Tests;

public class AddEditViewModelTests
{
    [Fact]
    public async Task PickImageAsyncTest()
    {
        // Arrange
        var mockDataService = Substitute.For<IRockDataService>();
        var mockMediaPicker = Substitute.For<IMediaPicker>();
        var mockGeolocation = Substitute.For<IGeolocation>();
        var viewModel = new AddEditViewModel(mockDataService, mockGeolocation, mockMediaPicker);

        viewModel.SelectedRock = new Rock();

        mockMediaPicker.PickPhotoAsync().Returns(Task.FromResult<FileResult?>(new FileResult("Default.jpg", "")));

        // Act
        await viewModel.PickImageAsync();

        // Assert
        viewModel.SelectedRock?.ImagePathString?.Should().NotBeNullOrEmpty();
        viewModel.SelectedRock?.ImagePathString?.Should().Be("NotCorrect");
    }

    [Fact]
    public async Task PickImageAsyncTest_2()
    {
        // Arrange
        var mockDataService = Substitute.For<IRockDataService>();
        var mockMediaPicker = Substitute.For<IMediaPicker>();
        var mockGeolocation = Substitute.For<IGeolocation>();
        var viewModel = new AddEditViewModel(mockDataService, mockGeolocation, mockMediaPicker);

        mockMediaPicker.PickPhotoAsync().Returns(Task.FromResult<FileResult?>(new FileResult("Default.jpg", "")));

        // Act
        await viewModel.PickImageAsync();

        // Assert
        viewModel.SelectedRock?.ImagePathString?.Should().NotBeNullOrEmpty();
    }
}