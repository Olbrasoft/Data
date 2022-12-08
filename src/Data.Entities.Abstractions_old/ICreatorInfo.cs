<<<<<<< HEAD
﻿namespace Olbrasoft.Data.Entities;
=======
﻿namespace Olbrasoft.Data.Entities.Abstractions;
>>>>>>> aa8b09ca7869a89cd3a323a811fafa00386a955a
public interface ICreatorInfo<TUser>
{
    int CreatorId { get; set; }

    TUser Creator { get; set; }
}
