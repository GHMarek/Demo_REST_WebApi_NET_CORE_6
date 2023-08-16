using DemoRESTWebApi.Entities;
using DemoRESTWebApi.Exceptions;
using DemoRESTWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoRESTWebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(LibraryDbContext libraryDbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings) 
        { 
            _dbContext = libraryDbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = new User()
            {
                Email = registerUserDto.Email,
                DateOfBirth = registerUserDto.DateOfBirth,
                Nationality = registerUserDto.Nationality,
                RoleId = registerUserDto.RoleId,
                
            };

            var hashPassword = _passwordHasher.HashPassword(newUser, registerUserDto.Password);

            newUser.PasswordHash = hashPassword;

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }

        public string GenerateJwt(LoginDto loginDto)
        {
            var user = _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefault(u => u.Email == loginDto.Email);

            if (user == null)
            {
                throw new BadRequestException("Invalid user or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid user or password");
            }

            var claims = new List<Claim>() 
            { 
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd"))
            
            };

            // Nationality isn't required so claim is optional.

            if (!string.IsNullOrEmpty(user.Nationality)) 
            {
                claims.Add(new Claim("Nationality", user.Nationality));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddDays(Convert.ToDouble(_authenticationSettings.JwtExpireDays));

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer
                , _authenticationSettings.JwtIssuer
                , claims
                , DateTime.Now
                , expires
                , credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);

        }
    }
}
