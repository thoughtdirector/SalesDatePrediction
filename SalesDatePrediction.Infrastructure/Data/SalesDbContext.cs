using Microsoft.EntityFrameworkCore;
using SalesDatePrediction.Core.Domain.Entities;

namespace SalesDatePrediction.Infrastructure.Data
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipper> Shippers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Sales.Customers").HasKey(c => c.Custid);

            modelBuilder.Entity<Order>().ToTable("Sales.Orders").HasKey(o => o.OrderID);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.Custid);

            modelBuilder.Entity<OrderDetail>().ToTable("Sales.OrderDetails")
                .HasKey(od => new { od.OrderID, od.ProductID });

            modelBuilder.Entity<Employee>().ToTable("HR.Employees").HasKey(e => e.EmpID);

            modelBuilder.Entity<Product>().ToTable("Production.Products").HasKey(p => p.ProductID);

            modelBuilder.Entity<Shipper>().ToTable("Sales.Shippers").HasKey(s => s.ShipperID);
        }
    }
}
