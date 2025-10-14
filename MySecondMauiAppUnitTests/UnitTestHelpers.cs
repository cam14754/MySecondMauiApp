using MySecondMauiApp;

public static class UnitTestHelpers
{
    public static (AddEditViewModel viewModel, IMediaPicker mockMediaPicker, IRockDataService mockDataService, IGeolocation mockGeolocation) GenerateViewModelWithRock()
    {
        var mockDataService = Substitute.For<IRockDataService>();
        var mockMediaPicker = Substitute.For<IMediaPicker>();
        var mockGeolocation = Substitute.For<IGeolocation>();

        var vm = new AddEditViewModel(mockDataService, mockGeolocation, mockMediaPicker)
        {
            SelectedRock = new Rock()
        };
        return (vm, mockMediaPicker, mockDataService, mockGeolocation);
    }

    public static FileResult CreateFileResult(object? imagePath, object? contentType)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        return new FileResult((string?)imagePath, (string?)contentType);
    }

    public static string GeneratePath()
    {
        return Path.GetTempPath();
    }
}