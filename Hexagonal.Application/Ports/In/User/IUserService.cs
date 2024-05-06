using InvenTrack.Application.Ports.In.User.Dto;

namespace InvenTrack.Application.Ports.In.User
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsersWithRoleAsync(string roleName);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<UserServiceResponses.GeneralResponse> RegisterUserAsync(UserDto? userDto);
        Task<UserServiceResponses.LoginResponse> LoginUserAsync(LoginDto loginDto);
        Task<UserServiceResponses.GeneralResponse> RegisterAdminAsync(UserDto? userDto);
        Task<UserServiceResponses.GeneralResponse> UpdateUserAsync(string email, UserDto? userDto);
        Task<UserServiceResponses.GeneralResponse> LogoutUserAsync(string token);
        Task<UserServiceResponses.GeneralResponse> DeleteUserAsync(string email);
    }
}
