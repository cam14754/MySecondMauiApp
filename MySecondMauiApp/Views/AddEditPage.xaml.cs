namespace MySecondMauiApp
{
    public partial class AddEditPage : ContentPage
    {
        public AddEditPage(AddEditViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
        }
    }
}
