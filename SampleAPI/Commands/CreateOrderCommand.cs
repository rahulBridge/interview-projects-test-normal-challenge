using SampleAPI.Entities;
using MediatR;
using System.Numerics;

namespace SampleAPI.Commands
{
    public class CreateOrderCommand : IRequest<Order>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsInvoiced { get; set; }
        public bool IsDeleted { get; set; }
        public CreateOrderCommand(int id, DateTime date, string description, string name, bool isInvoiced, bool isDeleted)
        {
            Id = id;
            Date = date;
            Description = description;
            Name = name;
            IsInvoiced = isInvoiced;
            IsDeleted = isDeleted;
        }
    }
}
