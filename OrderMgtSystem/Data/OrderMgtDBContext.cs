using Microsoft.EntityFrameworkCore;
using OrderMgtSystem.Models;

namespace OrderMgtSystem.Data
{
    public class OrderMgtDBContext: DbContext
    {
        /// <summary>
        /// Constructor accepting DbContextOptions for dependency injection
        /// </summary>
        public OrderMgtDBContext(DbContextOptions<OrderMgtDBContext> options): base(options)
        {
        }

        /// <summary>
        /// DbSet for Customer entities
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// DbSet for Product entities
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// DbSet for Order entities
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// DbSet for OrderItem entities
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// Configure entity models and database constraints
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");

                // Configure primary key
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(e => e.CreatedDate)
                    .HasDatabaseName("IX_Customers_CreatedDate");

            });

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                // Configure primary key
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Price)
                    .HasPrecision(10, 2);

                entity.Property(e => e.StockQuantity)
                    .HasPrecision(10, 2);

                entity.HasIndex(e => e.CreatedDate)
                    .HasDatabaseName("IX_Products_CreatedDate");

            });


            // Configure Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(e => e.OrderId);

                // Link to Customer
                entity.HasOne(e => e.Customer)
                      .WithMany()
                      .HasForeignKey(o => o.CustomerId);

                entity.HasIndex(e => e.OrderDate).HasDatabaseName("IX_Orders_OrderDate");
            });

            // Configure OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems");
                entity.HasKey(e => e.OrderItemId);

                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(o => o.OrderId);

                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(o => o.ProductId);

                entity.Property(e => e.Quantity).HasPrecision(10, 2);
                entity.Property(e => e.LineTotal).HasPrecision(10, 2);
            });
        }

    }
}
