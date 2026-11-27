using Microsoft.EntityFrameworkCore;
using WebApplication3.Application.Common.Abstractions;
using WebApplication3.Domain;

namespace WebApplication3.Infrastructure
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private readonly EntityConfigLocation _config;

        public AppDbContext(DbContextOptions<AppDbContext> options, EntityConfigLocation config)
            : base(options)
        {
            _config = config;
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Простые сущности — из JSON
            JsonSimpleConfigurator.Apply(modelBuilder, _config.Path, typeof(Order).Assembly);

            // 2. Сложные — вручную
            ConfigureOrder(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void ConfigureOrder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(b =>
            {
                b.ToTable("Orders");
                b.HasKey(o => o.Id);
                b.Property(o => o.CustomerEmail).IsRequired().HasMaxLength(256);
                b.Property(o => o.Status).HasConversion<int>().IsRequired();
                b.Property(o => o.Total).HasColumnType("decimal(18,2)").IsRequired();
                b.Property(o => o.CreatedAtUtc).IsRequired();
                b.HasIndex(o => o.CustomerEmail);

                // Items — только для чтения, EF заполняет приватное поле _items
                b.OwnsMany<OrderItem>(nameof(Order.Items), owned =>
                {
                    owned.ToTable("OrderItems");
                    owned.WithOwner().HasForeignKey("OrderId");
                    owned.HasKey(i => i.Id);
                    owned.Property(i => i.ProductId).IsRequired();
                    owned.Property(i => i.Name).IsRequired().HasMaxLength(200);
                    owned.Property(i => i.Price).HasColumnType("decimal(18,2)").IsRequired();
                    owned.Property(i => i.Quantity).IsRequired();
                });
            });
        }
    }
}
