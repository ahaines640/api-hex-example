namespace Example.Domain.Orders
{
    public class Order : Model
    {
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; }
    }
}