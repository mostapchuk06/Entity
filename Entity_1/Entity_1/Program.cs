using Microsoft.EntityFrameworkCore;
public class MusicalApplicationDbContext : DbContext
{
    public MusicalApplicationDbContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MusicalAppDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Artist>().HasData(new[]
        {
             new Artist(){Id = 1, Name = "John", Surname = "Doe", CountryId = 1},
             new Artist(){Id = 2, Name = "Jane", Surname = "Doe", CountryId = 2},
             new Artist(){Id = 3, Name = "Bob", Surname = "Smith", CountryId = 3},
        });

        modelBuilder.Entity<Track>().HasData(new[]
        {
            new Track(){Id = 1, Name = "Song 1", AlbumId = 1, Time = 3.5f},
            new Track(){Id = 2, Name = "Song 2", AlbumId = 1, Time = 4.2f},
            new Track(){Id = 3, Name = "Song 3", AlbumId = 2, Time = 5.1f},
        });

        modelBuilder.Entity<Playlist>().HasData(new[]
        {
            new Playlist(){Id = 1, Name = "My Playlist 1"},
            new Playlist(){Id = 2, Name = "My Playlist 2"},
        });

        modelBuilder.Entity<Coutry>().HasData(new[]
        {
            new Coutry(){Id = 1, Name = "Ukraine"},
            new Coutry(){Id = 2, Name = "German"},
            new Coutry(){Id = 3, Name = "USA"},
        });

        modelBuilder.Entity<Album>().HasData(new[]
        {
            new Album(){Id = 1, Name = "Album 1"},
            new Album(){Id = 2, Name = "Album 2"},
            new Album(){Id = 3, Name = "Album 3"},
        });

        modelBuilder.Entity<Genre>().HasData(new[]
        {
            new Genre(){Id = 1, Name = "Rock"},
            new Genre(){Id = 2, Name = "Alternative Rock"},
            new Genre(){Id = 3, Name = "Metal"},
        });
        
        modelBuilder.Entity<Category>().HasData(new[]
        {
            new Category(){Id = 1, Name = "Sleep"},
            new Category(){Id = 2, Name = "Party"},
            new Category(){Id = 3, Name = "Walk"},
        });
    }

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Track> Tracks  { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Category> Categories { get; set; }
}
public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int CountryId { get; set; }
    public int AlbumId { get; set; }
    public Coutry Coutry { get; set; }
    public ICollection<Album> Albums { get; set; }
}
public class Coutry
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Artist> Artists { get; set; }
}
public class Album
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Artist Artist { get; set; }
    public int Year { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
    public ICollection<Track> Tracks { get; set; }
}
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Album> Albums { get; set; }
}
public class Track
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Album Album { get; set; }
    public float Time { get; set; }
    public int AlbumId { get; set; }
    public ICollection<Playlist> Playlists { get; set; }
}
public class Playlist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Track> Tracks { get; set; }
    public ICollection<Category> Categories { get; set; }
}
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Playlist> Playlists { get; set; }
}

internal class Program
{
    static void Main(string[] args)
    {

        MusicalApplicationDbContext context = new();
        context.Artists.ToList();

        var playlist = new Playlist() { Name = "My New Playlist" };

        // add tracks to the playlist
        var tracks = context.Tracks.Take(2).ToList();
        foreach (var track in tracks)
        {
            playlist.Tracks.Add(track);
        }

        // add categories to the playlist
        var categories = context.Categories.Take(2).ToList();
        foreach (var category in categories)
        {
            playlist.Categories.Add(category);
        }

        // add the playlist to the database
        context.Playlists.Add(playlist);
        context.SaveChanges();
    }
}