using Application.ProjCqrs.Authentication.Response;
using MediatR;

namespace Application.ProjCqrs.Authentication.Commands.Login;

public class LoginCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}