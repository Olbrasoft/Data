namespace Olbrasoft.Data.Entities.Identity;

public class Role : IdentityRole<int>, IHaveCreated
{
    public DateTimeOffset Created { get; set; }
}