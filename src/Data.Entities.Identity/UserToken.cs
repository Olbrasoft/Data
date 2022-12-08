namespace Olbrasoft.Data.Entities.Identity;

public class UserToken : IdentityUserToken<int>, IHaveCreated
{
    public DateTimeOffset Created { get; set; }
}