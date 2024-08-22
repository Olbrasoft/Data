using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Data.Entities.Abstractions;

public abstract class CreationInfo<TUser> : BaseEnity, ICreatorInfo<TUser>, IHaveCreated
{
    [Required]
    public int CreatorId { get; set; }

    [Required]
    public TUser Creator { get; set; } = default!;

    public DateTimeOffset Created { get; set; }
}
