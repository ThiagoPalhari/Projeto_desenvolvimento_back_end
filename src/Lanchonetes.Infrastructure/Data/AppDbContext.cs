using Lanchonetes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<UnitProduct> UnitProducts => Set<UnitProduct>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<LoyaltyAccount> LoyaltyAccounts => Set<LoyaltyAccount>();
    public DbSet<LoyaltyTransaction> LoyaltyTransactions => Set<LoyaltyTransaction>();
    public DbSet<Consent> Consents => Set<Consent>();
    public DbSet<Promotion> Promotions => Set<Promotion>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePermission>().HasKey(x => new { x.RoleId, x.PermissionId });
        modelBuilder.Entity<UnitProduct>().HasKey(x => new { x.UnitId, x.ProductId });
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Unit>().HasIndex(x => x.Cnpj).IsUnique();

        modelBuilder.Entity<Product>().Property(x => x.Price).HasPrecision(18, 2);

        modelBuilder.Entity<Order>().Property(x => x.Subtotal).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.Discount).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.Total).HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(x => x.Total).HasPrecision(18, 2);

        modelBuilder.Entity<Payment>().Property(x => x.Amount).HasPrecision(18, 2);

        modelBuilder.Entity<Stock>().Property(x => x.Quantity).HasPrecision(18, 3);
        modelBuilder.Entity<StockMovement>().Property(x => x.Quantity).HasPrecision(18, 3);

        base.OnModelCreating(modelBuilder);
    }
}
