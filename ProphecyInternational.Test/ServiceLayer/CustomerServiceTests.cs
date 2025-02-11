using Microsoft.EntityFrameworkCore;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Database.DbContexts;
using ProphecyInternational.Server.Services;

namespace ProphecyInternational.Test.ServiceLayer
{
    public class CustomerServiceTests : IDisposable
    {
        private readonly CallCenterManagementDbContext _dbContext;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            var options = new DbContextOptionsBuilder<CallCenterManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _dbContext = new CallCenterManagementDbContext(options);
            _customerService = new CustomerService(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            _dbContext.Customers.AddRange(
                new CustomerModel { Id = "C001", Name = "Alice", Email = "alice@example.com", PhoneNumber = "1234567890", LastContactDate = DateTime.Now },
                new CustomerModel { Id = "C002", Name = "Bob", Email = "bob@example.com", PhoneNumber = "0987654321", LastContactDate = DateTime.Now }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _customerService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectCustomer()
        {
            // Arrange
            var customer = new CustomerModel { Id = "C001", Name = "Alice", Email = "alice@example.com", PhoneNumber = "1234567890", LastContactDate = DateTime.Now };
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _customerService.GetByIdAsync("C001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Alice", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCustomerNotFound()
        {
            // Act
            var result = await _customerService.GetByIdAsync("Unknown");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldAddCustomerSuccessfully()
        {
            // Arrange
            var customer = new CustomerModel { Id = "C001", Name = "Alice", Email = "alice@example.com", PhoneNumber = "1234567890", LastContactDate = DateTime.Now };

            // Act
            await _customerService.AddAsync(customer);
            var addedCustomer = await _dbContext.Customers.FindAsync("C001");

            // Assert
            Assert.NotNull(addedCustomer);
            Assert.Equal("Alice", addedCustomer.Name);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenCustomerIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _customerService.AddAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingCustomer()
        {
            // Arrange
            var customer = new CustomerModel { Id = "C001", Name = "Alice", Email = "alice@example.com", PhoneNumber = "1234567890", LastContactDate = DateTime.Now };
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();

            var updatedCustomer = new CustomerModel { Id = "C001", Name = "Alice Updated", Email = "alice.updated@example.com", PhoneNumber = "0000000000", LastContactDate = DateTime.Now };

            // Act
            await _customerService.UpdateAsync(updatedCustomer);
            var result = await _dbContext.Customers.FindAsync("C001");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Alice Updated", result.Name);
            Assert.Equal("0000000000", result.PhoneNumber);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenCustomerNotFound()
        {
            // Arrange
            var customer = new CustomerModel { Id = "Unknown", Name = "Test", Email = "test@example.com", PhoneNumber = "1234567890", LastContactDate = DateTime.Now };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.UpdateAsync(customer));
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCustomer()
        {
            // Arrange
            var customer = new CustomerModel { Id = "C001", Name = "Alice", Email = "alice@example.com", PhoneNumber = "1234567890", LastContactDate = DateTime.Now };
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();

            // Act
            await _customerService.DeleteAsync("C001");
            var result = await _dbContext.Customers.FindAsync("C001");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenCustomerNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _customerService.DeleteAsync("Unknown"));
        }
    }
}