namespace Olbrasoft.Data.Entities.Identity;

public class User : IdentityUser<int>, IHaveCreated
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;

    public DateTimeOffset Created { get; set; }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}

