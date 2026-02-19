using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderMgtSystem.Data;
using OrderMgtSystem.Models;
using OrderMgtSystem.Request;

namespace OrderMgtSystem.Controllers
{
    /// <summary>
    /// Provides endpoints for managing customer orders, including listing orders, displaying order details, and
    /// creating new orders in the system.
    /// </summary>
    /// <remarks>The OrderController uses dependency injection to access the OrderMgtDBContext for performing
    /// CRUD operations on orders and related entities. It supports asynchronous operations to improve application
    /// responsiveness. Actions include retrieving all orders, viewing order item details, and creating new orders with
    /// validation and transaction support. This controller is intended for use in an ASP.NET MVC application and
    /// assumes that related models and database context are properly configured.</remarks>
    public class OrderController : Controller
    {
        #region Variable & Constructor declaration and initialization
        private readonly OrderMgtDBContext _context;
        /// <summary>
        /// Constructor with dependency injection for database context
        /// </summary>
        public OrderController(OrderMgtDBContext context)
        {
            _context = context;
        }
        #endregion

        #region All the action or non action methods
        /// <summary>
        /// Asynchronously retrieves a list of orders, including associated customer information, and returns a view
        /// displaying the results.
        /// </summary>
        /// <remarks>This action method queries the database for orders and their related customers. If an
        /// exception is encountered during data retrieval, an empty order list is provided to the view instead of
        /// propagating the error.</remarks>
        /// <returns>A view that contains a list of orders with related customer data. If an error occurs during retrieval, the
        /// view contains an empty list.</returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = await _context.Orders.Include(o => o.Customer).ToListAsync();
                return View(orders);
            }
            catch (Exception)
            {
                return View(new List<Order>());
            }
        }

        /// <summary>
        /// Retrieves the details of a specific order, including associated customer and product information, for
        /// display in a partial view.
        /// </summary>
        /// <remarks>This method uses eager loading to include related customer and product data for the
        /// specified order. If the order does not exist or an exception occurs during retrieval, a NotFound result is
        /// returned.</remarks>
        /// <param name="id">The unique identifier of the order to retrieve. Must be a valid order ID; if null, the method returns a
        /// NotFound result.</param>
        /// <returns>A partial view containing the order details if the order is found; otherwise, a NotFound result.</returns>
        [HttpGet]
        public async Task<IActionResult> OrderItemDetails(int? id)
        {
            try
            {
                // Eager Loading: We use .Include to fetch related data
                var order = await _context.Orders
                            .Include(o => o.Customer)
                            .Include(o => o.OrderItems)
                                .ThenInclude(oi => oi.Product) // This fetches the Product for each OrderItem
                            .FirstOrDefaultAsync(o => o.OrderId == id);

                if (order == null)
                {
                    return NotFound();
                }

                return PartialView("_OrderDetailsPartial", order);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Displays the order creation view, initializing view data with available customers and products.
        /// </summary>
        /// <remarks>The view data includes a list of customers and a list of products with available
        /// stock. If an exception is thrown during data retrieval, the view is still displayed but without populated
        /// product data.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult that renders
        /// the order creation view. If an error occurs while retrieving customer or product data, the view is rendered
        /// with an empty product instance.</returns>
        public async Task<IActionResult> CreateOrder()
        {
            
            try
            {
                ViewBag.CustomerId = new SelectList(_context.Customers, "CustomerId", "FirstName");
                // We pass products to a JS-friendly list for the dynamic dropdown
                ViewBag.ProductList = await _context.Products.Where(p => p.StockQuantity > 0).ToListAsync();
                return View();
            }
            catch (Exception)
            {
                return View(new Product());
            }
        }

        /// <summary>
        /// Creates a new order for the specified customer using the provided order details.
        /// </summary>
        /// <remarks>The method validates that all requested products exist and have sufficient stock
        /// before finalizing the order. If any product is unavailable or stock is insufficient, the operation is rolled
        /// back and an error message is returned. The entire operation is performed within a database transaction to
        /// ensure consistency.</remarks>
        /// <param name="request">An object containing the customer identifier and a collection of order items to be processed. Cannot be null
        /// and must include at least one item.</param>
        /// <returns>A JSON result indicating whether the order was placed successfully, along with a message describing the
        /// outcome.</returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateOrder(int CustomerId, List<OrderItem> items)
        //{
        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            // Order master object initialization & save

        //            Order newOrder = new Order
        //            {
        //                CustomerId = CustomerId,
        //                OrderDate = DateTime.Now,
        //                Status = OrderStatus.Completed,
        //                TotalAmount = 0 
        //            };

        //            _context.Orders.Add(newOrder);
        //            await _context.SaveChangesAsync(); 

        //            decimal totalAmount = 0;

        //            foreach (var item in items)
        //            {
        //                var product = await _context.Products.FindAsync(item.ProductId);

        //                // Business Rule 2 (Cannot create order if product does not exist or stock is insufficient
        //                if (product == null || product.StockQuantity < item.Quantity)
        //                {
        //                    throw new Exception($"Stock insufficient for {product?.Name ?? "Product"}.");
        //                }

        //                //Save each order item property with product price and product quantity (calculation of each Item)
        //                item.OrderId = newOrder.OrderId;
        //                item.UnitPrice = product.Price;
        //                item.LineTotal = item.UnitPrice * item.Quantity;
        //                totalAmount += item.LineTotal;

        //                // Reducing the Product Stock
        //                product.StockQuantity -= item.Quantity;

        //                _context.OrderItems.Add(item);
        //            }

        //            // Final save of the Order master object  with total amount
        //            newOrder.TotalAmount = totalAmount;
        //            await _context.SaveChangesAsync();

        //            transaction.Commit();
        //            return Json(new { success = true, message = "Order placed successfully!" });
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            return Json(new { success = false, message = ex.Message });
        //        }
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        //{
        //    // Check if the request is null
        //    if (request == null || request.OrderItems == null || !request.OrderItems.Any())
        //    {
        //        return Json(new { success = false, message = "Invalid order data." });
        //    }

        //    using (var transaction = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            // Order master object initialization & save
        //            Order newOrder = new Order
        //            {
        //                CustomerId = request.CustomerId,
        //                OrderDate = DateTime.Now,
        //                Status = OrderStatus.Completed,
        //                TotalAmount = 0
        //            };

        //            _context.Orders.Add(newOrder);
        //            await _context.SaveChangesAsync(); // Generates the OrderId

        //            decimal runningTotal = 0;

        //            // Process Order Items from the Request
        //            foreach (var item in request.OrderItems)
        //            {
        //                var product = await _context.Products.FindAsync(item.ProductId);

        //                if (product == null || product.StockQuantity < item.Quantity)
        //                {
        //                    throw new Exception($"Stock insufficient for {product?.Name ?? "Unknown Product"}.");
        //                }

        //                //Save each order item property with product price and product quantity (calculation of each Item)
        //                item.OrderId = newOrder.OrderId;
        //                item.UnitPrice = product.Price;
        //                item.LineTotal = product.Price * item.Quantity;

        //                runningTotal += item.LineTotal;
        //                product.StockQuantity -= item.Quantity;

        //                _context.OrderItems.Add(item);
        //            }

        //            // Final save of the Order master object  with total amount
        //            newOrder.TotalAmount = runningTotal;
        //            await _context.SaveChangesAsync();

        //            transaction.Commit();
        //            return Json(new { success = true, message = "Order placed successfully!" });
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            return Json(new { success = false, message = ex.Message });
        //        }
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            if (request == null || request.OrderItems == null || !request.OrderItems.Any())
                return Json(new { success = false, message = "No items provided." });

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Step1: Initialize the Order Master
                    var newOrder = new Order
                    {
                        CustomerId = request.CustomerId,
                        OrderDate = DateTime.Now,
                        Status = OrderStatus.Completed,
                        TotalAmount = 0
                    };

                    _context.Orders.Add(newOrder);
                    await _context.SaveChangesAsync(); // Generates the OrderId

                    decimal runningTotal = 0;

                    // Step2. Add the Order Items
                    foreach (var itemDto in request.OrderItems)
                    {
                        var product = await _context.Products.FindAsync(itemDto.ProductId);
                        if (product == null) throw new Exception("Product not found.");
                        if (product.StockQuantity < (int)itemDto.Quantity)
                            throw new Exception($"Insufficient stock for {product.Name}.");

                        var orderItem = new OrderItem
                        {
                            OrderId = newOrder.OrderId,
                            ProductId = itemDto.ProductId,
                            Quantity = itemDto.Quantity,
                            UnitPrice = product.Price,
                            LineTotal = product.Price * itemDto.Quantity
                        };

                        runningTotal += orderItem.LineTotal;
                        product.StockQuantity -= (int)itemDto.Quantity; // Update Stock

                        _context.OrderItems.Add(orderItem);
                    }

                    // Step3. Update the final Total and Commit in Order Master and Order Items
                    newOrder.TotalAmount = runningTotal;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return Json(new { success = true, message = "Order placed successfully!" });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = ex.Message });
                }
            }
        }

        #endregion
    }
}
