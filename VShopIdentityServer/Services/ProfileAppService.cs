using Duende.IdentityModel;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VShopIdentityServer.Data;

namespace VShopIdentityServer.Services
{
    public class ProfileAppService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;

        public ProfileAppService(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context, CancellationToken ct)
        {
            string id = context.Subject.GetSubjectId();

            ApplicationUser user = await _userManager.FindByIdAsync(id);

            ClaimsPrincipal userClaims = await _claimsFactory.CreateAsync(user);

            List<Claim> claims = userClaims.Claims.ToList();
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

            if (_userManager.SupportsUserRole)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                foreach (string role in roles)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, role));

                    if (_roleManager.SupportsRoleClaims)
                    {
                        IdentityRole identityRole = await _roleManager.FindByNameAsync(role);
                        if (identityRole != null)
                        {
                            claims.AddRange(await _roleManager.GetClaimsAsync(identityRole));
                        }
                    }
                }
            }
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context, CancellationToken ct)
        {
            string userId = context.Subject.GetSubjectId();

            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            context.IsActive = user != null;
        }
    }
}
