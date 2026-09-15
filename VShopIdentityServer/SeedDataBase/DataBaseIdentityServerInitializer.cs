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
        if (_userManager.FindByEmailAsync("admin@vshop.com.br").Result == null)
        {
            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@vshop.com.br",
                NormalizedEmail = "ADMIN@VSHOP.COM.BR",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "+55 (11) 99999-9999",
                FirstName = "Douglas",
                LastName = "Admin",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult resultAdmin = _userManager.CreateAsync(admin, "Admin@123").Result;

            if (resultAdmin.Succeeded)
            {
                _userManager.AddToRoleAsync(admin, IdentityConfiguration.Admin).Wait();

                _ = _userManager.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                    new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
                }).Result;
            }
        }

        if (_userManager.FindByEmailAsync("cliente@vshop.com.br").Result == null)
        {
            ApplicationUser client = new ApplicationUser()
            {
                UserName = "cliente",
                NormalizedUserName = "CLIENTE",
                Email = "cliente@vshop.com.br",
                NormalizedEmail = "CLIENTE@VSHOP.COM.BR",
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "+55 (11) 99999-9999",
                FirstName = "Douglas",
                LastName = "Cliente",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult resultClient = _userManager.CreateAsync(client, "Cliente@123").Result;

            if (resultClient.Succeeded)
            {
                _userManager.AddToRoleAsync(client, IdentityConfiguration.Client).Wait();

                _ = _userManager.AddClaimsAsync(client, new Claim[]
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
