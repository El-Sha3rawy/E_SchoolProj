using Domain.Entities;

namespace Application.Authentication;

public interface IJwtService
{
    Task<string> GenerateAccessTokenAsync(ApplicationUser user);

}
