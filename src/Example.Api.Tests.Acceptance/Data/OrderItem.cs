using Microsoft.EntityFrameworkCore;

namespace Example.Api.Tests.Acceptance.Data
{
    public class OrderItem : Entity
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<OrderItem>();

            entityTypeBuilder
                .ToTable("OrderItem")
                .HasKey(orderItem => orderItem.Id);

            entityTypeBuilder
                .Property(orderItem => orderItem.Id)
                .ValueGeneratedOnAdd();

            entityTypeBuilder
                .HasQueryFilter(orderItem => !orderItem.IsDeleted);

            entityTypeBuilder
                .HasOne(orderItem => orderItem.Order)
                .WithMany(orders => orders.OrderItems)
                .HasForeignKey(orderItem => orderItem.OrderId);
        }
    }
}