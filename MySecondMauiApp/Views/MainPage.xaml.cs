namespace MySecondMauiApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await (BindingContext as MainPageViewModel)?.LoadRocksAsync();
        }
    }
}
