namespace DemoRESTWebApi.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set;}

        public bool Available { get; set; }

        public int LibraryId { get; set; }
        public virtual Library Library { get; set; }

    }
}
