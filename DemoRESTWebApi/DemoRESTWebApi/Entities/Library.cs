namespace DemoRESTWebApi.Entities
{
    public class Library
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasEbooks { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public int AddressId { get; set; }
        public int? CreatedById { get; set; }

        // Virtual methods provide access to objects related to class.

        public virtual User CreatedBy { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<Book> Books { get; set; }


    }



}
