using AutoMapper;
using BookstoreApi.Dtos;
using BookstoreApi.Models;
using System.Linq;

namespace BookstoreApi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Model -> DTO
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Authors, opt =>
                    opt.MapFrom(src => string.Join(", ", src.Authors)));

            // CreateBookDto -> Model
            CreateMap<CreateBookDto, Book>();

            // UpdateBookDto -> Model
            // AutoMapper: map only non-null values
            CreateMap<UpdateBookDto, Book>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    (!(srcMember is ICollection<object> coll) || coll.Count > 0)));

            CreateMap<Book, BookDto>();
        }
    }
}
