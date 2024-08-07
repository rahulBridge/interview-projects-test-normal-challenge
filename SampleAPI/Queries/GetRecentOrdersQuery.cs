using MediatR;
using SampleAPI.Entities;

namespace SampleAPI.Queries
{
    public class GetRecentOrdersQuery : IRequest<IEnumerable<Order>>
    {
    }
    
}
