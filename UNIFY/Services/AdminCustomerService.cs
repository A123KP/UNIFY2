using UNIFY.Data;
using UNIFY.Models;
using Microsoft.EntityFrameworkCore;
namespace UNIFY.Services
{
    public class AdminCustomerService : IAdminCustomerService
    {
        private readonly AppDbContext _context;

        public AdminCustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrdersWithUsersAsync()
        {
            return await _context.Orders
                .Include(o => o.User) // Ensures User.Username is available
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersForUserAsync(int userId)
        {
            return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.User)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
        }
        // orderdetail model form
        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product) // Optional: if you need product name, etc.
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }
        //delete from order /orderitems
        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems) // Include to delete children if necessary
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                throw new Exception("Order not found");

            //used ondelete cascade soo no need to delete from orderitems
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }



        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            if (Enum.TryParse<OrderStatus>(newStatus, true, out var parsedStatus))
            {
                order.Status = parsedStatus;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Invalid status value");
            }
        }
    }

}
