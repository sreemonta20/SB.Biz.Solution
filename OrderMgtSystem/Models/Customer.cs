using System.ComponentModel.DataAnnotations;

namespace OrderMgtSystem.Models
{
    /// <summary>
    /// This is the Customer (relational database objects) class
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Primary key for the Customer
        /// </summary>
        [Key]
        public int CustomerId { get; set; }

        /// <summary>
        /// Customer First Name - required field
        /// </summary>
        [Required(ErrorMessage = "Customer first name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Customer first name must be between 2 and 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Customer Last Name - required field
        /// </summary>
        [Required(ErrorMessage = "Customer last name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Customer last name must be between 2 and 100 characters")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Customer Email - required and must be valid email format
        /// </summary>
        [Required(ErrorMessage = "Customer email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Customer phone number - required and valid format
        /// </summary>
        [Required(ErrorMessage = "Customer phone is required")]
        [RegularExpression(@"^[0-9\-\+]{10,13}$", ErrorMessage = "Phone number must be between 10-13 digits")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Audit field: Date and time when customer was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
