using FluentValidation;

namespace Hexagonal.Application.Common.Validators
{
    /// <summary>
    /// General Guid validator
    /// </summary>
    public class NullableGuidValidator : AbstractValidator<Guid?>
    {
        /// <inheritdoc />
        public NullableGuidValidator()
        {
            RuleFor(r => r).NotEqual(Guid.Empty)
                .WithMessage("Provided Guid value is an empty Guid")
                .When(guid => guid.HasValue);
        }
    }
}
