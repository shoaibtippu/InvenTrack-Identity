using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Hexagonal.Application.Common.Constants;
using JetBrains.Annotations;

namespace Hexagonal.Application.Common.Validators
{
    /// <summary>
    /// Extension methods for building fluent validation rules
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class ValidatorExtensions
    {

        /// <summary>
        /// Defines a rule if a string provided is a guid or not
        /// also checks for default/empty guid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustBeValidGuid<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.Must(s => Guid.TryParseExact(s, "D", out var guid) && guid != Guid.Empty)
                .WithMessage("'{PropertyValue}' is not a valid guid");

        /// <summary>
        /// Defines a rule if a string provided is a guid or not if the string is not null
        /// also checks for default/empty guid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string?> MustBeValidGuidOptional<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
            ruleBuilder.Must(s => s == null
                                  || Guid.TryParseExact(s, "D", out var guid) && guid != Guid.Empty)
                .WithMessage("'{PropertyValue}' is not a valid guid");

        /// <summary>
        /// Checks if the string provided is in valid range or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustBeValidTitle<T>(this IRuleBuilder<T, string> ruleBuilder) =>
            ruleBuilder.NotEmpty().MaximumLength(LengthConstraints.Name);

        /// <summary>
        /// Checks if the string provided is in valid range or not if it is not null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string?> MustBeValidTitleOptional<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
            ruleBuilder.MaximumLength(LengthConstraints.Name);
    }
}
