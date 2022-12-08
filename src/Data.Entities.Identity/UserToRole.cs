namespace Olbrasoft.Data.Entities.Identity;

public class UserToRole : IdentityUserRole<int>, IHaveCreated 
{   
    public DateTimeOffset Created { get; set; }
}