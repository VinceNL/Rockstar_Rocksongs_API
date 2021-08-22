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
    public class SongsControllerTests
    {
        private readonly IServiceProvider serviceProvider;

        public SongsControllerTests()
        {
            var services = new ServiceCollection();

            services.AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<RockLibraryContext>(options => options.UseInMemoryDatabase("songsTestDB"));

            serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void GetSongsResultCheck()
        {
            // Arrange
            var dbContext = serviceProvider.GetRequiredService<RockLibraryContext>();
            CreateTestData(dbContext);
            var controller = new SongsController(dbContext);

            // Act
            var actionResult = controller.GetSongs();

            // Assert
            actionResult.Should().BeOfType<Task<ActionResult<IEnumerable<Song>>>>()
                .Which.Result.Value.Should().HaveCount(5);
        }

        [Fact]
        public void GetSongByIdShouldReturnResult()
        {
            // Arrange
            var dbContext = serviceProvider.GetRequiredService<RockLibraryContext>();
            CreateTestData(dbContext);
            var controller = new SongsController(dbContext);

            // Act
            var actionResult = controller.GetSong(190);

            // Assert
            actionResult.Result.Value.Should().NotBeNull();
        }

        [Fact]
        public void GetArtistByGenreCheck()
        {
            // Arrange
            var dbContext = serviceProvider.GetRequiredService<RockLibraryContext>();
            CreateTestData(dbContext);
            var controller = new SongsController(dbContext);

            // Act
            var actionResult = controller.GetSongByGenre("New Wave");

            // Assert
            actionResult.Result.Value.Id.Should().Be(1858);
        }

        [Fact]
        public void DeleteSongCheck()
        {
            // Arrange
            var dbContext = serviceProvider.GetRequiredService<RockLibraryContext>();
            CreateTestData(dbContext);
            var controller = new SongsController(dbContext);

            // Act
            var actionResult = controller.DeleteSong(190);

            // Assert
            actionResult.Result.Should().BeOfType<NoContentResult>();
        }

        private void CreateTestData(RockLibraryContext dbContext)
        {

            var songs = new List<Song>()
            {
                new Song() { Id = 190, Name = "(Don't Fear) The Reaper", Year = 1975, Artist = "Blue Öyster Cult", Shortname = "dontfearthereaper", Bpm = 141, Duration = 322822, Genre = "Classic Rock", SpotifyId = "5QTxFnGygVM4jFQiBovmRo", Album = "Agents of Fortune", InsertDate = DateTime.Now},
                new Song() { Id = 1858, Name = "(I Just) Died in Your Arms", Year = 1986, Artist = "Cutting Crew", Shortname = "ijustdiedinyourarms", Bpm = 126, Duration = 285745, Genre = "New Wave", SpotifyId = "4ByEFOBuLXpCqvO1kw8Wdm", Album = "Broadcast", InsertDate = DateTime.Now},
                new Song() { Id = 1557, Name = "(If You're Wondering If I Want You To) I Want You To", Year = 2009, Artist = "Weezer", Shortname = "iwantyouto", Bpm = 110, Duration = 211635, Genre = "Alternative", SpotifyId = "7akwKkIS6sp7Orbq7LEj5a", Album = "Raditude", InsertDate = DateTime.Now},
                new Song() { Id = 1363, Name = "(Funky) Sex Farm", Year = 2009, Artist = "Spinal Tap", Shortname = "funkysexfarm", Bpm = 107, Duration = 260068, Genre = "Metal", SpotifyId = "1VfdMD8JguRISrccgLifIZ", Album = "Back From The Dead", InsertDate = DateTime.Now.AddDays(-1)},
                new Song() { Id = 1364, Name = "(Listen to the) Flower People (Reggae Stylee)", Year = 2009, Artist = "Spinal Tap", Shortname = "flowerpeople", Bpm = 120, Duration = 197350, Genre = "Metal", SpotifyId = "5Qe3fjOInx8FitkywWeDKn", Album = "Back From The Dead", InsertDate = DateTime.Now}
            };

            dbContext.Database.EnsureDeleted();

            dbContext.Songs.AddRange(songs);
            dbContext.SaveChanges();
        }
    }
}
