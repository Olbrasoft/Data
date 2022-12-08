namespace Olbrasoft.Data.Entities.Identity;

public class UserLogin : IdentityUserLogin<int>, IHaveCreated
{
    public DateTimeOffset Created { get; set; }
}