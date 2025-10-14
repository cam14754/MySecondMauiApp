// SPDX-License-Identifier: Proprietary
// © 2025 Cameron Strachan, trading as Cameron's Rock Company. All rights reserved.
// Created by Cameron Strachan.
// For personal and educational use only.

namespace MySecondMauiApp;

/// <summary>
/// Provides services for managing rock data, including loading, saving, deleting, duplicating, and renaming rocks.
/// </summary>
/// <remarks>
/// Data is persisted as JSON in the application data directory (see <see cref="FileSystem.AppDataDirectory"/>).
/// All public methods are asynchronous to avoid blocking the UI thread in a .NET MAUI application.
/// </remarks>
public class RockDataService : IRockDataService
{
    /// <summary>
    /// The collection of <see cref="Rock"/> objects managed by the service.
    /// </summary>
    public ObservableCollection<Rock> Rocks { get; set; } = [];

    /// <summary>
    /// The filepath where the rock data is stored in JSON format.
    /// </summary>
    private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "rocks.json");

    /// <summary>
    /// Loads rocks from persistent storage into the in-memory <see cref="Rocks"/> collection.
    /// </summary>
    /// <remarks>
    /// If the backing JSON file does not exist, the method returns without modifying the current collection.
    /// Existing items in <see cref="Rocks"/> are cleared before loaded items are added.
    /// Deserialization tolerates a null result (e.g., empty file) and leaves the collection empty.
    /// </remarks>
    /// <returns>A task representing the asynchronous load operation.</returns>
    /// <exception cref="JsonException">Thrown if the JSON is malformed.</exception>
    /// <exception cref="IOException">Thrown on underlying file access errors.</exception>
    public async Task LoadRocksAsync()
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        using var dataStream = File.OpenRead(filePath);
        var rocksList = await JsonSerializer.DeserializeAsync<List<Rock>>(dataStream);
        Rocks.Clear();
        foreach (var rock in rocksList ?? [])
        {
            Rocks.Add(rock);
        }
    }

    /// <summary>
    /// Persists the current <see cref="Rocks"/> collection to disk as JSON.
    /// </summary>
    /// <remarks>
    /// Any exception during save is caught and logged via <see cref="Debug.WriteLine(string?)"/>.
    /// </remarks>
    public async Task SaveRocksAsync()
    {
        try
        {
            using var dataStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(dataStream, Rocks.ToList());
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in saving rock: {ex.Message}");
        }
    }

    /// <summary>
    /// Adds a new rock or updates an existing one (matched by <see cref="Rock.ID"/>) and saves the collection.
    /// </summary>
    /// <param name="rock">The rock to add or update. Ignored if null.</param>
    public async Task SaveRock(Rock? rock)
    {
        if (rock is null)
        {
            return;
        }

        var existingRock = Rocks.FirstOrDefault(r => r.ID == rock.ID);

        if (existingRock is null)
        {
            Rocks.Add(rock);
        }
        else
        {
            existingRock.CopyFrom(rock);
        }

        await SaveRocksAsync();
    }

    /// <summary>
    /// Deletes the specified rock from the collection and persists the change.
    /// </summary>
    /// <param name="rock">The rock to delete.</param>
    /// <returns>True if the rock was removed; otherwise false.</returns>
    public async Task<bool> DeleteRockAsync(Rock rock)
    {
        if (rock is null)
        {
            return false;
        }

        if (!Rocks.Remove(rock))
        {
            return false;
        }

        await SaveRocksAsync();
        return true;
    }

    /// <summary>
    /// Creates a duplicate of the specified rock (with a new <see cref="Guid"/>) and saves the collection.
    /// </summary>
    /// <param name="rock">The rock to duplicate.</param>
    /// <returns>True if duplication succeeded; otherwise false.</returns>
    public async Task<bool> DuplicateRockAsync(Rock rock)
    {
        if (rock is null)
        {
            return false;
        }

        var newRock = rock.Copy(true);
        await SaveRock(newRock);
        await SaveRocksAsync();
        return true;
    }

    /// <summary>
    /// Changes the name of the specified rock and persists the change.
    /// </summary>
    /// <param name="rock">The target rock.</param>
    /// <param name="name">The new name value.</param>
    /// <returns>True if the name was changed; otherwise false.</returns>
    public async Task<bool> ChangeRockNameAsync(Rock rock, string name)
    {
        if (rock is null || name is null)
        {
            return false;
        }

        rock.Name = name;
        await SaveRocksAsync();
        return true;
    }
}
