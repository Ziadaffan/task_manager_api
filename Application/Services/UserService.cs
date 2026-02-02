using Application.Interfaces;
using Application.Response;
using Domaine.Classes;
using Infrastructure.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class UserService : IUserService
    {
        const string emailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IConfiguration configuration, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<User> Add(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            const string emailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            if (string.IsNullOrWhiteSpace(user.Email) || !Regex.IsMatch(user.Email, emailPattern))
                throw new ArgumentException("Invalid email format.", nameof(user.Email));
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = HashPassword(user.PasswordHash);
            }

            return await _userRepository.Add(user);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _userRepository.Delete(id);
        }

        public async Task<User> GetById(Guid id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User> Update(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || !Regex.IsMatch(user.Email, emailPattern))
                throw new ArgumentException("Invalid email format.", nameof(user.Email));

            return await _userRepository.Update(user);
        }

        public async Task<LogInResponse> LogIn(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username);

            _logger.LogInformation("Attempting login for user: {Username}", user.UserRole);

            if (user == null)
                throw new InvalidOperationException("Invalid username or password.");


            if(user.PasswordHash != HashPassword(password))
                throw new InvalidOperationException("Invalid username or password.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var response = new LogInResponse
            {
                Role = user.UserRole.ToString(),
                Token = tokenString
            };

            return response;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public async Task<List<User>> GetByOrganisationId(Guid organisationId)
        {
            return await _userRepository.GetByOrganisationId(organisationId);
        }

        public async Task<List<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }
    }
}
