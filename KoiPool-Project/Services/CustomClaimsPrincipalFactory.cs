using KoiPool_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

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

            // Add custom claims
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email ?? ""));
            identity.AddClaim(new Claim("PhoneNumber", user.PhoneNumber ?? ""));
            identity.AddClaim(new Claim("Address", user.Address ?? ""));
            identity.AddClaim(new Claim("Occupation", user.Occupation ?? ""));
            identity.AddClaim(new Claim("Name", user.Name ?? ""));

            return identity;
        }
    }
}
