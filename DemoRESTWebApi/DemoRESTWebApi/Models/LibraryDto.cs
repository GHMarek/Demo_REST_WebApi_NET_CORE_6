namespace DemoRESTWebApi.Models
{
    public class LibraryDto
    {
        // DTO object - Data Transfer Object - meant to transfer data from entities to client.
        // Obviously it's crucial to mark properties as public.
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasEbooks { get; set; }


        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public List<BookDto> Books { get; set; } 
    }
}
