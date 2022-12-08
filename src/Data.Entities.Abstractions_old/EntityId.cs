namespace Olbrasoft.Data.Entities;

public abstract class EntityId : IHaveId
{
    [Key]
    public int Id { get; set; }
}