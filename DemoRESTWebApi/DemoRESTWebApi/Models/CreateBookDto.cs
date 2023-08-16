using System.ComponentModel.DataAnnotations;

namespace DemoRESTWebApi.Models
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int LibraryId { get; set;}
    }
}
