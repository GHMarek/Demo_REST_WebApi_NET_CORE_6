using DemoRESTWebApi.Models;
using DemoRESTWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoRESTWebApi.Controllers
{
    [Route("api/library/{libraryId}/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService) { _bookService = bookService; }

        // http://localhost:5001/api/library/21/book/
        [HttpPost]
        public ActionResult Post([FromRoute] int libraryId, CreateBookDto createBookDto)
        {
            var newBookId = _bookService.Create(libraryId, createBookDto);

            return Created($"api/library/{libraryId}/book/{newBookId}", null);
        }

        // http://localhost:5001/api/library/21/book/21
        [HttpGet("{bookId}")]
        public ActionResult<BookDto> Get([FromRoute] int libraryId, [FromRoute] int bookId)
        {
            BookDto book = _bookService.GetById(libraryId, bookId);

            return Ok(book);
        }

        // http://localhost:5001/api/library/21/book
        [HttpGet]
        public ActionResult<List<BookDto>> Get([FromRoute] int libraryId)
        {
            var result = _bookService.GetAll(libraryId);
            return Ok(result);
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int libraryId) 
        {
            _bookService.RemoveAll(libraryId);

            return NoContent();
        }

    }
}
