namespace DaneshkarEf.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PreferredLanguage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

       
        public ICollection<Order>? Orders { get; set; }
        public ICollection<CustomerAddress>? CustomerAddresses { get; internal set; }
    }


}
