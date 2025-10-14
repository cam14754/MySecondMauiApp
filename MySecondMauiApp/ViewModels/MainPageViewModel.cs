// SPDX-License-Identifier: Proprietary
// © 2025 Cameron Strachan, trading as Cameron's Rock Company. All rights reserved.
// Created by Cameron Strachan.
// For personal and educational use only.

namespace MySecondMauiApp;

/// <summary>
/// ViewModel for the main list page. Loads, selects, and manages <see cref="Rock"/> objects,
/// and coordinates navigation, duplication, rename, delete, and download actions.
/// </summary>
public partial class MainPageViewModel : BaseViewModel
{
    private readonly IRockDataService rockDataService;
    private readonly IFileSaver fileSaver;
    /// <summary>
    /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
    /// </summary>
    /// <param name="rockDataService">Handles load/CRUD operations for <see cref="Rock"/> objects.</param>
    /// <param name="fileSaver">Handles user-initiated file exports.</param>
    public MainPageViewModel(IRockDataService rockDataService, IFileSaver fileSaver)
    {
        this.rockDataService = rockDataService;
        this.fileSaver = fileSaver;
        Title = "My Rock Collection!";
    }

    /// <summary>
    /// The currently selected <see cref="Rock"/>.
    /// </summary>
    [ObservableProperty]
    Rock? selectedRock;

    /// <summary>
    /// Gets the collection of available <see cref="Rock"/> objects from the data service.
    /// </summary>
    public ObservableCollection<Rock> Rocks => rockDataService.Rocks;

    /// <summary>
    /// Loads the List of <see cref="Rock"/> objects from memory
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
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

    /// <summary>
    /// Prepares a deep copy of the selected <see cref="Rock"/> for navigates to the <see cref="AddEditPage"/>
    /// </summary>
    /// <param name="rock">The <see cref="Rock"/> to be edited</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
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

        //Make deep copy with same ID, for referancing back later
        Rock rockClone = rock.Copy(false);

        await GoToAddEditPage(rockClone);
    }

    /// <summary>
    /// Asynchronously navigates to the <see cref="AddEditPage"/> to add a new <see cref="Rock"/> or edit an existing one.
    /// </summary>
    /// <param name="rock">The <see cref="Rock"/> to be edited. If the rock is null, create a new one</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
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
                {"SelectedRock", rock },
                {"Completion", tcs }
            });

        var result = await tcs.Task;

        IsBusy = false;
    }

    /// <summary>
    /// Asynchronously deletes the selected <see cref="Rock"/> after user confirmation.
    /// </summary>
    /// <param name="rock">The <see cref="Rock"/> to be deleted.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
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

    /// <summary>
    /// Duplicates the selected <see cref="Rock"/>.
    /// </summary>
    /// <param name="rock">The <see cref="Rock"/> to be duplicated.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
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

    /// <summary>
    /// Renames the selected <see cref="Rock"/>.
    /// </summary>
    /// <param name="rock">The <see cref="Rock"/> to be renamed.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
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

        if (await rockDataService.ChangeRockNameAsync(rock, name))
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

    /// <summary>
    /// Downloads the selected <see cref="Rock"/> as a text file.
    /// </summary>
    /// <param name="rock">The <see cref="Rock"/> to be downloaded.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
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

    /// <summary>
    /// Prompts the user for a filename and saves a simple text export of the given rock.
    /// </summary>
    /// <param name="rock">The rock to export.</param>
    /// <param name="ct">Cancellation token for the save operation.</param>
    private async Task SaveFileAsync(Rock rock, CancellationToken ct)
    {
        try
        {
            var input = await Shell.Current.DisplayPromptAsync(
                title: "Save Rock",
                message: "Choose a filename (e.g., my-rock.txt)",
                accept: "Save",
                cancel: "Cancel",
                placeholder: $"{Sanitize(rock.Name is null ? "RockName" : rock.Name)}.txt",
                maxLength: 128);


            // Ensure an extension
            var fileName = EnsureValidFileName(input.Trim(), ".txt", rock.ID);

            // Build content
            var content = $"""
            My Downloaded Rock!
            ----
            Name: {rock.Name ?? "(empty)"}
            Type: {(rock.RockType is null ? "(empty)" : rock.RockType)}
            Description: {rock.Description ?? "(empty)"}
            ID: {rock.ID}
            Location: {(rock.Location is null ? "(empty)" : $"{rock.Location.Latitude}, {rock.Location.Longitude}")}
            """;

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

    // Helpers 
    private static string EnsureValidFileName(string? name, string ext, Guid ID)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = $"{ID}";
        }
        name = Path.HasExtension(name) ? name : name + ext;
        return name;
    }

    // (AI Slop, be warned)
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

