namespace MySecondMauiApp.Components;

public partial class MainPageButtonComponent : ContentView
{
    public static readonly BindableProperty CustomProperty1Property =
        BindableProperty.Create(
            propertyName: nameof(CustomProperty1),
            returnType: typeof(string),
            declaringType: typeof(MainPageButtonComponent));

    public string CustomProperty1
    {
        get => GetValue(CustomProperty1Property) as string ?? "boo";
        set => SetValue(CustomProperty1Property, value);
    }

    public static readonly BindableProperty CustomProperty2Property =
        BindableProperty.Create(
            propertyName: nameof(CustomProperty2),
            returnType: typeof(int),
            declaringType: typeof(MainPageButtonComponent),
            defaultValue: 69);

    public int CustomProperty2
    {
        get => (int)GetValue(CustomProperty2Property);
        set => SetValue(CustomProperty2Property, value);
    }

    public MainPageButtonComponent()
    {
        InitializeComponent();
    }
}
