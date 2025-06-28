using Core.Models;
using Core.Models.Audit;
using Microsoft.EntityFrameworkCore;

namespace BLL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Таблицы в БД
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientPhone> ClientPhones { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderTask> Tasks { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>().HasKey(a => a.Id);

        // Настройка связей между таблицами
        modelBuilder.Entity<Client>()
            .HasMany(c => c.Phones) // У клиента много телефонов
            .WithOne(p => p.Client) // У телефона один клиент
            .HasForeignKey(p => p.ClientId); // Внешний ключ

        modelBuilder.Entity<Client>()
            .HasMany(c => c.Orders) // У клиента много заказов
            .WithOne(o => o.Client) // У заказа один клиент
            .HasForeignKey(o => o.ClientId);
        
        // Связь Client-User (1-to-1)
        modelBuilder.Entity<Client>()
            .HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Client>(c => c.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems) // У заказа много товаров
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Tasks) // У заказа много задач
            .WithOne(t => t.Order)
            .HasForeignKey(t => t.OrderId);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.OrderItems) // У товара много записей в заказах
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2); // 18 цифр всего, 2 после запятой

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.PriceAtOrder)
            .HasPrecision(18, 2);

        /*modelBuilder.Entity<AuditLog>()
            .Property(a => a.AuditDate)
            .HasDefaultValueSql("GETUTCDATE()");*/
    }
}