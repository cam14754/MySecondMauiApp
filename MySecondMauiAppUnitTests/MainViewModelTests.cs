namespace MySecondMauiApp.Tests;

public class MainViewModelTests
{
    //Not sure how to test loading and saving 
    //[Fact]
    //public async Task LoadRocksAsyncTest()
    //{
    //    //Arange
    //    var mockRockDataService = Substitute.For<RockDataService>();
    //    var mockFileSaver = Substitute.For<IFileSaver>();
    //    var viewModel = new MainPageViewModel(mockRockDataService, mockFileSaver);

    //    var testRocks = new ObservableCollection<Rock>
    //    {
    //        new() { Name = "Granite", Description = "A common type of felsic intrusive igneous rock." },
    //        new() { Name = "Basalt", Description = "A common extrusive igneous rock formed from the rapid cooling of basaltic lava." },
    //        new() { Name = "Limestone", Description = "A sedimentary rock composed mainly of skeletal fragments of marine organisms." }
    //    };

    //    mockRockDataService.Rocks = testRocks;

    //    //Act
    //    await viewModel.LoadRocksAsync();

    //    //Assert
    //    viewModel.Rocks.Should().AllBeOfType<Rock>();
    //    viewModel.Rocks.Should().NotBeEmpty();
    //    viewModel.Rocks.Count.Should().Be(3);
    //}



}