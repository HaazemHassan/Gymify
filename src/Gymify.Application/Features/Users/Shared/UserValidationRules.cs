using FluentValidation;
using Gymify.Application.Common.Options;
using Gymify.Domain.Common.Constants;

namespace Gymify.Application.Features.Users.Shared
{
    public static class UserValidationRules
    {
        public static IRuleBuilderOptions<T, string?> ApplyUserFullNameRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            int minLength = 3,
            int maxLength = 100
        )
        {
            var rule = ruleBuilder;

            return rule
                .MinimumLength(minLength)
                    .WithMessage($"{{PropertyName}} must be at least {minLength} characters")
                .MaximumLength(maxLength)
                    .WithMessage($"{{PropertyName}} cannot exceed {maxLength} characters")
                .Matches(@"^\s*[\p{L}\p{M}]+(?:\s+[\p{L}\p{M}]+)*\s*$")
                    .WithMessage("{PropertyName} must contain only letters and spaces");
        }

        public static IRuleBuilderOptions<T, string?> ApplyNameRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            int minLength = 3,
            int maxLength = 100
        )
        {
            return ruleBuilder.ApplyUserFullNameRules(minLength, maxLength);
        }



        public static IRuleBuilderOptions<T, string?> ApplyPasswordRules<T>(
             this IRuleBuilder<T, string> ruleBuilder,
             AppPasswordOptions settings
        )
        {
            var rule = (IRuleBuilderOptions<T, string>)ruleBuilder;

            rule = rule
                .MinimumLength(settings.MinLength)
                .WithMessage($"{{PropertyName}} must be at least {settings.MinLength} characters");

            if (settings.RequireUppercase)
            {
                rule = rule.Matches("[A-Z]")
                    .WithMessage("{PropertyName} must contain at least one uppercase letter");
            }

            if (settings.RequireLowercase)
            {
                rule = rule.Matches("[a-z]")
                    .WithMessage("{PropertyName} must contain at least one lowercase letter");
            }

            if (settings.RequireDigit)
            {
                rule = rule.Matches("[0-9]")
                    .WithMessage("{PropertyName} must contain at least one number");
            }

            if (settings.RequireNonAlphanumeric)
            {
                rule = rule.Matches(@"[\W_]")
                    .WithMessage("{PropertyName} must contain at least one special character");
            }

            if (settings.RequireDigitOrNonAlphanumeric)
            {
                rule = rule.Matches(@"[0-9\W_]")
                    .WithMessage("{PropertyName} must contain at least one number or symbol");
            }

            return rule;
        }


        public static IRuleBuilderOptions<T, string?> ApplyConfirmPasswordRules<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            Func<T, string> passwordSelector
        )
        {
            var rule = ruleBuilder;
            return rule
                .Must((model, confirmPassword) =>
                {
                    var password = passwordSelector((T)model);
                    return password == confirmPassword;
                })
                .WithErrorCode(ErrorCodes.Authentication.PasswordsDoNotMatch)
                .WithMessage("Passwords do not match");
        }
    }
}

