using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderMgtSystem.Models
{
    /// <summary>
    /// This is the Order (relational database objects) class
    /// </summary>
    public class Order
    {
        ///// <summary>
        ///// Primary key for the Order
        ///// </summary>
        //[Key]
        //public int OrderId { get; set; }

        ///// <summary>
        ///// CustomerId - Foreign Key field
        ///// </summary>
        //[ForeignKey("Customer")]
        //public virtual int CustomerId { get; set; }

        ///// <summary>
        ///// Audit field: Date and time when Order was created
        ///// </summary>
        //public DateTime OrderDate { get; set; } = DateTime.Now;

        ///// <summary>
        ///// Order Total amount (All the order items amount total is equivalent to item quantity * item price)
        ///// </summary>
        //public decimal TotalAmount { get; set; }

        ///// <summary>
        ///// Order status (Pending, Completed, Cancelled)
        ///// </summary>
        //[Required(ErrorMessage = "Order status is required")]
        //public OrderStatus Status { get; set; } = OrderStatus.Pending;

        //public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        //public virtual Customer? Customer { get; set; }
        [Key]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
