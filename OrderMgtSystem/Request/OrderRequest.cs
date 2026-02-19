using OrderMgtSystem.Models;

namespace OrderMgtSystem.Request
{
    /// <summary>
    /// This class is used to transport the data from the client to the Controller, so that controller store it into database.
    /// </summary>
    public class OrderRequest
    {
        public int CustomerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

    /// <summary>
    /// Represents an item within an order, including the product identifier and the quantity ordered.
    /// </summary>
    /// <remarks>Use this data transfer object to encapsulate the details of a single product in an order when
    /// transferring order data between application layers or services.</remarks>
    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
