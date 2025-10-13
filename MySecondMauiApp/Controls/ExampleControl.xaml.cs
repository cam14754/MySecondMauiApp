// SPDX-License-Identifier: Proprietary
// © 2025 Cameron Strachan, trading as Cameron's Rock Company. All rights reserved.
// Created by Cameron Strachan.
// For personal and educational use only.

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
