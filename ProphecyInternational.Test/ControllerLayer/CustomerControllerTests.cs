using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProphecyInternational.Common.Models;
using ProphecyInternational.Server.Controllers;
using ProphecyInternational.Server.Interfaces;

namespace ProphecyInternational.Test.ControllerLayer
{
    public class CustomerControllerTests
    {
        private readonly Mock<IGenericService<CustomerModel, string>> _mockService;
        private readonly Mock<ILogger<CustomerController>> _mockLogger;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockService = new Mock<IGenericService<CustomerModel, string>>();
            _mockLogger = new Mock<ILogger<CustomerController>>();
            _controller = new CustomerController(_mockLogger.Object, _mockService.Object);
        }

        [Fact]
        public async Task GetAllCustomers_ReturnsOkResult_WithListOfCustomers()
        {
            var customers = new List<CustomerModel> { new CustomerModel { Id = "1" }, new CustomerModel { Id = "2" } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(customers);

            var result = await _controller.GetAllCustomers();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomers = Assert.IsType<List<CustomerModel>>(okResult.Value);
            Assert.Equal(2, returnedCustomers.Count);
        }

        [Fact]
        public async Task GetCustomer_ExistingId_ReturnsOkResult()
        {
            var customer = new CustomerModel { Id = "1" };
            _mockService.Setup(s => s.GetByIdAsync("1")).ReturnsAsync(customer);

            var result = await _controller.GetCustomer("1");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCustomer = Assert.IsType<CustomerModel>(okResult.Value);
            Assert.Equal("1", returnedCustomer.Id);
        }

        [Fact]
        public async Task GetCustomer_NonExistingId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync("1")).ReturnsAsync((CustomerModel)null);

            var result = await _controller.GetCustomer("1");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCustomer_ValidCustomer_ReturnsCreatedAtAction()
        {
            var customer = new CustomerModel { Id = "1" };
            _mockService.Setup(s => s.AddAsync(customer)).Returns(Task.CompletedTask);

            var result = await _controller.CreateCustomer(customer);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetCustomer", createdAtActionResult.ActionName);
            Assert.Equal("1", ((CustomerModel)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task UpdateCustomer_ValidCustomer_ReturnsNoContent()
        {
            var customer = new CustomerModel { Id = "1" };
            _mockService.Setup(s => s.UpdateAsync(customer)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateCustomer("1", customer);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ExistingId_ReturnsNoContent()
        {
            _mockService.Setup(s => s.DeleteAsync("1")).Returns(Task.CompletedTask);

            var result = await _controller.DeleteCustomer("1");

            Assert.IsType<NoContentResult>(result);
        }
    }
}