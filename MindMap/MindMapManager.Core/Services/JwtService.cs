using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;


namespace MindMapManager.Core.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtService(IConfiguration config, UserManager<ApplicationUser> userManager )
        {
           _config = config;
            _userManager = userManager;
        }
        public async Task<AuthenticationResponse> CreateJwtTokenAsync(ApplicationUser applicationUser)
        {
            DateTime expiration = DateTime.UtcNow
                .AddDays(Convert.ToDouble(_config["Jwt:Expiration_Days"]));

            // payload
            List<Claim> Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Email,applicationUser.Email),
                new Claim(ClaimTypes.Name, applicationUser.UserName)
            };
            var roles = await _userManager.GetRolesAsync(applicationUser);
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    Claims.Add(new Claim(ClaimTypes.Role, role));
                }
            } // payload

            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));

            var SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken TokenGenerator = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims:Claims,
                expires: expiration,
                signingCredentials: SigningCredentials);

            JwtSecurityTokenHandler JwtHandler = new JwtSecurityTokenHandler();
            string token = JwtHandler.WriteToken(TokenGenerator);

            return new AuthenticationResponse()
            {
                PersonName = applicationUser.UserName,
                Email = applicationUser.Email,
                Token = token,
                Expiration = expiration,
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(Convert.ToDouble(_config["RefreshToken:Expiration_Days"]))
            };
        }

        public ClaimsPrincipal? GetPrincipaleFromJwtToken(string? jwtToken)
        {
            var TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]))

            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var Claims =  handler.ValidateToken(jwtToken, TokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }

            return Claims;
        }

        private string CreateRefreshToken()
        {
            byte[] bytes = new byte[64];
            var randomGenerator = RandomNumberGenerator.Create();
            randomGenerator.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
