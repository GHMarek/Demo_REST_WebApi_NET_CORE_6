using System.ComponentModel.DataAnnotations;

namespace DemoRESTWebApi.Models
{
    public class CreateLibraryDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasEbooks { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        public string Street { get; set; }
        public string PostalCode { get; set; }

        // Other possible attributes in ASP .NET are ie. [CreditCard],
        // [Compare("otherProperty")], [EmailAddress], [Phone], [Range(min, max), [RegularExpression(pattern)]]
    }
}
