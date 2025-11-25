using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Interfaces.mappers;

public interface IReviewMapper
{
    Review MapToEntity(ReviewCreateDTO dto, int userId);

    ReviewDetailDTO MapToDetailDto(Review entity);
}