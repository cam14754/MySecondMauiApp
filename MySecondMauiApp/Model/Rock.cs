// SPDX-License-Identifier: Proprietary
// © 2025 Cameron Strachan, trading as Cameron's Rock Company. All rights reserved.
// Created by Cameron Strachan.
// For personal and educational use only.

namespace MySecondMauiApp.Model;

/// <summary> 
/// Class <c>Rock</c> represents a rock in the rock collection app. 
/// </summary>
public partial class Rock() : ObservableObject
{
    /// <summary>
    /// Gets or sets the name of the rock, of type <see cref="string"/>.
    /// </summary>
    [ObservableProperty] private string? name;

    /// <summary>
    /// Gets or sets the description of the rock, of type <see cref="string"/>.
    /// </summary>
    [ObservableProperty] private string? description;

    /// <summary>
    /// Gets or sets the type of the rock, of type <see cref="RockTypesEnum"/>.
    /// </summary>
    [NotifyPropertyChangedFor(nameof(RockSpeciesList))]
    [NotifyPropertyChangedFor(nameof(HasRockType))]
    [ObservableProperty] private RockTypesEnum? rockType;

    /// <summary>
    /// Gets or sets the species of the rock, of type <see cref="string"/>.
    /// </summary>
    [ObservableProperty] private string? species;

    /// <summary>
    /// Gets or sets the path location of the image, of type <see cref="string"/>.
    /// </summary>
    [ObservableProperty] private string? imagePathString;

    /// <summary>
    /// Gets or sets the location of the rock, of type <see cref="Location"/>.
    /// </summary>
    [NotifyPropertyChangedFor(nameof(HasLocation))]
    [ObservableProperty] private Location? location;

    /// <summary>
    /// Gets or sets the date the rock was found, of type <see cref="DateTime"/>.
    /// </summary>
    [ObservableProperty] private DateTime dateTime = DateTime.Today;

    /// <summary>
    /// Gets or sets the time the rock was found, of type <see cref="TimeSpan"/>.
    /// </summary>
    [ObservableProperty] private TimeSpan? time;

    /// <summary>
    /// Gets or sets the unique identifier for the rock, of type <see cref="Guid"/>.
    /// Has a default value of a <see cref="Guid.NewGuid"/>.
    /// </summary>
    [ObservableProperty] private Guid iD = Guid.NewGuid();


    /// <summary> 
    /// This method creates a copy of the current <see cref="Rock"/> instance. 
    /// </summary> 
    /// <param name="newID"> 
    /// If <see langword="true"/>, a new <see cref="Guid"/> will be assigned to the <see cref="ID"/> property; otherwise, the existing ID will be used. 
    /// </param>
    /// <remarks>
    /// Generally is it safer to assign a new ID to the rock, so newID defaults to <see langword="true"/>.
    /// </remarks>
    /// <returns> 
    /// A new instance of <see cref="Rock"/> with the same property values as the current instance. 
    /// </returns>
    public Rock Copy(bool newID = true)
    {
        return new Rock()
        {
            Name = this.Name,
            Description = this.Description,
            RockType = this.RockType,
            Species = this.Species,
            ImagePathString = this.ImagePathString,
            Location = this.Location,
            DateTime = this.DateTime,
            Time = this.Time,
            ID = newID ? Guid.NewGuid() : this.ID,
        };
    }

    /// <summary> 
    /// Copies the properties from another <see cref="Rock"/> instance to this instance. 
    /// </summary> 
    /// <param name="other"> 
    /// The other <see cref="Rock"/> instance from which to copy properties. 
    /// </param>
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
        this.ImagePathString = other.ImagePathString;
        this.Location = other.Location;
        this.DateTime = other.DateTime;
        this.Time = other.Time;
        this.ID = other.ID;
    }

    /// <summary>
    /// Returns the list of available rock types, from the <see cref="RockTypesEnum"/>.
    /// </summary>
    public List<RockTypesEnum> RockTypesList =>
    [
        RockTypesEnum.Igneous,
        RockTypesEnum.Sedimentary,
        RockTypesEnum.Metamorphic
    ];


    /// <summary>
    /// Returns the list of available rock species based on the selected <see cref="RockType"/>.
    /// </summary>
    /// <remarks>
    /// When Type is null (eg, when the page first loads), return an empty list
    /// </remarks>
    public IReadOnlyList<string> RockSpeciesList => RockType switch
    {
        RockTypesEnum.Igneous => RockSpecies.IgneousRocks,
        RockTypesEnum.Sedimentary => RockSpecies.SedimentaryRocks,
        RockTypesEnum.Metamorphic => RockSpecies.MetamorphicRocks,
        _ => [],
    };


    /// <summary>
    /// Returns true if <see cref="Location"/> exists; otherwise, false.
    /// </summary>
    public bool HasLocation => Location is not null;

    /// <summary>
    /// Returns true if <see cref="RockType"/> exists; otherwise, false.
    /// </summary>
    public bool HasRockType => RockType is not null;
}
