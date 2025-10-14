// SPDX-License-Identifier: Proprietary
// © 2025 Cameron Strachan, trading as Cameron's Rock Company. All rights reserved.
// Created by Cameron Strachan.
// For personal and educational use only.

namespace MySecondMauiApp;
/// <summary>
/// Interface for rock data service, providing methods for managing rock data including loading, saving, deleting, duplicating, and renaming rocks.
/// </summary>
public interface IRockDataService
{
    ObservableCollection<Rock> Rocks { get; set; }
    Task<bool> ChangeRockNameAsync(Rock rock, string name);
    Task<bool> DeleteRockAsync(Rock rock);
    Task<bool> DuplicateRockAsync(Rock rock);
    Task LoadRocksAsync();
    Task SaveRock(Rock? rock);
    Task SaveRocksAsync();
}
