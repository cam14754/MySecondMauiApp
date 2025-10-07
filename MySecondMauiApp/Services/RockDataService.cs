namespace MySecondMauiApp
{
    public class RockDataService
    {
        public ObservableCollection<Rock> Rocks { get; } = [];



        public RockDataService()
        {

        }

        public void SaveRock(Rock rock)
        {
            if (rock is null)
                return;

            var existingRock = Rocks.FirstOrDefault(r => r.ID == rock.ID);

            if (existingRock is null)
            {
                Rocks.Add(rock);
            }
            else
            {
                existingRock.CopyFrom(rock);
            }
        }

        public bool DeleteRock(Rock rock)
        {
            return Rocks.Remove(rock);
        }
    }
}
