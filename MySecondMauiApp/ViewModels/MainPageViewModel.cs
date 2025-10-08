using System.Text;

namespace MySecondMauiApp
{

    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly RockDataService rockDataService;
        private readonly IFileSaver fileSaver;
        public ObservableCollection<Rock> Rocks => rockDataService.Rocks;

        public MainPageViewModel(RockDataService rockDataService, IFileSaver fileSaver)
        {
            this.rockDataService = rockDataService;
            this.fileSaver = fileSaver;
            Title = "My Rock Collection!";
        }

        [ObservableProperty]
        Rock selectedRock;



        [RelayCommand]
        public async Task LoadRocksAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                await rockDataService.LoadRocksAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load rocks: {ex.Message}", "OK");
            }
            finally
            {
                IsRefreshing = false;
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task EditRockAsync(Rock rock)
        {
            if (rock is null)
            {
                await Shell.Current.DisplayAlert(
                    "No Rock Deleted",
                    "Please select a rock to delete.",
                    "OK");
                return;
            }

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

            if (await rockDataService.DeleteRockAsync(rock))
            {
                await Shell.Current.DisplayAlert(
                    "Rock Deleted",
                    "Your rock has been deleted successfully!",
                    "OK");

                SelectedRock = null;
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

            if (await rockDataService.DuplicateRockAsync(rock))
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

            if (await rockDataService.ChangeNameAsync(rock, name))
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
        async Task DownloadRockAsync(Rock? rock)
        {
            if (rock is null)
            {
                await Shell.Current.DisplayAlert(
                    "No Rock Downloaded",
                    "Please select a rock to download.",
                    "OK");
                return;
            }

            await SaveFileAsync(rock, CancellationToken.None);
        }


        private async Task SaveFileAsync(Rock rock, CancellationToken ct)
        {
            try
            {
                var input = await Shell.Current.DisplayPromptAsync(
                    "Save Rock",
                    "Choose a filename (e.g., my-rock.txt)",
                    accept: "Save",
                    cancel: "Cancel",
                    placeholder: $"{Sanitize(rock.Name)}.txt",
                    maxLength: 128,
                    keyboard: Keyboard.Text);

                if (string.IsNullOrWhiteSpace(input))
                    return; // user cancelled

                // Ensure an extension
                var fileName = EnsureExtension(input.Trim(), ".txt");

                // Build content
                var content =
$@"Rock
----
Name: {rock.Name}
Type: {rock.Type}
Description: {rock.Description}
ID: {rock.ID}
Location: {(rock.Location is null ? "N/A" : $"{rock.Location.Latitude}, {rock.Location.Longitude}")}";

                // Make the stream
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

                // Save (lets user pick a location on Android/Windows/macOS)
                var fileLocationResult = await fileSaver.SaveAsync(fileName, stream, ct);
                fileLocationResult.EnsureSuccess();

                var pathShown = string.IsNullOrWhiteSpace(fileLocationResult.FilePath) ? "Selected location" : fileLocationResult.FilePath;
                await Shell.Current.DisplayAlert("Downloaded Successfully", $"Saved: {pathShown}", "OK");
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Download Unsuccessful", ex.Message, "OK");
            }
        }

        // Helpers
        private static string EnsureExtension(string name, string ext)
            => Path.HasExtension(name) ? name : name + ext;

        private static string Sanitize(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(name.Length);
            foreach (var ch in name)
                sb.Append(Array.IndexOf(invalid, ch) >= 0 ? '_' : ch);
            return sb.ToString();
        }


    }
}

