using Duende.IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VShopIdentityServer.Configuration;
using VShopIdentityServer.Data;

namespace VShopIdentityServer.SeedDataBase;

public class DataBaseIdentityServerInitializer : IDataBaseSeedInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DataBaseIdentityServerInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void InitializeSeedRoles()
    {
        if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Admin).Result)
        {
            IdentityRole roleAdmin = new IdentityRole();
            roleAdmin.Name = IdentityConfiguration.Admin;
            roleAdmin.NormalizedName = IdentityConfiguration.Admin.ToUpper();
            _roleManager.CreateAsync(roleAdmin).Wait();
        }

        if (!_roleManager.RoleExistsAsync(IdentityConfiguration.Client).Result)
        {
            IdentityRole roleClient = new IdentityRole();
            roleClient.Name = IdentityConfiguration.Client;
            roleClient.NormalizedName = IdentityConfiguration.Client.ToUpper();
            _roleManager.CreateAsync(roleClient).Wait();
        }
    }

    public void InitializeSeedUsers()
    {
        if(_userManager.FindByEmailAsync("admin1@google.com").Result == null)
        {
            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "admin1",
                NormalizedUserName = "ADMIN1",
                Email = "admin1@google.com",
                NormalizedEmail = "ADMIN1@GOOGLE.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "+55 (11) 99999-9999",
                FirstName = "Braia",
                LastName = "Senju",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            
            IdentityResult resultAdmin = _userManager.CreateAsync(admin, "Admin123*").Result;

            if(resultAdmin.Succeeded)
            {
                _userManager.AddToRoleAsync(admin, IdentityConfiguration.Admin).Wait();

                var adminClaims = _userManager.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                    new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
                }).Result;
            }

            if(_userManager.FindByEmailAsync("client1@google.com").Result == null)
            {
                ApplicationUser client = new ApplicationUser()
                {
                    UserName = "client1",
                    NormalizedUserName = "CLIENT1",
                    Email = "client1@google.com",
                    NormalizedEmail = "CLIENT1@GOOGLE.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "+55 (11) 99999-9999",
                    FirstName = "Client",
                    LastName = "One",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult resultClient = _userManager.CreateAsync(client, "Client123*").Result;

                if(resultClient.Succeeded)
                {
                    _userManager.AddToRoleAsync(client, IdentityConfiguration.Client).Wait();

                    var clientClaims = _userManager.AddClaimsAsync(client, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                        new Claim(JwtClaimTypes.GivenName, client.FirstName),
                        new Claim(JwtClaimTypes.FamilyName, client.LastName),
                        new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
                    }).Result;
                }
            }
        }
    }
}
