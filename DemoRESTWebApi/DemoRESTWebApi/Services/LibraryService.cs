using AutoMapper;
using DemoRESTWebApi.Authorization;
using DemoRESTWebApi.Entities;
using DemoRESTWebApi.Exceptions;
using DemoRESTWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;

namespace DemoRESTWebApi.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LibraryService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public LibraryService(LibraryDbContext dbContext, IMapper mapper, ILogger<LibraryService> logger,
            IAuthorizationService authorizationService, IUserContextService userContextService) 
        {
            _dbContext = dbContext; 
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }
        public LibraryDto GetById(int id)
        {
            var library = _dbContext
            .Libraries
            .Include(r => r.Address)
            .Include(r => r.Books)
            .FirstOrDefault(x => x.Id == id);

            if (library == null) { throw new NotFoundException("Library not found"); }

            var result = _mapper.Map<LibraryDto>(library);

            return result;

        }

        public PageResult<LibraryDto> GetAll(LibraryQuery query)
        {

            _logger.LogWarning($"Library - GetAll action invoked");

            var baseQuery = _dbContext
                .Libraries
                .Include(r => r.Address)
                .Include(r => r.Books)
                .Where(r => query.SearchPhrase == null || r.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                || r.Description.ToLower().Contains(query.SearchPhrase.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Library, object>>>
                {
                    { nameof(Library.Name), x => x.Name},
                    { nameof(Library.Description), x => x.Description},
                    { nameof(Library.Category), x => x.Category}
                };

                var selectedColumn = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection ==
                    SortDirection.Asc ? baseQuery.OrderBy(selectedColumn) 
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var libraries = baseQuery.Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            // Map entity to Dto (also List to List).
            var librariesDto = _mapper.Map<List<LibraryDto>>(libraries);

            var result = new PageResult<LibraryDto>(librariesDto, totalItemsCount, query.PageSize, query.PageNumber );

            return result;

        }

        public int CreateLibrary(CreateLibraryDto dto)
        {

            var library = _mapper.Map<Library>(dto);
            library.CreatedById = _userContextService.GetUserId;

            _dbContext.Libraries.Add(library);
            _dbContext.SaveChanges();

            return library.Id;
        }

        public void Delete(int id)
        {
            _logger.LogWarning($"Library id {id} - delete action invoked");

            var library = _dbContext
                .Libraries
                .FirstOrDefault(x => x.Id == id);

            var authResult = _authorizationService
                .AuthorizeAsync(_userContextService.User, library
                , new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authResult.Succeeded) { throw new ForbidException(); }

            if (library == null) { throw new NotFoundException("Library not found"); }

            _dbContext.Libraries.Remove(library);
            _dbContext.SaveChanges();

        }
        public void UpdateLibrary([FromRoute] int id, [FromBody] UpdateLibraryDto updateLibraryDto)
        {
            var library = _dbContext.Libraries.FirstOrDefault(x => x.Id == id);

            if (library == null) { throw new NotFoundException("Library not found"); }

            var authResult = _authorizationService
                .AuthorizeAsync(_userContextService.User, library
                , new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authResult.Succeeded) { throw new ForbidException(); }

            library.Name = updateLibraryDto.Name;
            library.Description = updateLibraryDto.Description;
            library.HasEbooks = updateLibraryDto.HasEbooks;

            // _dbContext.Update(library);
            _dbContext.SaveChanges();

        }
    }
}
