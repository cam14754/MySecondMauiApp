// SPDX-License-Identifier: Proprietary
// © 2025 Cameron Strachan, trading as Cameron's Rock Company. All rights reserved.
// Created by Cameron Strachan.
// For personal and educational use only.

namespace MySecondMauiApp.Model;

public partial class Rock() : ObservableObject
{
    [ObservableProperty] private string? name;
    [ObservableProperty] private string? description;

    [NotifyPropertyChangedFor(nameof(RockSpeciesList))]
    [NotifyPropertyChangedFor(nameof(HasRockType))]
    [ObservableProperty] private RockTypesEnum? rockType;

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
            RockType = this.RockType,
            Species = this.Species,
            ImageString = this.ImageString,
            Location = this.Location,
            DateTime = this.DateTime,
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
        this.RockType = other.RockType;
        this.Species = other.Species;
        this.ImageString = other.ImageString;
        this.Location = other.Location;
        this.DateTime = other.DateTime;
        this.Time = other.Time;
        this.ID = other.ID;
    }

    public List<RockTypesEnum> RockTypesList =>
    [
        RockTypesEnum.Igneous,
        RockTypesEnum.Sedimentary,
        RockTypesEnum.Metamorphic
    ];

    public List<string> RockSpeciesList => RockType switch
    {
        RockTypesEnum.Igneous => RockSpecies.IgneousRocks,
        RockTypesEnum.Sedimentary => RockSpecies.SedimentaryRocks,
        RockTypesEnum.Metamorphic => RockSpecies.MetamorphicRocks,
        //When Type is null (eg, when the page first loads), return an empty list
        _ => [],
    };

    public bool HasLocation => Location is not null;

    public bool HasRockType => RockType is not null;
}
