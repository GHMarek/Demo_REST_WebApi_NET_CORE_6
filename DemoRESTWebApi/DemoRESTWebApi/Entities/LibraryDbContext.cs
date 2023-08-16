using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DemoRESTWebApi.Entities
{
    public class LibraryDbContext : DbContext
    {
        private string _connection =
            "Data Source=DESKTOP-0EEPPO3\\MSSQLSERVER01;Initial Catalog=LibraryDb;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        public DbSet<Library> Libraries { get; set;}
        public DbSet<Address> Addresses { get; set;}
        public DbSet<Book> Books { get; set;}
        public DbSet<User> Users { get; set;}
        public DbSet<Role> Roles { get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Library>()
                .Property(a => a.Name).IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Book>()
                .Property(d => d.Title).IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Address>()
                .Property(e => e.Street).IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(f => f.City).IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(f => f.Email).IsRequired();

            modelBuilder.Entity<Role>()
                .Property(f => f.Name).IsRequired();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connection);

        }
    }
}
