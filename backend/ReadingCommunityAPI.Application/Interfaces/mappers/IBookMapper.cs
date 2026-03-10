using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Interfaces.mappers;

public interface IBookMapper
{
    Book MapToEntity(BookCreateDTO dto);
    BookListDTO MapToListDto(Book entity);
    BookDetailDTO MapToDetailDto(Book entity);
}