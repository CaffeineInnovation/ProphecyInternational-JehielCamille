using Microsoft.EntityFrameworkCore;
using ProphecyInternational.Common.Enums;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Database.DbContexts;
using ProphecyInternational.Server.Services;

namespace ProphecyInternational.Test.ServiceLayer
{
    public class TicketServiceTests : IDisposable
    {
        private readonly CallCenterManagementDbContext _dbContext;
        private readonly TicketService _ticketService;

        public TicketServiceTests()
        {
            var options = new DbContextOptionsBuilder<CallCenterManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            _dbContext = new CallCenterManagementDbContext(options);
            _ticketService = new TicketService(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTickets()
        {
            // Arrange
            _dbContext.Tickets.AddRange(
                new TicketModel { Id = 1, CustomerId = "C001", AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Issue 1", Resolution = "Pending" },
                new TicketModel { Id = 2, CustomerId = "C002", AgentId = 2, Status = TicketStatus.Closed, Priority = TicketPriority.Low, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Issue 2", Resolution = "Resolved" }
            );
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ticketService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectTicket()
        {
            // Arrange
            var ticket = new TicketModel { Id = 1, CustomerId = "C001", AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Issue 1", Resolution = "Pending" };
            await _dbContext.Tickets.AddAsync(ticket);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ticketService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Issue 1", result.Description);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTicketNotFound()
        {
            // Act
            var result = await _ticketService.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_ShouldAddTicketSuccessfully()
        {
            // Arrange
            var ticket = new TicketModel { Id = 1, CustomerId = "C001", AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Issue 1", Resolution = "Pending" };

            // Act
            await _ticketService.AddAsync(ticket);
            var addedTicket = await _dbContext.Tickets.FindAsync(1);

            // Assert
            Assert.NotNull(addedTicket);
            Assert.Equal("Issue 1", addedTicket.Description);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenTicketIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ticketService.AddAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingTicket()
        {
            // Arrange
            var ticket = new TicketModel { Id = 1, CustomerId = "C001", AgentId =1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Issue 1", Resolution = "Pending" };
            await _dbContext.Tickets.AddAsync(ticket);
            await _dbContext.SaveChangesAsync();

            var updatedTicket = new TicketModel { Id = 1, CustomerId = "C002", AgentId = 2, Status = TicketStatus.Closed, Priority = TicketPriority.Low, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Updated Issue", Resolution = "Resolved" };

            // Act
            await _ticketService.UpdateAsync(updatedTicket);
            var result = await _dbContext.Tickets.FindAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Issue", result.Description);
            Assert.Equal(TicketStatus.Closed, result.Status);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenTicketNotFound()
        {
            // Arrange
            var ticket = new TicketModel { Id = 999, CustomerId = "C001", AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Non-existent Issue", Resolution = "Pending" };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _ticketService.UpdateAsync(ticket));
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTicket()
        {
            // Arrange
            var ticket = new TicketModel { Id = 1, CustomerId = "C001", AgentId = 1, Status = TicketStatus.Open, Priority = TicketPriority.High, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Description = "Issue 1", Resolution = "Pending" };
            await _dbContext.Tickets.AddAsync(ticket);
            await _dbContext.SaveChangesAsync();

            // Act
            await _ticketService.DeleteAsync(1);
            var result = await _dbContext.Tickets.FindAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenTicketNotFound()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _ticketService.DeleteAsync(999));
        }
    }
}