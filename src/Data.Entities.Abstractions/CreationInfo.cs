using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Data.Entities.Abstractions;

public abstract class CreationInfo<TUser> : ICreatorInfo<TUser>, IHaveCreated, IHaveId
{
    public int Id { get; set; }

    [Required]
    public int CreatorId { get; set; }

    [Required]
    public TUser Creator { get; set; } = default!;

    public DateTimeOffset Created { get; set; }
}