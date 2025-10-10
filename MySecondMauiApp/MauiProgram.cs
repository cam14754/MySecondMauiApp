// Copyright (c) 2025 Cameron's Rock Company. 
// PRIVATE AND CONFIDENTIALL INFORMATION 
// Please don't steal my code.

namespace MySecondMauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();


        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddTransient<AddEditViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<AddEditPage>();

        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IMap>(Map.Default);
        builder.Services.AddSingleton<RockDataService>();
        builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
        builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
