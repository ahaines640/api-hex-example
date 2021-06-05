using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Domain.Customers
{
    public interface ICustomerRepository
    {
        Task<Customer> GetAsync(int customerId);
        Task<IList<Customer>> ListAsync();
        Task<Customer> CreateAsync(Customer customer, string createdBy);
        Task<Customer> UpdateAsync(Customer customer, string updatedBy);
        Task DeleteAsync(int customerId, string deletedBy);
    }
}