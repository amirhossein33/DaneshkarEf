namespace DaneshkarEf.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } 
        public Customer? Customer { get; set; } 

        public DateTime OrderDate { get; set; }
        public required string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public required string PaymentMethod { get; set; }

        public ICollection<OrderItem>? Items { get; set; }

    }


}
