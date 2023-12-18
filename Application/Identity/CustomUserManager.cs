using Domain.ApplicationUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Identity;

public sealed class CustomUserManager<TUser>
    : UserManager<TUser>
    where TUser : ApplicationUser
{
    public CustomUserManager(
        IUserStore<TUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TUser> passwordHasher,
        IEnumerable<IUserValidator<TUser>> userValidators,
        IEnumerable<IPasswordValidator<TUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<TUser>> logger)
        : base(store,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger)
    {

    }

    public override async Task<IdentityResult> CreateAsync(TUser user, string password)
    {
        user.CreatedAtUtc = DateTime.UtcNow;
        return await base.CreateAsync(user, password);
    }
}

