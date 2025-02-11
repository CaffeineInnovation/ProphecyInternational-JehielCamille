using Microsoft.EntityFrameworkCore;
using ProphecyInternational.Common.Enums;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Database.DbContexts;
using ProphecyInternational.Server.Services;

namespace ProphecyInternational.Test.ServiceLayer
{
    public class CallServiceTests : IDisposable
    {
        private readonly CallCenterManagementDbContext _dbContext;
        private readonly CallService _callService;

        public CallServiceTests()
        {
            var options = new DbContextOptionsBuilder<CallCenterManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _dbContext = new CallCenterManagementDbContext(options);
            _callService = new CallService(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCalls()
        {
            // Arrange
            _dbContext.Calls.AddRange(
                new CallModel { Id = 1, CustomerId = "1001", AgentId = 201, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(10), Status = CallStatus.Completed, Notes = "Call 1" },
                new CallModel { Id = 2, CustomerId = "1002", AgentId = 202, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(15), Status = CallStatus.InProgress, Notes = "Call 2" }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _callService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectCall()
        {
            // Arrange
            var call = new CallModel { Id = 1, CustomerId = "1001", AgentId = 201, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(10), Status = CallStatus.Completed, Notes = "Test Call" };
            await _dbContext.Calls.AddAsync(call);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _callService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Call", result.Notes);
            Assert.Equal("1001", result.CustomerId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenCallNotFound()
        {
            // Act
            var result = await _callService.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldAddCallSuccessfully()
        {
            // Arrange
            var call = new CallModel { Id = 1, CustomerId = "1001", AgentId = 201, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(10), Status = CallStatus.Completed, Notes = "New Call" };

            // Act
            await _callService.AddAsync(call);
            var addedCall = await _dbContext.Calls.FindAsync(1);

            // Assert
            Assert.NotNull(addedCall);
            Assert.Equal("New Call", addedCall.Notes);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenCallIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _callService.AddAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingCall()
        {
            // Arrange
            var call = new CallModel { Id = 1, CustomerId = "1001", AgentId = 201, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(10), Status = CallStatus.Completed, Notes = "Original Note" };
            await _dbContext.Calls.AddAsync(call);
            await _dbContext.SaveChangesAsync();

            var updatedCall = new CallModel { Id = 1, CustomerId = "1002", AgentId = 202, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(20), Status = CallStatus.Queued, Notes = "Updated Note" };

            // Act
            await _callService.UpdateAsync(updatedCall);
            var result = await _dbContext.Calls.FindAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Note", result.Notes);
            Assert.Equal(CallStatus.Queued, result.Status);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenCallNotFound()
        {
            // Arrange
            var call = new CallModel { Id = 99, CustomerId = "1001", AgentId = 201, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(10), Status = CallStatus.Completed, Notes = "Test" };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _callService.UpdateAsync(call));
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCall()
        {
            // Arrange
            var call = new CallModel { Id = 1, CustomerId = "1001", AgentId = 201, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(10), Status = CallStatus.Completed, Notes = "Delete Me" };
            await _dbContext.Calls.AddAsync(call);
            await _dbContext.SaveChangesAsync();

            // Act
            await _callService.DeleteAsync(1);
            var result = await _dbContext.Calls.FindAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenCallNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _callService.DeleteAsync(99));
        }
    }
}