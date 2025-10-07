using CommunityToolkit.Mvvm.ComponentModel;
using MySecondMauiApp.Model;
using MySecondMauiApp.ViewModels;

namespace MySecondMauiApp
{
    [QueryProperty(nameof(Rock), "Rock")]

    public partial class AddPageViewModel : BaseViewModel
    {
        RockDataService rockDataService;
        IGeolocation Geolocation;
        public AddPageViewModel(RockDataService rockDataService, IGeolocation geolocation)
        {
            this.rockDataService = rockDataService;
            this.Geolocation = geolocation;

            rock ??= new Rock();
            rockDataService.AssignGUID(rock);
        }


        //Sync Fusion


        [ObservableProperty]
        Rock rock;



    }
}
