using devhouse.Context;
using devhouse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Services;

public class AuthService
{
    public DatabaseContext _ctx { get; set; }
    public TokenService _ts { get; set; }
    public AuthService(DatabaseContext context, TokenService tokenService) => (_ctx, _ts) = (context, tokenService);

    public async Task<(bool authenticated, string token)> Authenticate(string email, string password)
    {
        var entity = await _ctx.Developers.Include(e => e.Role) // We have to include roles so we can add the name to the JWT token
                                            .Where(e => e.Email == email)
                                            .FirstOrDefaultAsync();

        if (entity == null) return (authenticated: false, token: string.Empty);

        var isValidPassword = new PasswordHasher<Developer>().VerifyHashedPassword(entity, entity.Password!, password);

        if (isValidPassword == PasswordVerificationResult.Success)
            return (authenticated: true, token: _ts.Generate(entity));

        return (authenticated: false, token: string.Empty);
    }
}