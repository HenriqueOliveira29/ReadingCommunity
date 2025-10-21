using AutoMapper;
using ReadingCommunityApi.Core.Dto;
using ReadingCommunityApi.Core.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Core.Mapping;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        //Books Mapper
        CreateMap<BookCreateDto, Book>()
            .ForMember(dest => dest.Author, opt => opt.Ignore());
        
        CreateMap<Book, BookListDTO>().ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name.ToString()));

        // Books Author
        CreateMap<Author, AuthorListDTO>();
        CreateMap<AuthorCreateDTO, Author>();
        CreateMap<Author, AuthorDetailDTO>();

       
    }
}