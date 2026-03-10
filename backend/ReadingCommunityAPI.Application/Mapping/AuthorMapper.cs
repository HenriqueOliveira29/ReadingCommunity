using ReadingCommunityApi.Application.Dto;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.mappers;

public class AuthorMapper : IAuthorMapper
{
    private readonly IBookMapper _bookMapper;

    public AuthorMapper(IBookMapper bookMapper)
    {
        _bookMapper = bookMapper;
    }
    public AuthorDetailDTO MapToDetailDto(Author entity)
    {
        return new AuthorDetailDTO
        {
          Id = entity.Id,
          Name = entity.Name,
          Biography = entity.Biography,
          BirthDate = entity.BirthDate,
          DeathDate = entity.DeathDate,
          Nationality = entity.Nationality,
          ProfileImageUrl = entity.ProfileImageUrl,
          books = entity.Books.Select(b => _bookMapper.MapToListDto(b)).ToList(),
          Age = entity.Age ?? 0,
          isAlive = entity.IsAlive
        };
    }

    public Author MapToEntity(AuthorCreateDTO dto)
    {
        return new Author(dto.Name, dto.Biography, dto.BirthDate, dto.Nationality, dto.ProfileImageUrl);
    }

    public AuthorListDTO MapToListDto(Author entity)
    {
        return new AuthorListDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Nationality = entity.Nationality,
            NumberOfBooks = entity.Books.Count()
        };
    }
}