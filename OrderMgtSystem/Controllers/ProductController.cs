using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderMgtSystem.Data;
using OrderMgtSystem.Models;
using OrderMgtSystem.Services;
using System.Diagnostics;

namespace OrderMgtSystem.Controllers
{
    /// <summary>
    /// Provides actions for creating, editing, deleting, and viewing products within the application.
    /// </summary>
    /// <remarks>The ProductController relies on an injected IProductService to perform product-related
    /// operations. It validates model state before processing create and edit requests, and uses TempData to
    /// communicate operation success to the user interface. All actions are designed to support standard CRUD
    /// operations for products in an ASP.NET MVC application.</remarks>
    public class ProductController : Controller
    {
        #region Variable & Constructor decalation & initialization
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region All the action or non action methods
        /// <summary>
        /// Asynchronously retrieves all available products and displays them in the default view.
        /// </summary>
        /// <remarks>This action method obtains the product list by calling the product service
        /// asynchronously. Ensure that the product service is properly configured before invoking this
        /// method.</remarks>
        /// <returns>An IActionResult that renders the view populated with the list of products.</returns>
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        /// <summary>
        /// Returns a view for creating a new product, initializing the view with a default Product instance.
        /// </summary>
        /// <returns>An IActionResult that renders the product creation view with a new Product model.</returns>
        public IActionResult CreateProduct() => View(new Product());

        /// <summary>
        /// Creates a new product and adds it to the database.
        /// </summary>
        /// <remarks>A success message is stored in TempData upon successful creation. If the model state
        /// is invalid, the method returns the view with the current product data to allow the user to correct input
        /// errors.</remarks>
        /// <param name="product">The product to create. Must satisfy all model validation requirements; otherwise, the operation will not
        /// proceed.</param>
        /// <returns>An IActionResult that redirects to the index view if the product is created successfully; otherwise, returns
        /// the view with validation errors for correction.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            await _productService.CreateProductAsync(product);
            TempData["Success"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Returns the view for editing the specified product.
        /// </summary>
        /// <remarks>If the specified product does not exist, the method returns a 404 Not Found
        /// response.</remarks>
        /// <param name="id">The unique identifier of the product to edit. Must not be null.</param>
        /// <returns>An IActionResult that renders the edit view for the product if found; otherwise, a NotFound result.</returns>
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null) return NotFound();
            var product = await _productService.GetProductByIdAsync(id.Value);
            return product == null ? NotFound() : View(product);
        }

        /// <summary>
        /// Updates the details of an existing product with the specified information.
        /// </summary>
        /// <remarks>Returns a NotFound result if the provided id does not match the ProductId of the
        /// product. If the model state is invalid, the view is returned to allow for user corrections.</remarks>
        /// <param name="id">The unique identifier of the product to update. Must match the ProductId of the provided product.</param>
        /// <param name="product">An object containing the updated product information. Must satisfy all model validation requirements.</param>
        /// <returns>A redirect to the Index view if the update is successful; otherwise, returns the view with the current
        /// product data for correction.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, Product product)
        {
            if (id != product.ProductId) return NotFound();
            if (!ModelState.IsValid) return View(product);

            await _productService.UpdateProductAsync(product);
            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the delete confirmation view for a product specified by its identifier.
        /// </summary>
        /// <remarks>If the specified product identifier is null or the product does not exist, the method
        /// returns a NotFound result. This method does not perform the actual deletion; it only displays the
        /// confirmation view.</remarks>
        /// <param name="id">The unique identifier of the product to delete. Must not be null.</param>
        /// <returns>An IActionResult that renders the delete confirmation view for the specified product if found; otherwise, a
        /// NotFound result.</returns>
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null) return NotFound();
            var product = await _productService.GetProductByIdAsync(id.Value);
            return product == null ? NotFound() : View(product);
        }

        /// <summary>
        /// Deletes the specified product and redirects to the product list view.
        /// </summary>
        /// <remarks>This action requires a valid anti-forgery token and is intended to be invoked via a
        /// POST request. Upon successful deletion, a success message is stored in TempData for display after
        /// redirection.</remarks>
        /// <param name="id">The unique identifier of the product to delete. Must correspond to an existing product.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult that redirects
        /// to the index view after successful deletion.</returns>
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            TempData["Success"] = "Product deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Retrieves the details of a product with the specified identifier.
        /// </summary>
        /// <remarks>This method asynchronously obtains product information from the product service. If
        /// the specified <paramref name="id"/> is null or does not correspond to an existing product, a NotFound result
        /// is returned.</remarks>
        /// <param name="id">The unique identifier of the product to retrieve. Must not be null.</param>
        /// <returns>An <see cref="IActionResult"/> that displays the product details if found; otherwise, a NotFound result.</returns>
        public async Task<IActionResult> ProductDetails(int? id)
        {
            if (id == null) return NotFound();
            var product = await _productService.GetProductByIdAsync(id.Value);
            return product == null ? NotFound() : View(product);
        }
        #endregion
    }
}
