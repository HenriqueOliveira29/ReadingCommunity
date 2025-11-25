using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.mappers;

public class ReviewMapper : IReviewMapper
{
    public ReviewMapper()
    {
    }

    public ReviewDetailDTO MapToDetailDto(Review entity)
    {
        if(entity.Book == null)
        {
            throw new InvalidOperationException($"Review '{entity.Id}' has no book assigned.");
        }

        if(entity.User == null)
        {
            throw new InvalidOperationException($"Review '{entity.Id}' has no user assigned.");
        }

        return new ReviewDetailDTO
        {
            Id = entity.Id,
            Rating = entity.Rating,
            Comment = entity.Comment,
            BookId = entity.BookId,
            BookName = entity.Book.Title,
            UserId = entity.UserId,
            UserName = entity.User.UserName,
        };
    }

    public Review MapToEntity(ReviewCreateDTO dto, int userId)
    {
        return new Review(dto.Rating, dto.Comment, dto.BookId, userId);
    }
}