namespace MySecondMauiApp
{
    [QueryProperty(nameof(Rock), "Rock")]

    public partial class AddPageViewModel(RockDataService rockDataService, IGeolocation geolocation) : BaseViewModel
    {
        RockDataService rockDataService = rockDataService;
        IGeolocation Geolocation = geolocation;

        [ObservableProperty]
        Rock rock;

        [RelayCommand]
        public async Task Submit()
        {
            Rock.ImageString = "rock.jpg";
            rockDataService.AddRock(Rock);
            await GoBackAsync();
        }

        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..", true);
        }
    }
}
