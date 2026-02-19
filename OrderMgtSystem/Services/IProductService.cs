using OrderMgtSystem.Models;

namespace OrderMgtSystem.Services
{
    /// <summary>
    /// Defines a contract for managing products, including operations to retrieve, create, update, and delete product
    /// entities asynchronously.
    /// </summary>
    /// <remarks>Implementations of this interface should ensure thread safety and handle exceptions that may
    /// occur during data access operations. All methods are asynchronous and intended for use in scalable, non-blocking
    /// application scenarios.</remarks>
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
