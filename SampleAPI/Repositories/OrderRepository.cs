using Microsoft.EntityFrameworkCore;
using SampleAPI.Entities;
using System.Collections.Generic;
using System.Threading;

namespace SampleAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SampleApiDbContext _context;

        public OrderRepository(SampleApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetRecentOrdersAsync()
        {
            return await _context.Orders.Where(o => !o.IsDeleted && o.Date > DateTime.Now.Date.AddDays(-1)).OrderBy(o => o.Date).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int Id)
        {
            return await _context.Orders.FindAsync(Id);
        }

        public async Task<Order> AddNewOrderAsync(Order order)
        {
            var result = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}