namespace Orders.API.Controllers
{
    public class AddOrderItemRequest
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}