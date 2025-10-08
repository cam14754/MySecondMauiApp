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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is AddEditViewModel vm)
            {
                // Safe: no exception if it was already completed by Save/Cancel
                vm.Completion?.TrySetResult(null);
            }
        }
    }
}
