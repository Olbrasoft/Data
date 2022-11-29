namespace Olbrasoft.Data.Entities.Abstractions;
public interface ICreatorInfo<TUser>
{
    int CreatorId { get; set; }

    TUser Creator { get; set; }
}
