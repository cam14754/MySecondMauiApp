namespace MySecondMauiApp
{
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
}