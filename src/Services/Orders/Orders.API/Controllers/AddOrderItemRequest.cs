namespace Orders.API.Controllers
{
    public class AddOrderItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}