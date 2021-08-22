using Microsoft.EntityFrameworkCore;

namespace Rockstar_RockSongs_API.Models
{
    public class RockLibraryContext : DbContext
    {
        public RockLibraryContext(DbContextOptions<RockLibraryContext> options)
            : base(options)
        {
        }

        #region Defaults
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Song>()
                .Property(b => b.InsertDate)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Artist>()
                .Property(b => b.InsertDate)
                .HasDefaultValueSql("getdate()");
        }
        #endregion

        public DbSet<Song> Songs { get; set; }
        public DbSet<Artist> Artists { get; set; }

    }
}
