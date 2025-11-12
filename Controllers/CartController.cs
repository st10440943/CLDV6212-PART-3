using ABC_Retail.Helpers;
using ABC_Retail.Models;
using ABC_Retail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Retail.Controllers
{
    public class CartController : Controller
    {
        private readonly TableStorageService _tables;

        public CartController(TableStorageService tables)
        {
            _tables = tables;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(string productId, string productName, double price)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existing = cart.FirstOrDefault(c => c.ProductId == productId);
            if (existing != null)
                existing.Quantity++;
            else
                cart.Add(new CartItem { ProductId = productId, ProductName = productName, Price = price });

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            TempData["Message"] = "✅ Product added to cart!";
            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
            {
                TempData["Message"] = "🛒 Your cart is empty!";
                return RedirectToAction("Index", "Products");
            }

            foreach (var item in cart)
            {
                var order = new OrderEntity
                {
                    CustomerId = Guid.NewGuid().ToString(), 
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    TotalPrice = (decimal)(item.Price * item.Quantity),
                    CustomerName = "Test Customer",
                    ProductName = item.ProductName,
                    Status = "Pending",
                    CreatedDate = DateTimeOffset.UtcNow
                };

                await _tables.UpsertOrderAsync(order);
            }

            HttpContext.Session.Remove("Cart");
            TempData["Message"] = "✅ Your order has been placed successfully!";
            return RedirectToAction("Index", "Products");
        }
    }
}
