// Copyright (c) 2025 Cameron's Rock Company. 
// PRIVATE AND CONFIDENTIALL INFORMATION 
// Please don't steal my code.

namespace MySecondMauiApp.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string? title;

    [ObservableProperty]
    bool isRefreshing;

    public bool IsNotBusy => !IsBusy;
}
