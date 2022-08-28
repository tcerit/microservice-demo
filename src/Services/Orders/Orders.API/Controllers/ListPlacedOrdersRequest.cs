namespace Orders.API.Controllers
{
    public class ListPlacedOrdersRequest
    {
        public DateTime DateStart { get; set; } = DateTime.MinValue;
        public DateTime DateEnd { get; set; } = DateTime.MaxValue;
    }
}