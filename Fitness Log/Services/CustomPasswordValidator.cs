using Microsoft.AspNetCore.Identity;

namespace Fitness_Log.Services
{
    // Enforce password policy: at least 2 uppercase, 3 digits, 3 symbols
    public class CustomPasswordValidator : IPasswordValidator<IdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string? password)
        {
            var errors = new List<IdentityError>();

            if (string.IsNullOrEmpty(password))
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "PasswordNull", Description = "Password cannot be empty." }));

            if (password.Count(char.IsUpper) < 2)
                errors.Add(new IdentityError { Code = "PasswordUppercase", Description = "Password must have at least 2 uppercase letters." });

            if (password.Count(char.IsDigit) < 3)
                errors.Add(new IdentityError { Code = "PasswordDigits", Description = "Password must have at least 3 digits." });

            if (password.Count(ch => !char.IsLetterOrDigit(ch)) < 3)
                errors.Add(new IdentityError { Code = "PasswordSymbols", Description = "Password must have at least 3 symbols." });

            if (errors.Any())
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
