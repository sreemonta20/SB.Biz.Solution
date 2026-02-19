using Microsoft.EntityFrameworkCore;
using OrderMgtSystem.Data;
using OrderMgtSystem.Models;

namespace OrderMgtSystem.Services
{
    /// <summary>
    /// This Service is used to handle all the operations such as create, view, and fetch the list of products. In other words, it used for product management.
    /// </summary>
    public class ProductService: IProductService
    {
        #region Variable & Constructor decalation & initialization
        private readonly OrderMgtDBContext _context;

        public ProductService(OrderMgtDBContext context)
        {
            _context = context;
        }
        #endregion

        #region All the Product Service methods
        /// <summary>
        /// Asynchronously retrieves all products from the database, ordered by their creation date in descending order.
        /// </summary>
        /// <remarks>This method performs a non-blocking database query, which can help maintain
        /// application responsiveness in UI or web scenarios.</remarks>
        /// <returns>A collection of <see cref="Product"/> objects representing all products in the database, ordered from newest
        /// to oldest. The collection is empty if no products are found.</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.OrderByDescending(p => p.CreatedDate).ToListAsync();
        }

        /// <summary>
        /// Asynchronously retrieves a product with the specified unique identifier.
        /// </summary>
        /// <remarks>Returns null if no product with the specified identifier exists in the data
        /// store.</remarks>
        /// <param name="id">The unique identifier of the product to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the product if found; otherwise,
        /// null.</returns>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        /// <summary>
        /// Creates a new product and saves it to the database asynchronously.
        /// </summary>
        /// <remarks>The CreatedDate property of the product is set to the current date and time before
        /// the product is saved. The product instance returned will reflect any changes made during the save operation,
        /// such as database-generated values.</remarks>
        /// <param name="product">The product to add to the database. Cannot be null. The CreatedDate property will be set to the current date
        /// and time.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created product, including
        /// its assigned creation date.</returns>
        public async Task<Product> CreateProductAsync(Product product)
        {
            product.CreatedDate = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Updates the details of an existing product in the database asynchronously.
        /// </summary>
        /// <remarks>If no product with the specified ProductId exists, the method completes without
        /// making any changes. This method does not throw an exception if the product is not found.</remarks>
        /// <param name="product">The product entity containing the updated information. The ProductId property must correspond to an existing
        /// product in the database. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous update operation.</returns>
        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.StockQuantity = product.StockQuantity;

                _context.Update(existingProduct);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes the product with the specified identifier from the data store asynchronously.
        /// </summary>
        /// <remarks>If no product with the specified identifier exists, the method completes without
        /// performing any action.</remarks>
        /// <param name="id">The unique identifier of the product to delete. Must correspond to an existing product.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        #endregion
    }
}
