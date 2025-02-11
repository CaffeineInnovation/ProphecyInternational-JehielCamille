using Microsoft.EntityFrameworkCore;
using ProphecyInternational.Common.Enums;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Database.DbContexts;
using ProphecyInternational.Server.Services;

namespace ProphecyInternational.Test.ServiceLayer
{
    public class AgentServiceTests : IDisposable
    {
        private readonly CallCenterManagementDbContext _dbContext;
        private readonly AgentService _agentService;

        public AgentServiceTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<CallCenterManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _dbContext = new CallCenterManagementDbContext(options);
            _agentService = new AgentService(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAgents()
        {
            // Arrange
            _dbContext.Agents.AddRange(
                new AgentModel { Id = 1, Name = "Agent A", Email = "a@test.com", PhoneExtension = "101", Status = AgentStatus.Available },
                new AgentModel { Id = 2, Name = "Agent B", Email = "b@test.com", PhoneExtension = "102", Status = AgentStatus.Busy },
                new AgentModel { Id = 3, Name = "Agent C", Email = "c@test.com", PhoneExtension = "103", Status = AgentStatus.Offline }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _agentService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectAgent()
        {
            // Arrange
            var agent = new AgentModel { Id = 1, Name = "Agent A", Email = "a@test.com", PhoneExtension = "101", Status = AgentStatus.Available };
            await _dbContext.Agents.AddAsync(agent);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _agentService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Agent A", result.Name);
            Assert.Equal("a@test.com", result.Email);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenAgentNotFound()
        {
            // Act
            var result = await _agentService.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldAddAgentSuccessfully()
        {
            // Arrange
            var agent = new AgentModel { Id = 1, Name = "New Agent", Email = "new@test.com", PhoneExtension = "103", Status = AgentStatus.Available };

            // Act
            await _agentService.AddAsync(agent);
            var addedAgent = await _dbContext.Agents.FindAsync(1);

            // Assert
            Assert.NotNull(addedAgent);
            Assert.Equal("New Agent", addedAgent.Name);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenAgentIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _agentService.AddAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingAgent()
        {
            // Arrange
            var agent = new AgentModel { Id = 1, Name = "Agent A", Email = "a@test.com", PhoneExtension = "101", Status = AgentStatus.Available };
            await _dbContext.Agents.AddAsync(agent);
            await _dbContext.SaveChangesAsync();

            var updatedAgent = new AgentModel { Id = 1, Name = "Updated Name", Email = "updated@test.com", PhoneExtension = "999", Status = AgentStatus.Busy };

            // Act
            await _agentService.UpdateAsync(updatedAgent);
            var result = await _dbContext.Agents.FindAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.Name);
            Assert.Equal("updated@test.com", result.Email);
            Assert.Equal("999", result.PhoneExtension);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenAgentNotFound()
        {
            // Arrange
            var agent = new AgentModel { Id = 99, Name = "Non-existent", Email = "none@test.com", PhoneExtension = "000", Status = AgentStatus.Busy };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _agentService.UpdateAsync(agent));
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveAgent()
        {
            // Arrange
            var agent = new AgentModel { Id = 1, Name = "Agent A", Email = "a@test.com", PhoneExtension = "101", Status = AgentStatus.Available };
            await _dbContext.Agents.AddAsync(agent);
            await _dbContext.SaveChangesAsync();

            // Act
            await _agentService.DeleteAsync(1);
            var result = await _dbContext.Agents.FindAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenAgentNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _agentService.DeleteAsync(99));
        }
    }
}