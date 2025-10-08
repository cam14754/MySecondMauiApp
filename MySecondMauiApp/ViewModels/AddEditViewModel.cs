namespace MySecondMauiApp
{
    [QueryProperty(nameof(Rock), "Rock")]
    [QueryProperty(nameof(Completion), "Completion")]


    public partial class AddEditViewModel(RockDataService rockDataService, IGeolocation geolocation, IMediaPicker mediaPicker) : BaseViewModel
    {

        RockDataService rockDataService = rockDataService;
        IGeolocation geolocation = geolocation;
        IMediaPicker mediaPicker = mediaPicker;

        [ObservableProperty]
        List<string> rockTypes = new()
        {
            "Igneous",
            "Sedimentary",
            "Metamorphic"
        };

        [ObservableProperty]
        Rock rock;

        public TaskCompletionSource<Rock?>? Completion { get; set; }




        [RelayCommand]
        async Task PickImageAsync()
        {
            try
            {
                FileResult? photo = await mediaPicker.PickPhotoAsync();

                if (photo is null)
                    return;

                Rock.ImageString = await SavePhoto(photo);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Camera error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task TakeImageAsync()
        {
            try
            {
                FileResult? photo = await mediaPicker.CapturePhotoAsync();

                if (photo is null)
                    return;

                Rock.ImageString = await SavePhoto(photo);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Camera error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task Submit()
        {
            await rockDataService.SaveRock(Rock);

            Completion?.TrySetResult(Rock);

            await GoBackAsync();
        }

        static public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        private async Task<string> SavePhoto(FileResult file)
        {
            string uniqueName = $"img_{Guid.NewGuid()}";
            string destPath = Path.Combine(FileSystem.AppDataDirectory, uniqueName);

            if (File.Exists(destPath))
                File.Delete(destPath);

            using var src = await file.OpenReadAsync();
            using var dest = File.OpenWrite(destPath);
            await src.CopyToAsync(dest);

            return destPath;
        }
        [RelayCommand]
        async Task GetRockLocation()
        {
            Debug.WriteLine("Get rock location run");
            if (IsBusy || rock is null)
                return;
            try
            {
                var location = await geolocation.GetLastKnownLocationAsync();
                location ??= await geolocation.GetLocationAsync(
                        new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(30)
                        });

                if (location is null)
                    return;

                Debug.WriteLine($"Found Location: {location.Latitude}, {location.Longitude}");

                rock.Location = location;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {

                IsBusy = false;
            }
        }
    }
}
