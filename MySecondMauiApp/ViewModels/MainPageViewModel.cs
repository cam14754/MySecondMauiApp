// COPYRIGHT © 2025 ESRI
//
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts Dept
// 380 New York Street
// Redlands, California, USA 92373
//
// email: contracts@esri.com
namespace MySecondMauiApp;

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
    Rock? selectedRock;



    [RelayCommand]
    public async Task LoadRocksAsync()
    {
        if (IsBusy)
        {
            return;
        }

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
    async Task EditRockAsync(Rock? rock)
    {
        if (rock is null)
        {
            await Shell.Current.DisplayAlert(
                "No Rock to Edit",
                "Please select a rock to Edit.",
                "OK");
            return;
        }

        Rock rockClone = rock.Copy();

        await GoToAddEditPage(rockClone);
    }

    [RelayCommand]
    async Task GoToAddEditPage(Rock? rock)
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;

        rock ??= new Rock();
        var tcs = new TaskCompletionSource<Rock?>(TaskCreationOptions.RunContinuationsAsynchronously);

        await Shell.Current.GoToAsync(nameof(AddEditPage), true, new Dictionary<string, object?>
            {
                {"Rock", rock},
                {"Completion", tcs }
            });

        var result = await tcs.Task;

        IsBusy = false;

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

        if (!await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {rock.Name}?", "Yes", "No"))
        {
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
                placeholder: $"{Sanitize(rock.Name is null ? "RockName" : rock.Name)}.txt",
                maxLength: 128);


            // Ensure an extension
            var fileName = EnsureValidFileName(input.Trim(), ".txt", rock.ID);

            // Build content
            var content =
$@"My Downloaded Rock!
----
Name: {(rock.Name is null ? "Empty" : rock.Name)}
Type: {(rock.Type is null ? "Empty" : rock.Type)}
Description: {(rock.Description is null ? "Empty" : rock.Description)}
ID: {rock.ID}
Location: {(rock.Location is null ? "Empty" : $"{rock.Location.Latitude}, {rock.Location.Longitude}")}";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

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

    private static string EnsureValidFileName(string? name, string ext, Guid ID)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = $"{ID}";
        }
        name = Path.HasExtension(name) ? name : name + ext;
        return name;
    }

    // Helpers (AI Slop, be warned)
    private static string Sanitize(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sb = new StringBuilder(name.Length);
        foreach (var ch in name)
        {
            sb.Append(Array.IndexOf(invalid, ch) >= 0 ? '_' : ch);
        }

        return sb.ToString();
    }
}

