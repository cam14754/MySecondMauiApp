namespace MySecondMauiApp
{
    public partial class AddPage : ContentPage
    {
        public AddPage(AddPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
