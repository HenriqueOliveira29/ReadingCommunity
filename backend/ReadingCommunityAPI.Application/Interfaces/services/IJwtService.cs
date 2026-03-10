using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}