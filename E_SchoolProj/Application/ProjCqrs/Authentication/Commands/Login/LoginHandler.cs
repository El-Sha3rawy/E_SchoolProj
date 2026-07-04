using Application.Authentication;
using Application.Common;
using Application.Dtos;
using Application.ProjCqrs.Authentication.Response;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProjCqrs.Authentication.Commands.Login
{
  
    public class LoginCommandHandler
        : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _context = context;
        }

        public async Task<Result<AuthResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            // Get User
            var user = await _userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(
                    x => x.Email == request.Email,
                    cancellationToken);

            if (user is null)
                return Result<AuthResponse>.Failed("Invalid email or password.");

            // Check Active
            if (!user.IsActive)
                return Result<AuthResponse>.Failed("Your account has been deactivated.");

            // Check Password
            var validPassword =
                await _userManager.CheckPasswordAsync(
                    user,
                    request.Password);

            if (!validPassword)
                return Result<AuthResponse>.Failed("Invalid email or password.");

            // Generate Tokens
            var tokenResult =
                await _jwtService.GenerateTokensAsync(user);

            // Save Refresh Token
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = tokenResult.RefreshToken,
                ExpiresOn = tokenResult.RefreshTokenExpiration,
                CreatedOn = DateTime.UtcNow
            });

            // Update Login Date
            user.LastLogin = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            // Roles
            var roles =
                await _userManager.GetRolesAsync(user);

            // Response
            var response = new AuthResponse
            {
                Success = true,
                Message = "Login successful.",

                AccessToken = tokenResult.AccessToken,

                RefreshToken = tokenResult.RefreshToken,

                ExpiresAt = tokenResult.AccessTokenExpiration,

                ForceChangePassword = user.ForceChangedPassword,

                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Roles = roles
                }
            };

            return Result<AuthResponse>.Succeeded(response);
        }
    }
}
