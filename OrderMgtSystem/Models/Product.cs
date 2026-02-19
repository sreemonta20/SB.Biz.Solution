using System.ComponentModel.DataAnnotations;

namespace OrderMgtSystem.Models
{
    /// <summary>
    /// This is the Product (relational database objects) class
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Primary key for the Product
        /// </summary>
        [Key]
        public int ProductId { get; set; }

        /// <summary>
        /// Product Name - required field
        /// </summary>
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 2,ErrorMessage = "Product name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product Name
        /// </summary>
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Product description must be between 2 and 200 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Product Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Product Stock Quantity
        /// </summary>
        public decimal StockQuantity { get; set; }

        /// <summary>
        /// Audit field: Date and time when Product was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
