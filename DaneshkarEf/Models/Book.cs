namespace DaneshkarEf.Models
{
    public class Book
    {
        public int Id { get; set; }  
        public required string Title { get; set; } 
        public int PublisherId { get; set; }  
        public decimal Price { get; set; }  
        public DateTime PublicationDate { get; set; } 
        public int LanguageId { get; set; }  
        public  ICollection<BookAuthor>? Authors { get; set; } 
        public  List<string>? Genres { get; set; }  
        public required string Description { get; set; }  
        public int Stock { get; set; }  
    }


}
