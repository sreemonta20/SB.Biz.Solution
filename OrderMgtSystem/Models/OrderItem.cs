using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderMgtSystem.Models
{
    /// <summary>
    /// This is the OrderItem (relational database objects) class
    /// </summary>
    public class OrderItem
    {
        ///// <summary>
        ///// Primary key for the Customer
        ///// </summary>
        //[Key]
        //public int OrderItemId { get; set; }

        ///// <summary>
        ///// OrderId - Foreign Key field
        ///// </summary>
        //[ForeignKey("Order")]
        //public int OrderId { get; set; }

        ///// <summary>
        ///// ProductId - Foreign Key field
        ///// </summary>
        //[ForeignKey("Product")]
        //public virtual int ProductId { get; set; }

        ///// <summary>
        ///// Order Item Quantity
        ///// </summary>
        //public decimal Quantity { get; set; }

        ///// <summary>
        ///// Order Item Unit Price
        ///// </summary>
        //public decimal UnitPrice { get; set; }

        ///// <summary>
        ///// Order Item Line Total (Item Quantity * Item Unit Price)
        ///// </summary>
        //public decimal LineTotal { get; set; }

        //public virtual Order? Order { get; set; }

        //public virtual Product? Product { get; set; }

        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; } // Removed 'virtual' from int

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
