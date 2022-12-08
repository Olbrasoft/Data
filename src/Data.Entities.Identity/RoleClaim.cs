namespace Olbrasoft.Data.Entities.Identity;

public class RoleClaim : IdentityRoleClaim<int>, IHaveCreated
{
    public DateTimeOffset Created { get; set; }
}