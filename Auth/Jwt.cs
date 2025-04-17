using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace MrX.Web.Auth;

using IHAB = IHostApplicationBuilder;
using IWA = IApplicationBuilder;

public static class Jwt
{
    private const string Issuer = "MrX ";
    private const string Audience = "Client ";

    private static string? _secretKey;
    private static string? _for;
    private static TimeSpan _validTime = TimeSpan.FromDays(1);

    public static void SetValidTime(TimeSpan validTime) => _validTime = validTime;

    public static IHAB AddJwtService(this IHAB builder, string @for, int seed = 0)
    {
        Jwt._secretKey = MrX.Web.Security.Random.String(512, seed: seed);
        Jwt._for = @for;
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication().AddJwtBearer(
            options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer + @for,
                    ValidAudience = Audience + @for,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_secretKey))
                };
            });
        return builder;
    }

    public static IWA UseJwt(this IWA app)
    {
        app.UseAuthentication();
        return app;
    }

    public static string GenerateJwtToken(params IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey!));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer + Jwt._for,
            audience: Audience + Jwt._for,
            claims: claims,
            expires: DateTime.Now + _validTime,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static JwtSecurityToken DecodeJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey!);
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
        }, out SecurityToken validatedToken);
        return (JwtSecurityToken)validatedToken;
    }
}