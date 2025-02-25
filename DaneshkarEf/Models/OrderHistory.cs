namespace DaneshkarEf.Models
{
    public class OrderHistory
    {
        public int Id { get; set; }  
        public int OrderId { get; set; }  
        public Order? Order { get; set; }  
        public required string Status { get; set; } 
        public DateTime UpdateTime { get; set; }  
      
    }


}
