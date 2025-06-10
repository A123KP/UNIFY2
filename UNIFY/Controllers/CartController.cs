// controller/Cart.cs
using Microsoft.AspNetCore.Mvc;
using UNIFY.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using UNIFY.Models; // For UNIFY.Models.Cart
using System.Linq;
using Microsoft.AspNetCore.Http; // For ISession
using Microsoft.Extensions.Logging; // For ILogger
using Serilog;
using Microsoft.AspNetCore.Authorization;
using UNIFY.Auth; // For Serilog

namespace UNIFY.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        //private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly ILogger<CartController> _logger;
        //put this inside cartController  IOrderService orderService
        public CartController(ICartService cartService, IProductService productService, IUserService userService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _productService = productService;
            //_orderService = orderService;
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessCheckout([FromBody] List<Cart> cartItemsFromClient)
        {
            _logger.LogInformation("ProcessCheckout action started.");

            if (cartItemsFromClient == null || !cartItemsFromClient.Any())
            {
                _logger.LogWarning("ProcessCheckout: Cart is empty or null.");
                return Json(new { success = false, message = "Cart is empty." });
            }

            // --- User ID Retrieval ---

            // Step 1: Use the service to check if the user is properly logged in.
            if (!_userService.IsUserLoggedIn(HttpContext))
            {
                _logger.LogWarning("ProcessCheckout: User not logged in. Redirecting to login page.");
                TempData["ErrorMessage"] = "You need to be logged in to proceed to checkout.";
                return Json(new { success = false, message = "User not logged in.", redirectUrl = Url.Action("Login", "User") });
            }

            // Step 2: Since the user is logged in, now get their ID using the service.
            var userIdFromSession = _userService.GetUserIdFromSession(HttpContext);

            // This is a safety check. This block should not be reached if IsUserLoggedIn is true.
            if (!userIdFromSession.HasValue)
            {
                _logger.LogError("ProcessCheckout: Inconsistent session state. IsUserLoggedIn was true, but GetUserIdFromSession was null.");
                return Json(new { success = false, message = "A critical session error occurred. Please log in again.", redirectUrl = Url.Action("Login", "User") });
            }

            int currentUserId = userIdFromSession.Value;
            _logger.LogInformation($"ProcessCheckout: Processing for UserId: {currentUserId}");

            // Prepare the list to be sent to the service (set UserId, clear nav props etc.)
            foreach (var item in cartItemsFromClient)
            {
                item.UserId = currentUserId;
                item.CartId = 0;
                item.User = null;
                item.Product = null;
                _logger.LogInformation($"ProcessCheckout: Item to process - ProductId: {item.ProductId}, Quantity: {item.Quantity}, UserId set to: {item.UserId}");
            }

            try
            {
                await _cartService.SaveCartAsync(cartItemsFromClient); // Pass the list of Cart entities

                _logger.LogInformation($"ProcessCheckout: Cart saved successfully for UserId: {currentUserId}.");

                //var orderId = await _orderService.CreateOrderAsync(currentUserId);
                //_logger.LogInformation($"ProcessCheckout: Order created successfully with OrderId: {orderId}.");

                string redirectUrl = Url.Action("Payment", "Payment"); // Ensure PaymentController and Payment action exist
                _logger.LogInformation($"ProcessCheckout: Redirecting to {redirectUrl}");

                return Json(new { success = true, message = "Cart processed successfully! Go to Payment", redirectUrl = redirectUrl });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"ProcessCheckout: Error occurred during checkout process for UserId: {currentUserId}.");
                return Json(new { success = false, message = "An error occurred during checkout: " + ex.Message });
            }
        }
    }
}