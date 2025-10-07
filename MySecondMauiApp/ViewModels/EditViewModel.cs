using CommunityToolkit.Mvvm.ComponentModel;
using MySecondMauiApp.ViewModels;

namespace MySecondMauiApp
{
    [QueryProperty(nameof(Rock), "Rock")]
    public partial class EditViewModel : BaseViewModel
    {
        public ObservableObject Rock { get; set; }
        public EditViewModel()
        {

        }
    }
}
