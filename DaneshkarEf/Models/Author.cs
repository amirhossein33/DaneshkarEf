namespace DaneshkarEf.Models
{
    public class Author
    {
        public int Id { get; set; }  
        public required string Name { get; set; } 
        public required string Bio { get; set; }  
        public DateTime? Dob { get; set; }  
        public required string Nationality { get; set; }  
    }


}
