using InvenTrack.Application.Ports.In.User;
using InvenTrack.Application.Ports.In.User.Dto;
using InvenTrack.Application.Ports.Out.Persistence.Interfaces;

namespace InvenTrack.Application.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<List<UserDto>> GetUsersWithRoleAsync(string roleName)
        {
            return await userRepository.GetUsersWithRoleAsync(roleName);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            return await userRepository.GetUserByEmailAsync(email);
        }

        public async Task<UserServiceResponses.GeneralResponse> RegisterUserAsync(UserDto? userDto)
        {
            return await userRepository.RegisterUserAsync(userDto);
        }

        public async Task<UserServiceResponses.GeneralResponse> RegisterAdminAsync(UserDto? userDto)
        {
            return await userRepository.RegisterAdminAsync(userDto);
        }

        public Task<UserServiceResponses.GeneralResponse> UpdateUserAsync(string email, UserDto? userDto)
        {
            return userRepository.UpdateUserAsync(email, userDto);
        }

        public async Task<UserServiceResponses.LoginResponse> LoginUserAsync(LoginDto loginDto)
        {
            return await userRepository.LoginUserAsync(loginDto);
        }

        public async Task<UserServiceResponses.GeneralResponse> LogoutUserAsync(string token)
        {
            return await userRepository.LogoutUserAsync(token);
        }

        public async Task<UserServiceResponses.GeneralResponse> DeleteUserAsync(string email)
        {
            return await userRepository.DeleteUserAsync(email);
        }
    }
}
