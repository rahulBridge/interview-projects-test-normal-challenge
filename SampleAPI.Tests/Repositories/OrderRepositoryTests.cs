using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using SampleAPI.Requests;
using System.Data.Entity;

namespace SampleAPI.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly Mock<DbSet<Order>> _mockSet;
        private readonly Mock<SampleApiDbContext> _mockContext;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            _mockSet = new Mock<DbSet<Order>>();
            _mockContext = new Mock<SampleApiDbContext>();
            _mockContext.Setup(c => c.Orders).Returns(_mockSet.Object);
            _repository = new OrderRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetRecentOrdersAsync()
        {
            var orders = new List<Order>
            {
                new Order { Id = 1, Date = DateTime.Now.AddDays(-1), Description = "Veg", Name = "Veg Biriyani", IsDeleted = false, IsInvoiced = true },
                new Order { Id = 2, Date = DateTime.Now.AddDays(-2), Description = "Non-Veg", Name = "Egg Biriyani", IsDeleted = false, IsInvoiced = true }
            }.AsQueryable();

            _mockSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(orders.Provider);
            _mockSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(orders.Expression);
            _mockSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(orders.ElementType);
            _mockSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(orders.GetEnumerator());

            var result = await _repository.GetRecentOrdersAsync();

            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task AddNewOrderAsync()
        {
            var order = new Order { Id = 1, Date = DateTime.UtcNow, IsDeleted = false };
            _mockSet.Setup(m => m.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>())).ReturnsAsync(new EntityEntry<Order>(order));

            var result = await _repository.AddNewOrderAsync(order);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task DeleteOrderAsync()
        {
            var order = new Order { Id = 1, Date = DateTime.UtcNow, IsDeleted = false };
            _mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(order);

            await _repository.DeleteOrderAsync(1);

            _mockSet.Verify(m => m.Remove(It.IsAny<Order>()), Times.Once);
        }
    }
}