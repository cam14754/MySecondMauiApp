// SPDX-License-Identifier: Proprietary
// © 2025 Cameron Strachan, trading as Cameron's Rock Company. All rights reserved.
// Created by Cameron Strachan.
// For personal and educational use only.

namespace MySecondMauiApp;

/// <summary>
/// ViewModel for the Add/Edit Rock page.
/// for creating or editing a <see cref="Rock"/>.
/// </summary>
[QueryProperty(nameof(SelectedRock), "SelectedRock")]
[QueryProperty(nameof(Completion), "Completion")]
public partial class AddEditViewModel(IRockDataService rockDataService, IGeolocation geolocation, IMediaPicker mediaPicker) : BaseViewModel
{
    readonly IRockDataService rockDataService = rockDataService;
    readonly IGeolocation geolocation = geolocation;
    readonly IMediaPicker mediaPicker = mediaPicker;

    /// <summary>
    /// Gets or sets the selected rock for editing.
    /// </summary>
    [ObservableProperty]
    Rock? selectedRock;

    /// <summary>
    /// Gets or sets the completion source for the Add/Edit operation.
    /// </summary>
    // TODO: Find a differant way to do this
    public TaskCompletionSource<Rock?>? Completion { get; set; }

    /// <summary>
    /// Setting the bounds for the Min and Max date pickers
    /// </summary>
    public DateTime MinRockDate { get; } = new(1900, 1, 1);
    public DateTime MaxRockDate { get; } = DateTime.Today;


    /// <summary>
    /// Asynchronously allows the user to pick an image from their device and assigns the image path to the selected
    /// rock.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.
    /// </returns>
    [RelayCommand]
    public async Task PickImageAsync()
    {
        try
        {
            FileResult? photo = await mediaPicker.PickPhotoAsync();

            if (photo is null || SelectedRock is null)
            {
                return;
            }

            SelectedRock.ImagePathString = await SavePhoto(photo);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Camera error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// Asynchronously allows the user to take a photo using their device's camera and assigns the image path to the taken photo.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.
    /// </returns>
    [RelayCommand]
    private async Task TakeImageAsync()
    {
        try
        {
            FileResult? photo = await mediaPicker.CapturePhotoAsync();

            if (photo is null)
            {
                return;
            }

            if (SelectedRock is null)
            {
                return;
            }

            SelectedRock.ImagePathString = await SavePhoto(photo);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Camera error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// Asynchronously saves the selected rock and navigates back to the previous page.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.
    /// </returns>
    [RelayCommand]
    public async Task Submit()
    {
        await rockDataService.SaveRock(SelectedRock);

        Completion?.TrySetResult(SelectedRock);

        await GoBackAsync();
    }

    /// <summary>
    /// Asynchronously navigates back to the previous page.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.
    /// </returns>
    static public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    /// <summary>
    /// Asynchronously saves the photo taken.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.
    /// </returns>
    private static async Task<string> SavePhoto(FileResult file)
    {
        string uniqueName = $"img_{Guid.NewGuid()}";
        string destPath = Path.Combine(FileSystem.AppDataDirectory, uniqueName);

        if (File.Exists(destPath))
        {
            File.Delete(destPath);
        }

        using var src = await file.OpenReadAsync();
        using var dest = File.OpenWrite(destPath);
        await src.CopyToAsync(dest);

        return destPath;
    }

    /// <summary>
    /// Asynchronously gets the current location of the device and assigns it to the selected rock.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation.
    /// </returns>
    [RelayCommand]
    public async Task GetRockLocation()
    {
        if (IsBusy || SelectedRock is null)
        {
            return;
        }

        try
        {
            IsBusy = true;
            Location? location;


            location = await geolocation.GetLastKnownLocationAsync();
            location ??= await geolocation.GetLocationAsync(
                new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.High,
                    Timeout = TimeSpan.FromSeconds(30),
                });

            if (location is null)
            {
                Debug.WriteLine("Unable to get location.");
                return;
            }

            Debug.WriteLine($"Found Location: {location.Latitude}, {location.Longitude}");

            SelectedRock.Location = location;
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
