using SampleAPI.Commands;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using MediatR;

namespace SampleAPI.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<Order> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderDetails = new Order()
            {
                Date = command.Date,
                Description = command.Description,
                Name = command.Name,
                IsInvoiced = command.IsInvoiced,
                IsDeleted = command.IsDeleted
            };

            return await _orderRepository.AddNewOrderAsync(orderDetails);
        }
    }
}