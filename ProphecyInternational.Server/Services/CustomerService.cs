using Microsoft.EntityFrameworkCore;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Database.DbContexts;
using ProphecyInternational.Server.Interfaces;

namespace ProphecyInternational.Server.Services
{
    public class CustomerService : IGenericService<CustomerModel, string>
    {
        private readonly CallCenterManagementDbContext _dbContext;

        public CustomerService(CallCenterManagementDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            return await _dbContext.Customers
                .Select(customer => new CustomerModel 
                { 
                    Id = customer.Id, 
                    Name = customer.Name,
                    Email = customer.Email, 
                    PhoneNumber = customer.PhoneNumber,
                    LastContactDate = customer.LastContactDate
                }).ToListAsync();
        }

        public async Task<CustomerModel> GetByIdAsync(string customerId)
        {
            return await _dbContext.Customers
               .Where(a => a.Id == customerId)
               .Select(customer => new CustomerModel
               {
                   Id = customer.Id,
                   Name = customer.Name,
                   Email = customer.Email,
                   PhoneNumber = customer.PhoneNumber,
                   LastContactDate = customer.LastContactDate
               }).FirstOrDefaultAsync();
        }

        public async Task AddAsync(CustomerModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbContext.Customers.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CustomerModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var existingCustomer = await _dbContext.Customers.FindAsync(entity.Id);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {entity.Id} not found.");
            }

            existingCustomer.Name = entity.Name;
            existingCustomer.Email = entity.Email;
            existingCustomer.PhoneNumber = entity.PhoneNumber;
            existingCustomer.LastContactDate = entity.LastContactDate;

            _dbContext.Customers.Update(existingCustomer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();
        }
    }
}
