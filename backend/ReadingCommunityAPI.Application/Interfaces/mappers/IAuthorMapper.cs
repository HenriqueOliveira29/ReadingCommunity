using ReadingCommunityApi.Application.Dto;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Interfaces.mappers;

public interface IAuthorMapper
{
    Author MapToEntity(AuthorCreateDTO dto);
    AuthorDetailDTO MapToDetailDto(Author entity);
    AuthorListDTO MapToListDto(Author entity);    

}