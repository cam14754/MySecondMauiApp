using MySecondMauiApp.Model;
using System.Collections.ObjectModel;

namespace MySecondMauiApp
{
    public class RockDataService
    {
        public ObservableCollection<Rock> Rocks { get; } = [];

        public RockDataService()
        {

        }


        public void AddTestRock()
        {
            Rock rock = new()
            {
                Name = "My Rock",
                Details = "These are the details of my rock",
                Image = "rock.jpg",
            };
            AssignGUID(rock);
            Rocks.Add(rock);
        }

        public void AssignGUID(Rock rock)
        {
            rock ??= new Rock();

            rock.ID = new Guid();
        }
    }
}
