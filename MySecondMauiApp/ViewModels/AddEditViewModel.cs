// SPDX-License-Identifier: Proprietary
// © 2025 Cameron Strachan, trading as Cameron's Rock Company. All rights reserved.
// Created by Cameron Strachan.
// For personal and educational use only.

namespace MySecondMauiApp;

[QueryProperty(nameof(SelectedRock), "SelectedRock")]
[QueryProperty(nameof(Completion), "Completion")]


public partial class AddEditViewModel(RockDataService rockDataService, IGeolocation geolocation, IMediaPicker mediaPicker) : BaseViewModel
{
    RockDataService rockDataService = rockDataService;
    IGeolocation geolocation = geolocation;
    IMediaPicker mediaPicker = mediaPicker;

    [ObservableProperty]
    Rock? selectedRock;

    public TaskCompletionSource<Rock?>? Completion { get; set; }

    public DateTime MinRockDate { get; } = new(1900, 1, 1);
    public DateTime MaxRockDate { get; } = DateTime.Today;

    [RelayCommand]
    async Task PickImageAsync()
    {
        try
        {
            FileResult? photo = await mediaPicker.PickPhotoAsync();

            if (photo is null)
            {
                return;
            }

            if (SelectedRock is null)
            {
                return;
            }

            SelectedRock.ImageString = await SavePhoto(photo);
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
            {
                return;
            }

            if (SelectedRock is null)
            {
                return;
            }

            SelectedRock.ImageString = await SavePhoto(photo);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Camera error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    public async Task Submit()
    {
        await rockDataService.SaveRock(SelectedRock);

        Completion?.TrySetResult(SelectedRock);

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
        {
            File.Delete(destPath);
        }

        using var src = await file.OpenReadAsync();
        using var dest = File.OpenWrite(destPath);
        await src.CopyToAsync(dest);

        return destPath;
    }
    [RelayCommand]
    async Task GetRockLocation()
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
            if (location is null)
            {
                location = await geolocation.GetLocationAsync(
                new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.High,
                    Timeout = TimeSpan.FromSeconds(30),
                });
            }

            if (location is null)
            {
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
