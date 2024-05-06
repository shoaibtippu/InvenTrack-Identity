using Hexagonal.Adapters.Rest.Controllers;
using InvenTrack.Application.Ports.In.User;
using InvenTrack.Application.Ports.In.User.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InvenTrack.Adapters.Rest.Controllers
{
    /// <summary>
    /// Controller for User Accounts.
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : BaseController
    {
        #region Queries

        /// <summary>
        /// Retrieves a list of all users with the role 'User'.
        /// </summary>
        /// <returns>A list of all users with the role 'User'.</returns>
        [HttpGet("GetAllUsers")]
        [SwaggerResponse(200, "List of all users with the role 'User'", typeof(IEnumerable<UserDto>))]
        [SwaggerOperation(Summary = "Retrieves a list of all users with the role 'User'.", Description = "This method will retrieve a list of all users with the role 'User'.")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await userService.GetUsersWithRoleAsync("User");
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a list of all users with the role 'Admin'.
        /// </summary>
        /// <returns>A list of all users with the role 'Admin'.</returns>
        [HttpGet("GetAllAdmins")]
        [SwaggerResponse(200, "List of all users with the role 'Admin'", typeof(IEnumerable<UserDto>))]
        [SwaggerOperation(Summary = "Retrieves a list of all users with the role 'Admin'.", Description = "This method will retrieve a list of all users with the role 'Admin'.")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var response = await userService.GetUsersWithRoleAsync("Admin");
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a user by their email.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>The user with the specified email if found, otherwise returns a 404 Not Found response.</returns>
        [HttpGet("GetUser/{email}")]
        [SwaggerResponse(200, "The user with the specified email.", typeof(UserDto))]
        [SwaggerResponse(404, "User not found")]
        [SwaggerOperation(Summary = "Retrieves a user by their email.", Description = "This method will retrieve a user by their email.")]
        public async Task<IActionResult> GetUser([FromRoute] string email)
        {
            var response = await userService.GetUserByEmailAsync(email);
            return Ok(response);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Registers a new admin.
        /// </summary>
        /// <param name="userDto">The data for creating the admin.</param>
        /// <returns>The newly registered admin.</returns>
        [HttpPost("RegisterAdmin")]
        [SwaggerResponse(201, "Registered admin.", typeof(UserDto))]
        [SwaggerResponse(400, "Invalid request.")]
        [SwaggerOperation(Summary = "Registers a new admin.", Description = "This method will register a new admin.")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserDto userDto)
        {
            var response = await userService.RegisterAdminAsync(userDto);
            return Ok(response);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userDto">The data for creating the user.</param>
        /// <returns>The newly registered user.</returns>
        [HttpPost("RegisterUser")]
        [Authorize(Roles = "Admin")]
        [SwaggerResponse(201, "Registered user.", typeof(UserDto))]
        [SwaggerResponse(400, "Invalid request.")]
        [SwaggerOperation(Summary = "Registers a new user.", Description = "This method will register a new user.")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userDto)
        {
            var response = await userService.RegisterUserAsync(userDto);
            return Ok(response);
        }

        /// <summary>
        /// Authenticates a user and returns a token.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        [HttpPost("Login")]
        [SwaggerResponse(200, "Authentication successful.", typeof(string))]
        [SwaggerResponse(401, "Authentication failed.")]
        [SwaggerOperation(Summary = "Authenticates a user and returns a token.", Description = "This method authenticates a user and returns a JWT token if successful.")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await userService.LoginUserAsync(loginDto);
            return Ok(response);
        }

        /// <summary>
        /// Updates a user by their email.
        /// </summary>
        /// <param name="email">The email of the user to be deleted.</param>
        /// <param name="userDto">The new details of the user.</param>
        /// <returns>A 204 No Content response if successful, otherwise returns a 404 Not Found response.</returns>
        [HttpPut("UpdateUser")]
        [SwaggerResponse(204, "Update user.")]
        [SwaggerResponse(404, "User not found.")]
        [SwaggerOperation(Summary = "Updates a user by their email.", Description = "This method will update a user by their email.")]
        public async Task<IActionResult> UpdateUser([FromRoute] string email, UserDto? userDto)
        {
            var response = await userService.UpdateUserAsync(email, userDto);
            return Ok(response);
        }

        /// <summary>
        /// Deletes a user by their email.
        /// </summary>
        /// <param name="email">The email of the user to be deleted.</param>
        /// <returns>A 204 No Content response if successful, otherwise returns a 404 Not Found response.</returns>
        [HttpDelete("DeleteUser")]
        [SwaggerResponse(204, "Deleted user.")]
        [SwaggerResponse(404, "User not found.")]
        [SwaggerOperation(Summary = "Deletes a user by their email.", Description = "This method will delete a user by their email.")]
        public async Task<IActionResult> DeleteUser([FromBody] string email)
        {
            var response = await userService.DeleteUserAsync(email);
            return Ok(response);
        }

        /// <summary>
        /// Logouts a user 
        /// </summary>

        [HttpPost("Logout")]
        [SwaggerResponse(204, "Logout user.")]
        [SwaggerOperation(Summary = "Logouts a user.", Description = "This method will logout a user.")]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var response = await userService.LogoutUserAsync(token!);
            return Ok(response);
        }   

        #endregion
    }
}
