using System.Xml;
using AutoMapper.Internal.Mappers;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.mappers;

public class BookMapper : IBookMapper
{
    private readonly IReviewMapper _reviewMapper;

    public BookMapper(IReviewMapper reviewMapper)
    {
        _reviewMapper = reviewMapper;
    }

    public BookDetailDTO MapToDetailDto(Book entity)
    {
        if(entity.Author == null)
        {
            throw new InvalidOperationException($"Book '{entity.Title}' has no author assigned.");
        }

        return new BookDetailDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            AuthorId = entity.AuthorId,
            AuthorName = entity.Author.Name,
            Description = entity.Description,
            PublicationDate = entity.PublicationDate,
            CoverImageUrl = entity.CoverImageUrl,
            NumberOfPages = entity.NumberOfPages,
            Dimensions = entity.Dimensions,
            Images = entity.Images.Select(t => t.ImageUrl).ToList(),
            Categories = entity.Categories.Select(t => t.Name).ToList(),
            Reviews = entity.Reviews.Select(r => _reviewMapper.MapToDetailDto(r)).ToList()
        };
    }

    public Book MapToEntity(BookCreateDTO dto)
    {
        return new Book(dto.Title, dto.Description, 
        dto.PublicationDate, dto.AuthorId, dto.NumberOfPages, 
        dto.Width, dto.Height, dto.Depth, dto.CoverImageUrl);
    }

    public BookListDTO MapToListDto(Book entity)
    {
        if(entity.Author == null)
        {
            throw new InvalidOperationException($"Book '{entity.Title}' has no author assigned.");
        }

        return new BookListDTO
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            PublicationDate = entity.PublicationDate,
            AuthorName = entity.Author.Name,
            CoverImageUrl = entity.CoverImageUrl,
            Categories = entity.Categories.Select(t => t.Name).ToList()
        };
    }
}