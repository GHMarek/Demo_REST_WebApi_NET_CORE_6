using AutoMapper;
using DemoRESTWebApi.Entities;
using DemoRESTWebApi.Models;
using DemoRESTWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Drawing;

namespace DemoRESTWebApi.Controllers
{
    // [ApiController] implements model validation on every request,
    // checking ModelState in methods becomes obsolete.

    // If [Authorize] is added at level of class, it affects every method.
    // We can escape [Authorize] in method by adding [AllowAnnonymous].
    // Method attribute priority is higher.
    [Route("api/library")]
    [ApiController]
    [Authorize]
    public class LibraryController : ControllerBase
    {
        private ILibraryService _libraryService;

        // Pass Imapper from automapper to constructor,
        // so entites are mapped to Dto according to LibraryMappingProfile class.
        public LibraryController(ILibraryService libraryService, LibraryDataSeeder seeder) 
        {
            _libraryService = libraryService;
            seeder.Seed();
        }

        // Get all libraries. Attribute is optional, but let's have it everytime.
        // http://localhost:5001/api/library
        // http://localhost:5001/api/library?SearchPhrase=l&PageSize=1&PageNumber=1
        // Added Authorization attribute, as example in this method.
        // Authorize with custom policy, registered in startup.cs.
        // In this ex., can't access without Nationality claim.
        [HttpGet]
        //[Authorize]
        [Authorize(Policy = "HasNationality")]
        [Authorize(Policy = "AtLeast20yo")]
        public ActionResult<IEnumerable<LibraryDto>> GetAll([FromQuery]LibraryQuery query)
        {
            // Map entity to Dto (also List to List).
            var librariesDto = _libraryService.GetAll(query);

            return Ok(librariesDto);
        }

        // Get library by id passed in route.
        // http://localhost:5001/api/library/24
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult<Library> GetById([FromRoute] int id)
        {
            var libraryById = _libraryService.GetById(id);

            return libraryById == null ? NotFound() : Ok(libraryById);

        }

        // Add new library.
        // http://localhost:5001/api/library
        /*{"Name" : "Biblioteka Powiatowa",
            "Description" : "Powiatowa biblioteka w Naleczowie", 
            "Category" : "Szkolna",
            "HasEbooks" : true,
            "ContactEmail" : "bib_naleczow@wp.pl",
            "ContactNumber" : "500-899-182",
            "City" : "Naleczow",
            "Street" : "Gorzowska",
            "PostalCode" : "55-600"}
         */
        [HttpPost]
        public ActionResult CreateLibrary([FromBody] CreateLibraryDto dto)
        {

            // Check if passed dto is valid.
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var resultId = _libraryService.CreateLibrary(dto);

            return Created($"/api/restaurants/{resultId}", null);
        }

        // Deletes library by id.
        // http://localhost:5001/api/library/26
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _libraryService.Delete(id);

            return Ok();


        }

        // Update library by id and data from body.
        //
        /*{ "Name" : "Library of Santa Fe",
        "Description" : "Public City Library",
        "HasEbooks" : true}
        */
        [HttpPut("{id}")]
        public ActionResult UpdateLibrary([FromRoute] int id, [FromBody] UpdateLibraryDto updateLibraryDto)
        {
            //if (!ModelState.IsValid) { return NotFound(); }

            _libraryService.UpdateLibrary(id, updateLibraryDto);

            return Ok();

        }

}
}
