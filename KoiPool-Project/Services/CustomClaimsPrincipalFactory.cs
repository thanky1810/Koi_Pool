using KoiPool_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace KoiPool_Project.Services
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUserModel>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<AppUserModel> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUserModel user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // Thêm các claim tùy chỉnh
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email ?? ""));
            identity.AddClaim(new Claim("Occupation", user.Occupation ?? ""));

            return identity;
        }
    }
}
