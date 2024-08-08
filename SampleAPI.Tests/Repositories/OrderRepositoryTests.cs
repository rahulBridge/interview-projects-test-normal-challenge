using Microsoft.EntityFrameworkCore;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SampleAPI.Tests
{
    public class OrderRepositoryTests
    {
        private SampleApiDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<SampleApiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new SampleApiDbContext(options);
        }

        private OrderRepository CreateRepository(SampleApiDbContext context)
        {
            return new OrderRepository(context);
        }

        [Fact]
        public async Task GetRecentOrdersAsyncs()
        {
            var context = CreateInMemoryContext();
            var repository = CreateRepository(context);

            context.Orders.Add(new Order
            {
                Id = 1,
                Date = DateTime.UtcNow.AddHours(-1),
                IsDeleted = false,
                Description = "Veg", 
                Name = "Veg Biriyani" 
            });
            context.Orders.Add(new Order
            {
                Id = 2,
                Date = DateTime.UtcNow.AddDays(-2),
                IsDeleted = false,
                Description = "Non-veg",
                Name = "Chicken Biriyani" 
            });
            
            await context.SaveChangesAsync();

            var recentOrders = await repository.GetRecentOrdersAsync();

            Assert.Single(recentOrders); 
            var order = recentOrders.First();
            Assert.Equal(1, order.Id);
        }

        [Fact]
        public async Task AddNewOrderAsync()
        {
            var context = CreateInMemoryContext();
            var repository = CreateRepository(context);

            var newOrder = new Order
            {
                Date = DateTime.UtcNow.AddHours(-1),
                IsDeleted = false,
                Description = "Veg",
                Name = "Veg Biriyani"
            };

            var addedOrder = await repository.AddNewOrderAsync(newOrder);

            Assert.NotNull(addedOrder);
            Assert.Equal(newOrder.Date, addedOrder.Date);
            Assert.Equal(newOrder.Description, addedOrder.Description); 
            Assert.Equal(newOrder.Name, addedOrder.Name);
        }


        [Fact]
        public async Task DeleteOrderAsync()
        {
            var context = CreateInMemoryContext();
            var repository = CreateRepository(context);

            var order = new Order
            {
                Id = 1,
                Date = DateTime.UtcNow.AddHours(-1),
                IsDeleted = false,
                Description = "Veg",
                Name = "Veg Biriyani"
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            await repository.DeleteOrderAsync(order);

            var deletedOrder = await context.Orders.FindAsync(order.Id);
            Assert.Null(deletedOrder); 
        }
    }
}
