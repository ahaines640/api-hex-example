using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Example.Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Example.Data.Customers
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly IMapper _mapper;
        private readonly ExampleDbContext _dbContext;

        public CustomerRepository(
            IMapper mapper,
            ExampleDbContext dbContext) 
            : base(dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Domain.Customers.Customer> GetAsync(int customerId)
        {
            Customer customer = await GetEntityAsync(customerId);
            return _mapper.Map<Domain.Customers.Customer>(customer);
        }

        public async Task<IList<Domain.Customers.Customer>> ListAsync()
        {
            var customerEntities = await _dbContext
                .Customers
                .ToListAsync();

            return _mapper.Map<IList<Domain.Customers.Customer>>(customerEntities);
        }

        public async Task<Domain.Customers.Customer> CreateAsync(Domain.Customers.Customer customer, string createdBy)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (createdBy == null)
                throw new ArgumentNullException(nameof(createdBy));

            Customer customerEntity = _mapper.Map<Customer>(customer);
            await CreateEntityAsync(customerEntity, createdBy);

            return _mapper.Map<Domain.Customers.Customer>(customerEntity);
        }

        public async Task<Domain.Customers.Customer> UpdateAsync(Domain.Customers.Customer customer, string updatedBy)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (updatedBy == null)
                throw new ArgumentNullException(nameof(updatedBy));

            Customer customerEntity = await GetEntityAsync(customer.Id);

            if (customerEntity == null)
                throw new NullReferenceException($"Could not find customer Id {customer.Id}");

            customerEntity = _mapper.Map(customer, customerEntity);
            await UpdateEntityAsync(customerEntity, updatedBy);

            return _mapper.Map<Domain.Customers.Customer>(customerEntity);
        }

        public Task DeleteAsync(int customerId, string deletedBy)
        {
            if (deletedBy == null)
                throw new ArgumentNullException(nameof(deletedBy));

            return DeleteEntityAsync(customerId, deletedBy);
        }
    }
}
