using MediatR;
using SampleAPI.Entities;
using SampleAPI.Queries;
using SampleAPI.Repositories;

namespace SampleAPI.Handler
{
    public class GetRecentOrdersHandler : IRequestHandler<GetRecentOrdersQuery, IEnumerable<Order>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetRecentOrdersHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> Handle(GetRecentOrdersQuery query, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetRecentOrdersAsync();
        }
    }
}
