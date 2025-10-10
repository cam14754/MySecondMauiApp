// Copyright (c) 2025 Cameron's Rock Company. 
// PRIVATE AND CONFIDENTIALL INFORMATION 
// Please don't steal my code.

namespace MySecondMauiApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(AddEditPage), typeof(AddEditPage));
    }
}
