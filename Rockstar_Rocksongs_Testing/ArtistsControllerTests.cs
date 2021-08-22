using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rockstar_RockSongs_API.Controllers;
using Rockstar_RockSongs_API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Rockstar_Rocksongs_Testing
{
    public class ArtistsControllerTests
    {
        private readonly IServiceProvider serviceProvider;

        public ArtistsControllerTests()
        {
            var services = new ServiceCollection();

            services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<RockLibraryContext>(options => options.UseInMemoryDatabase("artistTestDB"));

            serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void GetArtistResultCheck()
        {
            // Arrange
            var dbContext = serviceProvider.GetRequiredService<RockLibraryContext>();
            CreateTestData(dbContext);
            var controller = new ArtistsController(dbContext);

            // Act
            var actionResult = controller.GetArtists();

            // Assert
            actionResult.Should().BeOfType<Task<ActionResult<IEnumerable<Artist>>>>()
                .Which.Result.Value.Should().HaveCount(5);
        }

        [Fact]
        public void GetArtistByIdShouldReturnResult()
        {
            // Arrange
            var dbContext = serviceProvider.GetRequiredService<RockLibraryContext>();
            CreateTestData(dbContext);
            var controller = new ArtistsController(dbContext);

            // Act
            var actionResult = controller.GetArtist(4);

            // Assert
            actionResult.Result.Value.Should().NotBeNull();
        }

        [Fact]
        public void GetArtistByNameCheck()
        {
            // Arrange
            var dbContext = serviceProvider.GetRequiredService<RockLibraryContext>();
            CreateTestData(dbContext);
            var controller = new ArtistsController(dbContext);

            // Act
            var actionResult = controller.GetArtistByName("Adele");

            // Assert
            actionResult.Result.Value.Id.Should().Be(5);
        }

        [Fact]
        public void DeleteArtistCheck()
        {
            // Arrange
            var dbContext = serviceProvider.GetRequiredService<RockLibraryContext>();
            CreateTestData(dbContext);
            var controller = new ArtistsController(dbContext);

            // Act
            var actionResult = controller.DeleteArtist(1);

            // Assert
            actionResult.Result.Should().BeOfType<NoContentResult>();
        }

        private void CreateTestData(RockLibraryContext dbContext)
        {

            var artists = new List<Artist>()
            {
                new Artist() { Id = 1, Name = "Kenny Rogers", InsertDate = DateTime.Now},
                new Artist() { Id = 2, Name = "Kendrick Lamar", InsertDate = DateTime.Now},
                new Artist() { Id = 3, Name = "Corrie Konings", InsertDate = DateTime.Now},
                new Artist() { Id = 4, Name = "Chuck Norris", InsertDate = DateTime.Now.AddDays(-1)},
                new Artist() { Id = 5, Name = "Adele", InsertDate = DateTime.Now}
            };

            dbContext.Database.EnsureDeleted();

            dbContext.Artists.AddRange(artists);
            dbContext.SaveChanges();
        }
    }
}
