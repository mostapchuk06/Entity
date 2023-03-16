using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

public class ShopDbConext : DbContext
{
    public ShopDbConext()
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
        DbSeeder.Seed(this);
    }
    public static class DbSeeder
    {
        public static void Seed(ShopDbConext context)
        {
            var country1 = new Countries { Name = "USA" };
            var country2 = new Countries { Name = "Canada" };
             context.SaveChanges();

            var city1 = new Cities { Name = "New York", CountryId = country1.Id };
            var city2 = new Cities { Name = "Toronto", CountryId = country2.Id };
             context.SaveChanges();

            var category1 = new Categories { Name = "Electronics" };
            var category2 = new Categories { Name = "Clothing" };
             context.SaveChanges();

            var position1 = new Positions { Name = "Manager" };
            var position2 = new Positions { Name = "Salesperson" };
             context.SaveChanges();
        }
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EntityShopDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Positions>().HasMany(x => x.Workers).WithOne(x => x.Position)
                                              .HasForeignKey(x => x.PositionId);
        modelBuilder.Entity<Shops>().HasMany(x => x.Workers).WithOne(x => x.Shop);
        modelBuilder.Entity<Shops>().HasMany(x => x.Products).WithMany(x => x.Shops);
        modelBuilder.Entity<Shops>().HasMany(x => x.Cities).WithMany(x => x.Shops);
        modelBuilder.Entity<Products>().HasOne(x => x.Category).WithMany(x => x.Products);
        modelBuilder.Entity<Cities>().HasOne(x => x.Country).WithMany(x => x.Cities);
    }
}
public class Positions
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Workers> Workers { get; set; }
}
public class Workers
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public decimal Salary { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int PositionId { get; set; }
    public int ShopId { get; set; }
    public Positions Position { get; set; }
    public Shops Shop { get; set; }
}
public class Shops
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Adress { get; set; }
    public int? ParkingArea { get; set; }
    public ICollection<Workers> Workers { get; set; }
    public ICollection<Products> Products { get; set; }
    public ICollection<Cities> Cities { get; set; }
}
public class Cities
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CountryId { get; set; }
    public Countries Country { get; set; }
    public ICollection<Shops> Shops { get; set; }
}
public class Countries
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Cities> Cities { get; set; }
}
public class Products
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public float Discount { get; set; }
    public int? CategoryId { get; set; }
    public int Quantity { get; set; }
    public bool IsinStock { get; set; }
    public ICollection<Shops> Shops { get; set; }
    public Categories Category { get; set; }
}
public class Categories
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Products> Products { get; set; }
}