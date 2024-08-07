//using MediatR;
using MediatR;
using SampleAPI.Commands;
using SampleAPI.Repositories;

namespace SampleAPI.Handler
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteOrderHandler> _logger;

        public DeleteOrderHandler(IOrderRepository orderRepository, ILogger<DeleteOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {

            var order = await _orderRepository.GetOrderByIdAsync(command.Id);
            if (order == null)
            {
                _logger.LogError($"Order with ID {command.Id} not found");
                throw new KeyNotFoundException($"Order with ID {command.Id} not found");
                //return default;
            }

            _logger.LogInformation($"Deleting order with ID {command.Id}");
            await _orderRepository.DeleteOrderAsync(order);
            return await Task.FromResult(Unit.Value);
        }
    }
}
