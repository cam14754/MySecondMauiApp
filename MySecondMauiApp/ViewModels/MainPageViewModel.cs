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

            Rock rockClone = rock.Copy();

            await Shell.Current.GoToAsync(nameof(AddEditPage), true, new Dictionary<string, object>
            {
                {"Rock", rockClone}
            });
        }

        [RelayCommand]
        async Task DeleteRockAsync(Rock? rock)
        {
            if (rock is null)
            {
                await Shell.Current.DisplayAlert(
                    "No Rock Deleted",
                    "Please select a rock to delete.",
                    "OK");
                return;
            }

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
                    "Your rock was not successfully deleted.",
                    "OK");
            }
        }

        [RelayCommand]
        async Task DuplicateRockAsync(Rock? rock)
        {
            if (rock is null)
            {
                await Shell.Current.DisplayAlert(
                    "No Rock Duplicated",
                    "Please select a rock to duplicate.",
                    "OK");
                return;
            }

            if (rockDataService.DuplicateRock(rock))
            {
                await Shell.Current.DisplayAlert(
                    "Rock Duplicated",
                    "Your rock has been duplicated successfully!",
                    "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert(
                    "No Rock Duplicated",
                    "Your rock was not successfully duplicated.",
                    "OK");
            }
        }
        [RelayCommand]
        async Task RenameRock(Rock? rock)
        {
            if (rock is null)
            {
                await Shell.Current.DisplayAlert(
                    "No Rock Renamed",
                    "Please select a rock to rename.",
                    "OK");
                return;
            }

            string? name = await Shell.Current.DisplayPromptAsync("Rename", "Enter new Rock name:");

            if (rockDataService.ChangeName(rock, name))
            {
                await Shell.Current.DisplayAlert(
                    "Rock Renamed",
                    "Your rock has been renamed successfully!",
                    "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert(
                    "No Rock Renamed",
                    "Your rock was not successfully renamed.",
                    "OK");
            }
        }

        [RelayCommand]
        async Task DownloadRock(Rock? rock)
        {
            if (rock is null)
            {
                await Shell.Current.DisplayAlert(
                    "No Rock Downloaded",
                    "Please select a rock to download.",
                    "OK");
                return;
            }
        }
    }
}

