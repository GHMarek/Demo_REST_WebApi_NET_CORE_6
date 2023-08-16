using DemoRESTWebApi.Models;
using System.Security.Claims;

namespace DemoRESTWebApi.Services
{
    public interface ILibraryService
    {
        int CreateLibrary(CreateLibraryDto dto);
        PageResult<LibraryDto> GetAll(LibraryQuery query);
        LibraryDto GetById(int id);
        void Delete(int id);
        void UpdateLibrary(int id, UpdateLibraryDto updateLibrartDto);
    }
}