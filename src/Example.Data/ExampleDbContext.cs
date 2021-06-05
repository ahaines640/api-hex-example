using Example.Data.Customers;
using Example.Data.Orders;
using Microsoft.EntityFrameworkCore;

namespace Example.Data
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Customer.OnModelCreating(modelBuilder);
            Order.OnModelCreating(modelBuilder);
            OrderItem.OnModelCreating(modelBuilder);
        }
    }
}