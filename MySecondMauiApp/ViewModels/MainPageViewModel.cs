using CommunityToolkit.Mvvm.Input;
using MySecondMauiApp.Model;
using MySecondMauiApp.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MySecondMauiApp
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly RockDataService rockDataService;

        public ObservableCollection<Rock> Rocks => rockDataService.Rocks;

        public MainPageViewModel(RockDataService rockDataService)
        {
            this.rockDataService = rockDataService;
            Title = "My Rock Collection!";
        }

        [RelayCommand]
        public void AddRockTest()
        {
            Debug.WriteLine("AddRockTest");
            rockDataService.AddTestRock();
        }

        [RelayCommand]
        async Task GoToAddPage()
        {
            await Shell.Current.GoToAsync(nameof(AddPage), true, new Dictionary<string, object>
            {
                {"Rock", new Rock()}
            });
        }
    }
}

