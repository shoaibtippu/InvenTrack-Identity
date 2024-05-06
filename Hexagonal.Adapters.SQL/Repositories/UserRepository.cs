using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hexagonal.Application.Common.Exceptions.EntityExceptions;
using InvenTrack.Application.Models;
using InvenTrack.Application.Ports.In.User.Dto;
using InvenTrack.Application.Ports.Out.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static InvenTrack.Application.Ports.In.User.Dto.UserServiceResponses;

namespace InvenTrack.Adapters.SQL.Repositories
{
    public class UserRepository(UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IConfiguration config,
        IDistributedCache cache) : IUserRepository
    {
        private readonly List<string> _blacklistedTokens = new List<string>();
        
        #region GetMethods

        public async Task<List<UserDto>> GetUsersWithRoleAsync(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new ArgumentException($"Role '{roleName}' not found.");
            }

            var usersInRole = new List<UserDto>();
            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains(roleName))
                {
                    usersInRole.Add(new UserDto
                    {
                        Name = user.Name,
                        Email = user.Email
                    });
                }
            }
            return usersInRole;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new EntityNotFound();
            }
            return new UserDto
            {
                Name = user.Name,
                Email = user.Email
            };
        }

        #endregion

        public async Task<GeneralResponse> RegisterUserAsync(UserDto? userDto)
        {
            return await RegisterAsync(userDto, "User");
        }

        public async Task<GeneralResponse> RegisterAdminAsync(UserDto? userDto)
        {
            return await RegisterAsync(userDto, "Admin");
        }

        public async Task<LoginResponse> LoginUserAsync(LoginDto loginDto)
        {
            if (loginDto == null!)
                return new LoginResponse(false, null!, "Login container is empty");

            var getUser = await userManager.FindByEmailAsync(loginDto.Email!);
            if (getUser is null)
                return new LoginResponse(false, null!, "User not found");

            bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginDto.Password!);
            if (!checkUserPasswords)
                return new LoginResponse(false, null!, "Invalid email/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);
            return new LoginResponse(true, token!, "Login completed");
        }

        public async Task<GeneralResponse> UpdateUserAsync(string email, UserDto? userDto)
        {
            if (string.IsNullOrEmpty(email) || userDto == null)
                return new GeneralResponse(false, "Email or User DTO is null or empty");

            // Optionally, check if the provided email matches the email in the UserDto
            if (!email.Equals(userDto.Email, StringComparison.OrdinalIgnoreCase))
                return new GeneralResponse(false, "Provided email does not match email in the user data");

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return new GeneralResponse(false, "User not found");

            // Update user properties
            user.Name = userDto.Name ?? user.Name; // Only update if the new value is not null
            // Assuming you want to allow updating the email to the new value in UserDto
            user.Email = userDto.Email;

            // Update the user in the database
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Optionally, aggregate the errors into a single string message
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return new GeneralResponse(false, $"Failed to update user: {errorMessage}");
            }

            return new GeneralResponse(true, "User successfully updated");
        }

        public async Task<GeneralResponse> DeleteUserAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new GeneralResponse(false, "User not found");
            }

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // Optionally, aggregate the errors into a single string message
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return new GeneralResponse(false, $"Failed to delete user: {errorMessage}");
            }

            return new GeneralResponse(true, "User successfully deleted");
        }

        public async Task<GeneralResponse> LogoutUserAsync(string token)
        {
            await BlacklistTokenAsync(token);
            return new GeneralResponse(true, "Successful logged out");
        }

        #region PrivateMethods

        private async Task BlacklistTokenAsync(string token)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            };
            await cache.SetStringAsync(token, "blacklisted", options);
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role!)
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<GeneralResponse> RegisterAsync(UserDto? userDto, string roleName)
        {
            if (userDto is null || userDto.Email is null || userDto.Password is null)
                return new GeneralResponse(false, "Model is empty, email, or password is null");

            var newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                UserName = userDto.Email
            };

            var userExists = await userManager.FindByEmailAsync(newUser.Email);
            if (userExists != null)
                return new GeneralResponse(false, "User registered already");

            var createUserResult = await userManager.CreateAsync(newUser, userDto.Password);
            if (!createUserResult.Succeeded)
                return new GeneralResponse(false, "Error occurred.. please try again");

            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });
            }

            await userManager.AddToRoleAsync(newUser, roleName);
            return new GeneralResponse(true, $"Account Created with {roleName} role");
        }

        #endregion

    }
}
