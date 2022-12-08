namespace Olbrasoft.Data.Entities.Identity;

public class UserClaim : IdentityUserClaim<int>, IHaveCreated
{
    public DateTimeOffset Created { get; set; }
}