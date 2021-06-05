using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Example.Data.Orders
{
    public class Order : Entity
    {
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; }
        public Customers.Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<Order>();

            entityTypeBuilder
                .ToTable("Order")
                .HasKey(order => order.Id);

            entityTypeBuilder
                .Property(order => order.Id)
                .ValueGeneratedOnAdd();

            entityTypeBuilder
                .HasQueryFilter(order => !order.IsDeleted);

            entityTypeBuilder
                .HasOne(order => order.Customer)
                .WithMany(customer => customer.Orders)
                .HasForeignKey(order => order.CustomerId);
        }
    }
}
