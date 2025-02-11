using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Server.Controllers;
using ProphecyInternational.Server.Interfaces;

namespace ProphecyInternational.Test.ControllerLayer
{
    public class TicketControllerTests
    {
        private readonly Mock<IGenericService<TicketModel, int>> _mockService;
        private readonly Mock<ILogger<TicketController>> _mockLogger;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _mockService = new Mock<IGenericService<TicketModel, int>>();
            _mockLogger = new Mock<ILogger<TicketController>>();
            _controller = new TicketController(_mockLogger.Object, _mockService.Object);
        }

        [Fact]
        public async Task GetAllTickets_ReturnsOkResult_WithListOfTickets()
        {
            var tickets = new List<TicketModel> { new TicketModel { Id = 1 }, new TicketModel { Id = 2 } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(tickets);

            var result = await _controller.GetAllTickets();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTickets = Assert.IsType<List<TicketModel>>(okResult.Value);
            Assert.Equal(2, returnedTickets.Count);
        }

        [Fact]
        public async Task GetTicket_ExistingId_ReturnsOkResult()
        {
            var ticket = new TicketModel { Id = 1 };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(ticket);

            var result = await _controller.GetTicket(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTicket = Assert.IsType<TicketModel>(okResult.Value);
            Assert.Equal(1, returnedTicket.Id);
        }

        [Fact]
        public async Task GetTicket_NonExistingId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((TicketModel)null);

            var result = await _controller.GetTicket(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateTicket_ValidTicket_ReturnsCreatedAtAction()
        {
            var ticket = new TicketModel { Id = 1 };
            _mockService.Setup(s => s.AddAsync(ticket)).Returns(Task.CompletedTask);

            var result = await _controller.CreateTicket(ticket);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetTicket", createdAtActionResult.ActionName);
            Assert.Equal(1, ((TicketModel)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task UpdateTicket_ValidTicket_ReturnsNoContent()
        {
            var ticket = new TicketModel { Id = 1 };
            _mockService.Setup(s => s.UpdateAsync(ticket)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateTicket(1, ticket);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTicket_ExistingId_ReturnsNoContent()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteTicket(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AssignTicketToAgent_ExistingTicket_ReturnsNoContent()
        {
            var ticket = new TicketModel { Id = 1, AgentId = 0 };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(ticket);
            _mockService.Setup(s => s.UpdateAsync(ticket)).Returns(Task.CompletedTask);

            var result = await _controller.AsssignTicketToAgent(1, 99);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AssignTicketToAgent_NonExistingTicket_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((TicketModel)null);

            var result = await _controller.AsssignTicketToAgent(1, 99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}