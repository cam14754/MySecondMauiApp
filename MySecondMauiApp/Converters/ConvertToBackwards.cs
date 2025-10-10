// Copyright (c) 2025 Cameron's Rock Company. 
// PRIVATE AND CONFIDENTIALL INFORMATION 
// Please don't steal my code.


namespace MySecondMauiApp.Converters;
public partial class ConvertToBackwards : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string? name = value.ToString();
        return name != null ? new string(name.Reverse().ToArray()) : null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
