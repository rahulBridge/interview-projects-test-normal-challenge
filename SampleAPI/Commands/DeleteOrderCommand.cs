using MediatR;

namespace SampleAPI.Commands
{
    public class DeleteOrderCommand  : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
