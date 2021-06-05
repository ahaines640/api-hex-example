using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Tests.Acceptance.Data
{
    public class Order : Entity
    {
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();

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