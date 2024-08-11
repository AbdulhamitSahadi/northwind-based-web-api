using Microsoft.EntityFrameworkCore;
using NorthwindBasedWebApplication.API.Models;

namespace NorthwindBasedWebApplication.API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerCustomerDemographic> CustomersCustomerDemographics { get; set; }
        public DbSet<CustomerDemographic> CustomerDemographics { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTerritory> EmployeesTerritories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Territory> Territories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>()
                .HasMany(p => p.Products)
                .WithOne(c => c.Category)
                .HasForeignKey(fk => fk.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Customer>()
                .HasMany(o => o.Orders)
                .WithOne(c => c.Customer)
                .HasForeignKey(fk => fk.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<CustomerCustomerDemographic>()
                .HasOne(c => c.Customer)
                .WithMany(m => m.CustomerCustomerDemographic)
                .HasForeignKey(fk => fk.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerCustomerDemographic>()
                .HasOne(t => t.CustomerType)
                .WithMany(m => m.CustomerCustomerDemographic)
                .HasForeignKey(fk => fk.CustomerTypeId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Employee>()
                .HasMany(o => o.Orders)
                .WithOne(e => e.Employee)
                .HasForeignKey(fk => fk.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasOne(rt => rt.Report)
                .WithMany()
                .HasForeignKey(fk => fk.ReportsTo)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<EmployeeTerritory>()
                .HasOne(e => e.Employee)
                .WithMany(m => m.EmployeeTerritory)
                .HasForeignKey(fk => fk.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeTerritory>()
                .HasOne(t => t.Territory)
                .WithMany(m => m.EmployeeTerritory)
                .HasForeignKey(fk => fk.TerritoryId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Order>()
                .HasOne(s => s.Shipper)
                .WithMany(m => m.Orders)
                .HasForeignKey(fk => fk.ShipVia)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<OrderDetails>()
                .HasOne(p => p.Product)
                .WithMany(m => m.OrderDetails)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetails>()
                .HasOne(o => o.Order)
                .WithMany(m => m.OrderDetails)
                .HasForeignKey(fk => fk.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Product>()
                .HasOne(s => s.Supplier)
                .WithMany(p => p.Products)
                .HasForeignKey(fk => fk.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Region>()
                .HasMany(t => t.Territories)
                .WithOne(r => r.Region)
                .HasForeignKey(fk => fk.RegionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
