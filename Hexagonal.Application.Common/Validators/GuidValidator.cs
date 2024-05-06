using FluentValidation;

namespace Hexagonal.Application.Common.Validators
{
    /// <summary>
    /// General Guid validator and doesn't allow empty guid
    /// </summary>
    public class GuidValidator : AbstractValidator<Guid>
    {
        /// <inheritdoc />
        public GuidValidator()
        {
            RuleFor(r => r).NotEqual(Guid.Empty)
                .WithMessage("Provided Guid value is an empty Guid");
        }
    }
}
