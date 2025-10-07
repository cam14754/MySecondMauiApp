namespace MySecondMauiApp
{
    public class RockDataService
    {
        public ObservableCollection<Rock> Rocks { get; } = [];



        public RockDataService()
        {

        }

        public void AddRock(Rock rock)
        {
            Rock? foundRock = null;
            try
            {
                foundRock = Rocks.First(r => r.ID == rock.ID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (foundRock is not null)
                {
                    rock = foundRock;
                }
                else
                {
                    // Add a new rock
                    Rocks.Add(rock);
                }
            }
        }

        public bool DeleteRock(Rock rock)
        {
            return Rocks.Remove(rock);
        }
    }
}
