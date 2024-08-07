using SampleAPI.Entities;

namespace SampleAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetRecentOrdersAsync();
        Task<Order> AddNewOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int Id);
        Task DeleteOrderAsync(Order order);
    }
}