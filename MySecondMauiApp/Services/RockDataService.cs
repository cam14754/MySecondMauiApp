// COPYRIGHT © 2025 ESRI
//
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts Dept
// 380 New York Street
// Redlands, California, USA 92373
//
// email: contracts@esri.com

using System.Text.Json;

namespace MySecondMauiApp;

public class RockDataService
{
    public ObservableCollection<Rock> Rocks { get; } = [];

    private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "rocks.json");

    public RockDataService()
    {

    }

    public async Task LoadRocksAsync()
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        var dataStream = File.OpenRead(filePath);
        var rocksList = await JsonSerializer.DeserializeAsync<List<Rock>>(dataStream);
        Rocks.Clear();
        foreach (var rock in rocksList ?? [])
        {
            Rocks.Add(rock);
        }
    }

    public async Task SaveRocksAsync()
    {
        try
        {
            using var dataStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(dataStream, Rocks.ToList());
        }
        catch { }
    }

    public async Task SaveRock(Rock rock)
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

    public async Task<bool> ChangeNameAsync(Rock rock, string name)
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
