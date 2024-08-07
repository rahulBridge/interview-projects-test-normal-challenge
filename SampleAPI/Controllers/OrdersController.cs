using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Commands;
using SampleAPI.Entities;
using SampleAPI.Handler;
using SampleAPI.Queries;
using SampleAPI.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            this._mediator = mediator;
            this._logger = logger;
        }

        [HttpGet("")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            try
            {
                var orders = await _mediator.Send(new GetRecentOrdersQuery());
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            try
            {
                var orderDetails = await _mediator.Send(new CreateOrderCommand(
                            order.Id,
                            order.Date,
                            order.Description,
                            order.Name,
                            order.IsInvoiced,
                            order.IsDeleted
                        )
                    );

                return CreatedAtAction(nameof(GetOrders), orderDetails);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error deleting order : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                await _mediator.Send(new DeleteOrderCommand() { Id = id });
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, $"Order with ID {id} not found");
                return NotFound(new { Message = ex.Message });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error deleting order : {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}