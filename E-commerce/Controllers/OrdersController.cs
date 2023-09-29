using E_commerce.Models;
using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public OrdersController(EcommerceContext context)
        {
            _context = context;
        }

        // Implement GET, POST, PUT, DELETE actions for Orders similarly to Users.
        // Customize the actions to match your specific business logic and data model.
        // GET: api/Orders
        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _context.Orders.ToList();
            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] Order order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest();
                }
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetOrder", new { id = order.Id }, order);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public IActionResult PutOrder(int id, [FromBody] Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return NoContent();
        }

    }

}
