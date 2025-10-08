namespace MySecondMauiApp.Model
{

    public partial class Rock : ObservableObject
    {
        [ObservableProperty] private string name;
        [ObservableProperty] private string description;
        [ObservableProperty] private string type;
        [ObservableProperty] private string imageString = "defaultrock.jpg";
        [ObservableProperty] private Location location;
        [ObservableProperty] private Guid iD = Guid.NewGuid();

        public Rock Copy(bool newID = false)
        {
            return new Rock
            {
                Name = this.Name,
                Description = this.Description,
                Type = this.Type,
                ImageString = this.ImageString,
                Location = this.Location,
                ID = newID ? Guid.NewGuid() : this.ID,
            };
        }

        public void CopyFrom(Rock other)
        {
            if (other is null)
                return;
            this.Name = other.Name;
            this.Description = other.Description;
            this.Type = other.Type;
            this.ImageString = other.ImageString;
            this.Location = other.Location;
            this.ID = other.ID;
        }
    }
}
