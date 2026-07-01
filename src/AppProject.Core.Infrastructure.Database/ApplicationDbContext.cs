using System;
using AppProject.Core.Infrastructure.Database.Entities.Auth;
using AppProject.Core.Infrastructure.Database.Entities.General;
using AppProject.Core.Infrastructure.Database.Entities.Inventory;
using Microsoft.EntityFrameworkCore;

namespace AppProject.Core.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for your entities
    public DbSet<TbUser> Users { get; set; } = default!;

    public DbSet<TbCountry> Countries { get; set; } = default!;

    public DbSet<TbState> States { get; set; } = default!;

    public DbSet<TbCity> Cities { get; set; } = default!;

    public DbSet<TbNeighborhood> Neighborhoods { get; set; } = default!;

    public DbSet<TbProduct> Products { get; set; } = default!;

    public DbSet<TbStockMovement> StockMovements { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
