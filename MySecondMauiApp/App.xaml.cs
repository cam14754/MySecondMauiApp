// SPDX-License-Identifier: Proprietary
// © 2025 Cameron Strachan, trading as Cameron's Rock Company. All rights reserved.
// Created by Cameron Strachan.
// For personal and educational use only.

namespace MySecondMauiApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        const int newHeight = 780;
        const int newWidth = 430;

        var newWindow = new Window(new AppShell())
        {
            Height = newHeight,
            Width = newWidth
        };

        return newWindow;
    }
}
