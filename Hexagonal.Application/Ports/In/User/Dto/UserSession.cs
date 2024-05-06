namespace InvenTrack.Application.Ports.In.User.Dto
{
    public record UserSession(Guid? Id, string? Name ,string? Email, string? Role);
}
