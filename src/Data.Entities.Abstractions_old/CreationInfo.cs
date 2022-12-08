<<<<<<< HEAD
﻿namespace Olbrasoft.Data.Entities;

public abstract class CreationInfo<TUser> : EntityId, ICreatorInfo<TUser>, IHaveCreated
{   
=======
﻿using System.ComponentModel.DataAnnotations;

namespace Olbrasoft.Data.Entities.Abstractions;

public abstract class CreationInfo<TUser> : ICreatorInfo<TUser>, IHaveCreated, IHaveId
{
    public int Id { get; set; }

>>>>>>> aa8b09ca7869a89cd3a323a811fafa00386a955a
    [Required]
    public int CreatorId { get; set; }

    [Required]
    public TUser Creator { get; set; } = default!;

    public DateTimeOffset Created { get; set; }
}