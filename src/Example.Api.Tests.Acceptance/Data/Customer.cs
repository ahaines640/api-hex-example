using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Tests.Acceptance.Data
{
    public class Customer : Entity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

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