namespace MySecondMauiApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddEditPage), typeof(AddEditPage));
        }
    }
}
