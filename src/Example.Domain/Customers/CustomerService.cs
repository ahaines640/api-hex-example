using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Domain.Customers
{
    public interface ICustomerService
    {
        Task<Customer> GetAsync(int customerId);
        Task<IList<Customer>> ListAsync();
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task DeleteAsync(int customerId);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserContextResolver _userContextResolver;

        public CustomerService(
            IUserContextResolver userContextResolver,
            ICustomerRepository customerRepository)
        {
            _userContextResolver = userContextResolver;
            _customerRepository = customerRepository;
        }

        public Task<Customer> GetAsync(int customerId)
        {
            return _customerRepository.GetAsync(customerId);
        }

        public Task<IList<Customer>> ListAsync()
        {
            return _customerRepository.ListAsync();
        }

        public Task<Customer> CreateAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            return _customerRepository.CreateAsync(customer, _userContextResolver.CurrentUser);
        }

        public Task<Customer> UpdateAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            return _customerRepository.UpdateAsync(customer, _userContextResolver.CurrentUser);
        }

        public Task DeleteAsync(int customerId)
        {
            return _customerRepository.DeleteAsync(customerId, _userContextResolver.CurrentUser);
        }
    }
}