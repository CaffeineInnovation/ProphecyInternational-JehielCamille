using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Server.Controllers;
using ProphecyInternational.Server.Interfaces;
using ProphecyInternational.Common.Enums;
using ProphecyInternational.Server.Models;

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

        [Fact]
        public async Task GetPaginatedCalls_ValidPage_ReturnsOkWithPaginatedData()
        {
            // Arrange
            var pagedResult = new PagedResult<CallModel>
            {
                Items = new List<CallModel>
        {
            new CallModel { Id = 1, CustomerId = "1001", AgentId = 201, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(10), Status = CallStatus.Completed, Notes = "Call 1" }
        },
                TotalCount = 3,
                PageNumber = 1,
                PageSize = 1
            };

            _mockService.Setup(s => ((IPagedGenericService<CallModel>)s).GetAllPaginatedAsync(1, 1))
                        .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetPaginatedCalls(1, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedData = Assert.IsType<PagedResult<CallModel>>(okResult.Value);

            Assert.Equal(3, returnedData.TotalCount);
            Assert.Equal(1, returnedData.PageNumber);
            Assert.Equal(1, returnedData.PageSize);
            Assert.Single(returnedData.Items);
        }

        [Fact]
        public async Task GetPaginatedCalls_PageOutOfRange_ReturnsEmptyList()
        {
            // Arrange: Simulate an out-of-range page request
            var pagedResult = new PagedResult<CallModel>
            {
                Items = new List<CallModel>(), // No data for this page
                TotalCount = 2,
                PageNumber = 5, // Requested page is out of range
                PageSize = 1
            };

            _mockService.Setup(s => ((IPagedGenericService<CallModel>)s).GetAllPaginatedAsync(5, 1))
                        .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetPaginatedCalls(5, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedData = Assert.IsType<PagedResult<CallModel>>(okResult.Value);

            Assert.Equal(2, returnedData.TotalCount);
            Assert.Equal(5, returnedData.PageNumber);
            Assert.Equal(1, returnedData.PageSize);
            Assert.Empty(returnedData.Items);
        }

        [Fact]
        public async Task GetPaginatedCalls_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange: Simulate an exception being thrown
            _mockService.Setup(s => ((IPagedGenericService<CallModel>)s).GetAllPaginatedAsync(It.IsAny<int>(), It.IsAny<int>()))
                        .ThrowsAsync(new Exception("Database failure"));

            // Act
            var result = await _controller.GetPaginatedCalls(1, 1);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);

            // Verify logger was called
            _mockLogger.Verify(
                x => x.LogError(It.IsAny<Exception>(), "Error occurred while getting calls."),
                Times.Once
            );
        }

    }
}