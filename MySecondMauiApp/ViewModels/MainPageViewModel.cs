namespace MySecondMauiApp
{

    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly RockDataService rockDataService;

        public ObservableCollection<Rock> Rocks => rockDataService.Rocks;

        public MainPageViewModel(RockDataService rockDataService)
        {
            this.rockDataService = rockDataService;
            Title = "My Rock Collection!";
        }

        [RelayCommand]
        async Task GoToAddPageNew(Rock? rock = null)
        {
            rock ??= new Rock();

            await Shell.Current.GoToAsync(nameof(AddPage), true, new Dictionary<string, object>
            {
                {"Rock", rock}
            });
        }

        [RelayCommand]
        async Task DeleteRockAsync(Rock? rock)
        {
            if (rockDataService.DeleteRock(rock))
            {
                await Shell.Current.DisplayAlert(
                    "Rock Deleted",
                    "Your rock has been deleted successfully!",
                    "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert(
                    "No Rock Deleted",
                    "Your rock was not successfully deleted or was not selected",
                    "OK");
            }
        }
    }
}

