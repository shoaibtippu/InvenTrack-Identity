namespace InvenTrack.Application.Ports.In.User.Dto
{
    public class UserServiceResponses
    {
        public record class LoginResponse(bool Flag, string Token, string Message);
        public record class GeneralResponse(bool Flag, string Message);
    }
}
