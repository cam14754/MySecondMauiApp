// Copyright (c) 2025 Cameron's Rock Company. 
// PRIVATE AND CONFIDENTIALL INFORMATION 
// Please don't steal my code.

namespace MySecondMauiApp.Controls;

using SQuan.Helpers.Maui.Mvvm;

public partial class ExampleControl : ContentView
{
    // Thanks Stephan 
    [BindableProperty]
    public partial string EnteredName { get; set; } = "DefaultName";

    [BindableProperty]
    public partial bool IsBackwards { get; set; } = false;

    public ExampleControl()
    {
        InitializeComponent();
    }
}
