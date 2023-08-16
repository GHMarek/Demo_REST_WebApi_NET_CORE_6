using AutoMapper;
using DemoRESTWebApi.Entities;
using DemoRESTWebApi.Exceptions;
using DemoRESTWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoRESTWebApi.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;

        public BookService(LibraryDbContext libraryDbContext, IMapper mapper)
        {
            _dbContext = libraryDbContext;
            _mapper = mapper;
        }

        public int Create(int libraryId, CreateBookDto createBookDto)
        {
            var library = GetLibraryById(libraryId);

            var result = _mapper.Map<Book>(createBookDto);

            result.LibraryId = libraryId;

            _dbContext.Books.Add(result);
            _dbContext.SaveChanges();

            return result.Id;


        }

        public BookDto GetById(int libraryId, int bookId)
        {
            var library = GetLibraryById(libraryId);

            var book = _dbContext.Books.FirstOrDefault(x => x.Id == bookId);

            if (book == null || book.LibraryId !=  libraryId) 
            {
                throw new NotFoundException("Book not found");
            }

            var bookDto = _mapper.Map<BookDto>(book);

            return bookDto;

        }

        public List<BookDto> GetAll(int libraryId)
        {
            var library = GetLibraryById(libraryId);

            var bookDtos = _mapper.Map<List<BookDto>>(library.Books);
            return bookDtos;
        }


        private Library GetLibraryById(int libraryId)
        {
            var library = _dbContext.Libraries
            .Include(x => x.Books)
            .FirstOrDefault(x => x.Id == libraryId);

            if (library == null) { throw new NotFoundException("Library not found"); }

            return library;
        }

        public void RemoveAll(int libraryId)
        {
            var library = GetLibraryById(libraryId);

            _dbContext.RemoveRange(library.Books);
            _dbContext.SaveChanges();

        }
    }
}
