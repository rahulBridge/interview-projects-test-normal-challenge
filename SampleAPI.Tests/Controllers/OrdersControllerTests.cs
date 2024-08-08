using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SampleAPI.Controllers;
using SampleAPI.Entities;
using SampleAPI.Queries;
using SampleAPI.Repositories;
using SampleAPI.Requests;
using SampleAPI.Commands;

namespace SampleAPI.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<OrdersController>> _loggerMock;

        public OrdersControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<OrdersController>>();
            _controller = new OrdersController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetOrders_ReturnsOrders()
        {
            var orders = new List<Order>
            {
                new Order { Id = 1, Date = DateTime.Now, Description = "Veg", Name = "Veg Biriyani", IsDeleted = false, IsInvoiced = true },
                new Order { Id = 2, Date = DateTime.Now, Description = "Non-Veg", Name = "Egg biriyani", IsDeleted = false, IsInvoiced = true },

            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetRecentOrdersQuery>(), default))
                .ReturnsAsync(orders);

            var result = await _controller.GetOrders();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnOrders = Assert.IsType<List<Order>>(okResult.Value);
            Assert.Equal(2, returnOrders.Count);
        }

        [Fact]
        public async Task CreateOrder_ReturnsOrders()
        {
            var order = new Order { Id = 1, Date = DateTime.Now, Description = "Veg", Name = "Veg Biriyani", IsDeleted = false, IsInvoiced = true };
            var createdOrder = new Order { Id = 1, Date = DateTime.Now, Description = "Veg", Name = "Veg Biriyani", IsDeleted = false, IsInvoiced = true };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), default))
                .ReturnsAsync(createdOrder);

            var result = await _controller.CreateOrder(order);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnOrder = Assert.IsType<Order>(createdAtActionResult.Value);
            Assert.Equal(order.Id, returnOrder.Id);
        }

        [Fact]
        public async Task DeleteOrder()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteOrderCommand>(), default))
                .ReturnsAsync(MediatR.Unit.Value);

            var result = await _controller.DeleteOrder(1);

            result.Should().BeOfType<NoContentResult>();
        }
    }
}