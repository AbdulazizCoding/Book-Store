using Book_Store.Entities;
using Microsoft.EntityFrameworkCore;

namespace Book_Store.Data;

public class AppDbContext : DbContext
{
  public DbSet<Book>? Books { get; set; }
  public DbSet<Author>? Authors { get; set; }
  public DbSet<Discount>? Discounts { get; set; }
  public DbSet<Genres>? Genres { get; set; }
  public DbSet<Publisher>? Publishers { get; set; }

  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}