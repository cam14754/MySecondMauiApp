// SPDX-License-Identifier: Proprietary
// © 2025 Cameron Strachan, trading as Cameron's Rock Company. All rights reserved.
// Created by Cameron Strachan.
// For personal and educational use only.


namespace MySecondMauiApp.Converters;

/// <summary>
/// Converter class to reverse a string value.
/// </summary>
public partial class ConvertToBackwards : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        string? name = value?.ToString();
        string reverse = new(name?.Reverse().ToArray());
        return reverse;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}
