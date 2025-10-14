using System.Collections.ObjectModel;

namespace MySecondMauiApp.Tests.ViewModelTests;

public class MainViewModelTests
{
    [Fact]
    public async Task LoadRocksAsync_PopulatesRocksFromService()
    {
        // Arrange
        var (vm, _, mockDataService) = UnitTestHelpers.GenerateMainPageViewModelWithRock();

        var testRocks = new List<Rock>
        {
            new() { Name = "Granite", Description = "A common type of felsic intrusive igneous rock." },
            new() { Name = "Basalt", Description = "A common extrusive igneous rock formed from the rapid cooling of basaltic lava." },
            new() { Name = "Limestone", Description = "A sedimentary rock composed mainly of skeletal fragments of marine organisms." }
        };

        // Provide a real backing collection for the mock's Rocks property
        var backing = new ObservableCollection<Rock>();
        mockDataService.Rocks.Returns(backing);

        // When LoadRocksAsync is called, simulate the service loading data
        mockDataService.LoadRocksAsync().Returns(ci =>
        {
            backing.Clear();
            foreach (var r in testRocks)
            {
                backing.Add(r);
            }
            return Task.CompletedTask;
        });

        // Act
        await vm.LoadRocksAsync();

        // Assert
        vm.Rocks.Should().HaveCount(3)
            .And.AllBeOfType<Rock>()
            .And.Contain(r => r.Name == "Granite")
            .And.Contain(r => r.Name == "Basalt")
            .And.Contain(r => r.Name == "Limestone");

        // Verify the dependency call
        await mockDataService.Received(1).LoadRocksAsync();
    }
}