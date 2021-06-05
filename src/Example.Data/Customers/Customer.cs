using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Example.Data.Customers
{
    public class Customer : Entity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public List<Orders.Order> Orders { get; set; } = new List<Orders.Order>();

        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypeBuilder = modelBuilder.Entity<Customer>();

            entityTypeBuilder
                .ToTable("Customer")
                .HasKey(customer => customer.Id);

            entityTypeBuilder
                .Property(customer => customer.Id)
                .ValueGeneratedOnAdd();

            entityTypeBuilder
                .HasQueryFilter(customer => !customer.IsDeleted);
        }
    }
}
