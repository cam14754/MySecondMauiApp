namespace MySecondMauiApp.Model
{

    public partial class Rock : ObservableObject
    {
        [ObservableProperty] private string name;
        [ObservableProperty] private string description;
        [ObservableProperty] private string type;
        [ObservableProperty] private string imageString;
        [ObservableProperty] private Location location;
        [ObservableProperty] private Guid iD = Guid.NewGuid();
    }
}
