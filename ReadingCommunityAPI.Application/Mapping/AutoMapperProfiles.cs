using AutoMapper;
using ReadingCommunityApi.Application.Dto;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Mapping;

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