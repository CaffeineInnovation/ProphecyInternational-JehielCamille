using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Server.Controllers;
using ProphecyInternational.Server.Interfaces;

namespace ProphecyInternational.Test.ControllerLayer
{
    public class CallControllerTests
    {
        private readonly Mock<IGenericService<CallModel, int>> _mockService;
        private readonly Mock<ILogger<CallController>> _mockLogger;
        private readonly CallController _controller;

        public CallControllerTests()
        {
            _mockService = new Mock<IGenericService<CallModel, int>>();
            _mockLogger = new Mock<ILogger<CallController>>();
            _controller = new CallController(_mockLogger.Object, _mockService.Object);
        }

        [Fact]
        public async Task GetAllCalls_ReturnsOkResult_WithListOfCalls()
        {
            var calls = new List<CallModel> { new CallModel { Id = 1 }, new CallModel { Id = 2 } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(calls);

            var result = await _controller.GetAllCalls();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCalls = Assert.IsType<List<CallModel>>(okResult.Value);
            Assert.Equal(2, returnedCalls.Count);
        }

        [Fact]
        public async Task GetCall_ExistingId_ReturnsOkResult()
        {
            var call = new CallModel { Id = 1 };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(call);

            var result = await _controller.GetCall(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCall = Assert.IsType<CallModel>(okResult.Value);
            Assert.Equal(1, returnedCall.Id);
        }

        [Fact]
        public async Task GetCall_NonExistingId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((CallModel)null);

            var result = await _controller.GetCall(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCall_ValidCall_ReturnsCreatedAtAction()
        {
            var call = new CallModel { Id = 1 };
            _mockService.Setup(s => s.AddAsync(call)).Returns(Task.CompletedTask);

            var result = await _controller.CreateCall(call);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetCall", createdAtActionResult.ActionName);
            Assert.Equal(1, ((CallModel)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task UpdateCall_ValidCall_ReturnsNoContent()
        {
            var call = new CallModel { Id = 1 };
            _mockService.Setup(s => s.UpdateAsync(call)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateCall(1, call);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCall_ExistingId_ReturnsNoContent()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteCall(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AssignCallToAgent_ExistingCall_ReturnsNoContent()
        {
            var call = new CallModel { Id = 1, AgentId = 0 };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(call);
            _mockService.Setup(s => s.UpdateAsync(call)).Returns(Task.CompletedTask);

            var result = await _controller.AssignCallToAgent(1, 99);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AssignCallToAgent_NonExistingCall_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((CallModel)null);

            var result = await _controller.AssignCallToAgent(1, 99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}