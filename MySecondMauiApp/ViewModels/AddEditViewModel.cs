namespace MySecondMauiApp
{
    [QueryProperty(nameof(Rock), "Rock")]

    public partial class AddEditViewModel(RockDataService rockDataService, IGeolocation geolocation) : BaseViewModel
    {
        RockDataService rockDataService = rockDataService;
        IGeolocation geolocation = geolocation;

        [ObservableProperty]
        List<string> rockTypes = new()
        {
            "Igneous",
            "Sedimentary",
            "Metamorphic"
        };

        [ObservableProperty]
        Rock rock;

        [RelayCommand]
        public async Task Submit()
        {
            Rock.ImageString = "rock.jpg";
            rockDataService.AddRock(Rock);
            await GoBackAsync();
        }

        static public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..", true);
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
