using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySecondMauiApp.Model;
using MySecondMauiApp.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MySecondMauiApp
{
    public partial class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<Rock> Rocks { get; } = new();

        [ObservableProperty]
        private Rock selectedRock;

        public MainPageViewModel()
        {
            Title = "My Rock Collection!";
        }

        [RelayCommand]
        public void AddRockTest()
        {
            Debug.WriteLine("AddRockTest");
            //Generate a rock for testing
            Rock rock = new Rock();
            rock.Name = "My Rock";
            rock.Details = "These are the details of my rock";
            rock.Image = "rock.jpg";

            Rocks.Add(rock);
        }


        [RelayCommand]
        void SelectRockCard(Rock rock)
        {
            SelectedRock = rock;
        }
    }
}
