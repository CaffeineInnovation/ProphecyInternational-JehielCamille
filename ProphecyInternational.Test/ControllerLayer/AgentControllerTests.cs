using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProphecyInternational.Common.Enums;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Server.Controllers;
using ProphecyInternational.Server.Interfaces;

namespace ProphecyInternational.Test.ControllerLayer
{
    public class AgentControllerTests
    {
        private readonly Mock<IGenericService<AgentModel, int>> _mockAgentService;
        private readonly Mock<ILogger<AgentController>> _mockLogger;
        private readonly AgentController _controller;

        public AgentControllerTests()
        {
            _mockAgentService = new Mock<IGenericService<AgentModel, int>>();
            _mockLogger = new Mock<ILogger<AgentController>>();
            _controller = new AgentController(_mockLogger.Object, _mockAgentService.Object);
        }

        [Fact]
        public async Task GetAllAgents_ReturnsOk_WithListOfAgents()
        {
            // Arrange
            var agents = new List<AgentModel> { new AgentModel { Id = 1, Name = "Agent1" } };
            _mockAgentService.Setup(s => s.GetAllAsync()).ReturnsAsync(agents);

            // Act
            var result = await _controller.GetAllAgents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnAgents = Assert.IsType<List<AgentModel>>(okResult.Value);
            Assert.Single(returnAgents);
        }

        [Fact]
        public async Task GetAgent_ReturnsOk_WithAgent()
        {
            // Arrange
            var agent = new AgentModel { Id = 1, Name = "Agent1" };
            _mockAgentService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(agent);

            // Act
            var result = await _controller.GetAgent(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnAgent = Assert.IsType<AgentModel>(okResult.Value);
            Assert.Equal(1, returnAgent.Id);
        }

        [Fact]
        public async Task GetAgent_ReturnsNotFound_WhenAgentDoesNotExist()
        {
            _mockAgentService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((AgentModel)null);

            var result = await _controller.GetAgent(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateAgent_ReturnsCreatedAtAction()
        {
            var agent = new AgentModel { Id = 1, Name = "Agent1" };
            _mockAgentService.Setup(s => s.AddAsync(agent)).Returns(Task.CompletedTask);

            var result = await _controller.CreateAgent(agent);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetAgent", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task UpdateAgent_ReturnsNoContent_WhenSuccessful()
        {
            var agent = new AgentModel { Id = 1, Name = "Agent1" };
            _mockAgentService.Setup(s => s.UpdateAsync(agent)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateAgent(1, agent);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAgent_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            var agent = new AgentModel { Id = 2, Name = "Agent1" };
            var result = await _controller.UpdateAgent(1, agent);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteAgent_ReturnsNoContent_WhenSuccessful()
        {
            _mockAgentService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.DeleteAgent(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAgentStatus_ReturnsNoContent_WhenSuccessful()
        {
            var agent = new AgentModel { Id = 1, Name = "Agent1", Status = AgentStatus.Available };
            _mockAgentService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(agent);
            _mockAgentService.Setup(s => s.UpdateAsync(agent)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateAgentStatus(1, AgentStatus.Busy);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAgentStatus_ReturnsNotFound_WhenAgentDoesNotExist()
        {
            _mockAgentService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((AgentModel)null);

            var result = await _controller.UpdateAgentStatus(1, AgentStatus.Offline);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}