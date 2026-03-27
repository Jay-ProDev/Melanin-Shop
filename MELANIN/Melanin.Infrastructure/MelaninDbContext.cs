using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Melanin.Infrastructure.Database;

public class MelaninDbContext : DbContext
{
    // Chaque DbSet représente une table en base
    public DbSet<Member> Members { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }

    // Constructeur : reçoit les options de connexion (connection string etc.)
    public MelaninDbContext(DbContextOptions<MelaninDbContext> options)
        : base(options)
    {
    }

    // Charge automatiquement toutes les configurations Fluent API du projet
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.ApplyConfigurationsFromAssembly(
        //    typeof(MelaninDbContext).Assembly
        //);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}