using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MrX.Web.Auth;

using IHAB = IHostApplicationBuilder;

public static class Jwt
{
    private const string Issuer = "MrX ";
    private const string Audience = "Client ";

    private static string? _secretKey;
    private static string? _for;
    private static TimeSpan _validTime = TimeSpan.FromDays(1);

    public static void SetValidTime(TimeSpan validTime) => _validTime = validTime;
    public static string GetSecretKey() => _secretKey ?? throw new NullReferenceException("First Add Jwt To Services");
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="for"></param>
    /// <param name="isDefault">by default jwt is not default scheme</param>
    /// <param name="seed">use seed if secretKey is null</param>
    /// <param name="secretKey"></param>
    /// <param name="usepem"></param>
    /// <param name="certPemFilePath">file by RFC 7468 PEM encoded certificate and private key</param>
    /// <returns></returns>
    public static AuthenticationBuilder AddJwtAuthentication(this IHAB builder,
        string @for,
        bool isDefault = false,
        int seed = 0,
        string? secretKey = null,
        bool usepem = false,
        string? certPemFilePath = null)
    {
        Jwt._secretKey = secretKey ?? Security.Random.String(512, seed: seed);
        Jwt._for = @for;
        AuthenticationBuilder d = (isDefault) ? builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) : builder.Services.AddAuthentication();
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        if (usepem == true && certPemFilePath is not null)
        {
            key = new X509SecurityKey(System.Security.Cryptography.X509Certificates.X509Certificate2.CreateFromPemFile(certPemFilePath));
        }
        d.AddJwtBearer(
            options =>
            {
                options.RequireHttpsMetadata = false;
                options.Audience = Audience + @for;
                options.Authority = Issuer + @for;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer + @for,
                    ValidAudience = Audience + @for,
                    IssuerSigningKey = key,
                };
                options.MapInboundClaims = false;
            });
        return d;
    }

    public static string GenerateJwtToken(params IEnumerable<Claim> claims)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_secretKey!));
        SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: Issuer + Jwt._for,
            audience: Audience + Jwt._for,
            claims: claims,
            expires: DateTime.Now + _validTime,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static JwtSecurityToken DecodeJwtToken(string token)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(_secretKey!);
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