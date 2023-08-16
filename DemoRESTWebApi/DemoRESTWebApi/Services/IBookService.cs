using DemoRESTWebApi.Entities;
using DemoRESTWebApi.Models;

namespace DemoRESTWebApi.Services
{
    public interface IBookService
    {
        int Create(int restaurantId, CreateBookDto createBookDto);
        BookDto GetById(int libraryId, int bookId);
        public List<BookDto> GetAll(int libraryId);
        public void RemoveAll(int libraryId);
    }
}