namespace Hexagonal.Application.Common.Dto
{
    public class GetAllBaseDto : IBaseDto
    {
        public PageParameters PageParameters { get; set; } = default!;
    }
}
