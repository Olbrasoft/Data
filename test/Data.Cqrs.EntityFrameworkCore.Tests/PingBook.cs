using Olbrasoft.Data.Entities.Abstractions;

namespace Data.Cqrs.EntityFrameworkCore.Tests;
public class PingBook : BaseEnity
{

    public string Title { get; set; } = string.Empty;

}
