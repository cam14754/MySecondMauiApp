// Copyright (c) 2025 Cameron's Rock Company. 
// PRIVATE AND CONFIDENTIALL INFORMATION 
// Please don't steal my code.

namespace MySecondMauiApp.Model;

public partial class Rock() : ObservableObject
{
    [ObservableProperty] private string? name;
    [ObservableProperty] private string? description;

    [NotifyPropertyChangedFor(nameof(SpeciesList))]
    [NotifyPropertyChangedFor(nameof(HasRockType))]
    [ObservableProperty] private RockType? type;

    [ObservableProperty] private string? species;
    [ObservableProperty] private string? imageString;

    [NotifyPropertyChangedFor(nameof(HasLocation))]
    [ObservableProperty] private Location? location;

    [ObservableProperty] private DateTime dateTime = DateTime.Today;
    [ObservableProperty] private TimeSpan? time;
    [ObservableProperty] private Guid iD = Guid.NewGuid();

    public Rock Copy(bool newID = false)
    {
        return new Rock()
        {
            Name = this.Name,
            Description = this.Description,
            Type = this.Type,
            Species = this.Species,
            ImageString = this.ImageString,
            Location = this.Location,
            Time = this.Time,
            ID = newID ? Guid.NewGuid() : this.ID,
        };
    }

    public void CopyFrom(Rock other)
    {
        if (other is null)
        {
            return;
        }

        this.Name = other.Name;
        this.Description = other.Description;
        this.Type = other.Type;
        this.Species = other.Species;
        this.ImageString = other.ImageString;
        this.Location = other.Location;
        this.Time = other.Time;
        this.ID = other.ID;
    }


    public List<string> SpeciesList => this.Type switch
    {
        RockType.Igneous => RockSpecies.IgneousRocks,
        RockType.Sedimentary => RockSpecies.SedimentaryRocks,
        RockType.Metamorphic => RockSpecies.MetamorphicRocks,
        //When Type is null (eg, when the page first loads), return an empty list
        _ => [],
    };

    public bool HasLocation => Location is not null;

    public bool HasRockType => Type != null;
}
