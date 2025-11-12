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
        CreateMap<BookCreateDTO, Book>()
            .ForMember(dest => dest.Author, opt => opt.Ignore())
            .ForMember(dest => dest.PublictionDate, opt => opt.MapFrom(src => src.PublicationDate.ToUniversalTime()));

        CreateMap<Book, BookDetailDTO>()
        .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name.ToString()))
        .ForMember(dest => dest.PublicationDate, opt => opt.MapFrom(src => src.PublictionDate.ToUniversalTime()));

        CreateMap<Book, BookListDTO>().ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name.ToString()));

        // Books Author
        CreateMap<Author, AuthorListDTO>();
        CreateMap<AuthorCreateDTO, Author>();
        CreateMap<Author, AuthorDetailDTO>();


        // Reviews

        CreateMap<ReviewCreateDTO, Review>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());        
        CreateMap<Review, ReviewDetailDTO>()
            .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));


    }
}