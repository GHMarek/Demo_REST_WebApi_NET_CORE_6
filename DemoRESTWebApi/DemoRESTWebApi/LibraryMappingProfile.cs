using AutoMapper;
using DemoRESTWebApi.Entities;
using DemoRESTWebApi.Models;

namespace DemoRESTWebApi
{
    public class LibraryMappingProfile : Profile
    {
        // We have to map only properties of different names or coming from another entity.
        /* When the source property is a member of the source object and the destination property 
         * is a member of the destination object, you'll see the DTO on the left and the entity on the right
         * 
         * When the source property is a member of the DTO and the destination property 
         * is a member of the entity, you'll see the DTO on the right and the entity on the left
         */

        public LibraryMappingProfile() 
        {
            CreateMap<Library, LibraryDto>()
                .ForMember(x => x.City, y => y.MapFrom(z => z.Address.City))
                .ForMember(x => x.Street, y => y.MapFrom(z => z.Address.Street))
                .ForMember(x => x.PostalCode, y => y.MapFrom(z => z.Address.PostalCode));

            CreateMap<Book, BookDto>();

            CreateMap<CreateLibraryDto, Library>()
                .ForMember(x => x.Address, y => 
                    y.MapFrom(z => new Address() { City = z.City, Street = z.Street, PostalCode = z.PostalCode }));

            CreateMap<CreateBookDto, Book>();

            CreateMap<RegisterUserDto, User>()
                .ForMember(x => x.PasswordHash, y => y.MapFrom(z => z.Password));


        }
    }
}
