namespace VShopIdentityServer.SeedDataBase;

public interface IDataBaseSeedInitializer
{
    void InitializeSeedRoles();
    void InitializeSeedUsers();
}
