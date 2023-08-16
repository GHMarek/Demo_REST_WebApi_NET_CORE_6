using Bogus;
using Bogus.Extensions;
using DemoRESTWebApi.Entities;

namespace DemoRESTWebApi
{
    public class LibraryDataSeeder
    {
        private readonly LibraryDbContext _dbContext;
        // Class created to seed initial data to db.
        public LibraryDataSeeder(LibraryDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Libraries.Any())
                {
                    var libraries = GetLibraries();
                    _dbContext.Libraries.AddRange(libraries);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Library> GetLibraries()
        {
            return GenerateFakeLibraries(5);
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role(){ Name = "Admin" },
                new Role(){ Name = "User" },
                new Role(){ Name = "Manager" }
            };

            return roles;
        }

        public static List<Library> GenerateFakeLibraries(int numberOfLibraries)
        {
            var faker = new Faker<Library>()
                //.RuleFor(l => l.Id, f => f.UniqueIndex)
                .RuleFor(l => l.Name, f => f.Company.CompanyName().ClampLength(5, 24))
                .RuleFor(l => l.Description, f => f.Lorem.Sentence())
                .RuleFor(l => l.Category, f => f.Commerce.Categories(1)[0])
                .RuleFor(l => l.HasEbooks, f => f.Random.Bool())
                .RuleFor(l => l.ContactEmail, f => f.Internet.Email())
                .RuleFor(l => l.ContactNumber, f => f.Phone.PhoneNumber())
                .RuleFor(l => l.AddressId, f => f.UniqueIndex) // Use UniqueIndex for unique AddressId.

                // Generate fake Addresses for each library (optional).
                .RuleFor(l => l.Address, (f, l) => new Faker<Address>()
                    .RuleFor(a => a.Street, f.Address.StreetAddress())
                    .RuleFor(a => a.City, f.Address.City())
                    .RuleFor(a => a.PostalCode, f.Address.ZipCode())
                    .Generate());

            // Generate fake Books for each library (optional).
            var bookFaker = new Faker<Book>()
                //.RuleFor(b => b.Id, f => f.UniqueIndex)
                .RuleFor(b => b.Title, f => f.Commerce.ProductName().ClampLength(5, 24))
                .RuleFor(b => b.Author, f => f.Name.FullName())
                .RuleFor(b => b.Description, f => f.Lorem.Sentence());
                

            var libraries = faker.Generate(numberOfLibraries);

            foreach (var library in libraries)
            {
                library.Books = bookFaker.Generate(5); // Generate 5 fake books for each library (you can adjust the number as needed).
            }

            return libraries;
        }
    }

    
}
